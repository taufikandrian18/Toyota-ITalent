using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
  public class CmsMenuService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly ClaimHelper HelperMan;

    public CmsMenuService(TalentContext db, IContextualService contextual, ClaimHelper hm)
    {
      this.DB = db;
      this.ContextMan = contextual;
      this.HelperMan = hm;
    }
    public string GetUserLogin()
    {
      try
      {
        return ContextMan.CookieClaims.EmployeeId;
      }
      catch
      {
        return "SYSTEM";
      }
    }
    public async Task<CmsMenuPaginate> GetAllCmsMenu(CmsMenuGridFilter filter)
    {

      var query = (from sl in this.DB.Cms_Menus
                   where sl.IsDeleted == false
                   select new CmsMenuModel
                   {
                     Cms_MenuId = sl.Cms_MenuId,
                     Cms_MenuName = sl.Cms_MenuName,
                     Cms_MenuCategory = sl.Cms_MenuCategory,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.Cms_MenuName))
      {
        query = query.Where(Q => Q.Cms_MenuName.Contains(filter.Cms_MenuName));
      }

      if (!string.IsNullOrEmpty(filter.Cms_MenuCategory))
      {
        if(filter.Cms_MenuCategory == "SPWA") {
          query = query.Where(Q => (Q.Cms_MenuCategory == filter.Cms_MenuCategory) || ((Q.Cms_MenuCategory == "TEST DRIVE")));
        } else {
          query = query.Where(Q => Q.Cms_MenuCategory == filter.Cms_MenuCategory);
        }
      }

      //sort
      switch (filter.SortBy)
      {
        case "CmsMenuNameAsc":
          query = query.OrderBy(Q => Q.Cms_MenuName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "CmsMenuNameDesc":
          query = query.OrderByDescending(Q => Q.Cms_MenuName);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.Cms_MenuName);
          break;
      }
      var cmsMenus = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new CmsMenuPaginate
      {
        CmsMenus = cmsMenus,
        TotalCmsMenus = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertCmsMenuAsync(CmsMenuCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await ValidateCmsMenuByNameAsync(model.Cms_MenuName, model.Cms_MenuCategory);

            if (isExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var newCmsMenus = new Cms_Menus
            {
                Cms_MenuName = model.Cms_MenuName,
                Cms_MenuCategory = model.Cms_MenuCategory,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.Cms_Menus.Add(newCmsMenus);

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateCmsMenuByNameAsync(string Cms_MenuName, string Cms_MenuCategory)
    {
      var isExist = await this
          .DB
          .Cms_Menus
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.Cms_MenuName == Cms_MenuName && Q.Cms_MenuCategory == Cms_MenuCategory);

      return isExist;
    }
    public async Task<CmsMenuModel> GetCmsMenuById(Guid Cms_MenuId)
    {
      var data = await (from mo in DB.Cms_Menus
                        where mo.Cms_MenuId == Cms_MenuId && mo.IsDeleted == false
                        select new CmsMenuModel
                        {
                          Cms_MenuId = mo.Cms_MenuId,
                          Cms_MenuName = mo.Cms_MenuName,
                          Cms_MenuCategory = mo.Cms_MenuCategory,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedAt = mo.UpdatedAt,
                          UpdatedBy = mo.UpdatedBy
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateCmsMenuAsync(CmsMenuUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateCmsMenuNameAsync(model.Cms_MenuName, model.Cms_MenuId, model.Cms_MenuCategory);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var cmsMenuToUpdate = await this.DB.Cms_Menus.Where(Q => Q.Cms_MenuId == model.Cms_MenuId).FirstOrDefaultAsync();

            cmsMenuToUpdate.Cms_MenuId = model.Cms_MenuId;
            cmsMenuToUpdate.Cms_MenuName = model.Cms_MenuName;
            cmsMenuToUpdate.Cms_MenuCategory = model.Cms_MenuCategory;
            cmsMenuToUpdate.UpdatedAt = thisDate;
            cmsMenuToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateUpdateCmsMenuNameAsync(string Cms_MenuName, Guid Cms_MenuId, string cms_MenuCategory)
    {
      var data = await this
          .DB
          .Cms_Menus
          .AsNoTracking()
          .Where(Q => Q.Cms_MenuId == Cms_MenuId)
          .FirstOrDefaultAsync();

      var isChange = data.Cms_MenuName != Cms_MenuName;

      var isExist = await this.ValidateCmsMenuByNameAsync(Cms_MenuName, cms_MenuCategory);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteCmsMenuAsync(DeleteCmsMenuModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.Cms_Menus.Where(Q => Q.Cms_MenuId == model.Cms_MenuId).FirstOrDefaultAsync();

            module.IsDeleted = true;
            module.UpdatedAt = thisDate;
            module.UpdatedBy = this.HelperMan.GetLoginUserId();


            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk(null);
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
  }
}
