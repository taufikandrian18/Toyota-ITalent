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
  public class KodawariMenuService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly ClaimHelper HelperMan;

    public KodawariMenuService(TalentContext db, IContextualService contextual, ClaimHelper hm)
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
    public async Task<KodawariMenuPaginate> GetAllKodawariMenu(KodawariMenusGridFilter filter)
    {

      var query = (from sl in this.DB.KodawariMenus
                   where sl.IsDeleted == false
                   select new KodawariMenuModel
                   {
                     KodawariMenuId = sl.KodawariMenuId,
                     KodawariMenuName = sl.KodawariMenuName,
                     KodawariMenuSequence = sl.KodawariMenuSequence,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.KodawariMenuName))
      {
        query = query.Where(Q => Q.KodawariMenuName.Contains(filter.KodawariMenuName));
      }

      //sort
      switch (filter.SortBy)
      {
        case "KodawariMenuNameAsc":
          query = query.OrderBy(Q => Q.KodawariMenuName);
          break;
        case "KodawariMenuSequenceAsc":
          query = query.OrderBy(Q => Q.KodawariMenuSequence);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "KodawariMenuNameDesc":
          query = query.OrderByDescending(Q => Q.KodawariMenuName);
          break;
        case "KodawariMenuSequenceDesc":
          query = query.OrderByDescending(Q => Q.KodawariMenuSequence);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.KodawariMenuSequence);
          break;
      }
      var kodawariMenus = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new KodawariMenuPaginate
      {
        KodawariMenus = kodawariMenus,
        TotalKodawariMenus = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertKodawariMenuAsync(KodawariMenuCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await ValidateKodawariMenuByNameAsync(model.KodawariMenuName);

            if (isExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var newKodawariMenus = new KodawariMenus
            {
                KodawariMenuName = model.KodawariMenuName,
                KodawariMenuSequence = model.KodawariMenuSequence,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.KodawariMenus.Add(newKodawariMenus);

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateKodawariMenuByNameAsync(string kodawariMenuName)
    {
      var isExist = await this
          .DB
          .KodawariMenus
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.KodawariMenuName == kodawariMenuName);

      return isExist;
    }
    public async Task<KodawariMenuModel> GetKodawariMenuById(Guid kodawariMenuId)
    {
      var data = await (from mo in DB.KodawariMenus
                        where mo.KodawariMenuId == kodawariMenuId && mo.IsDeleted == false
                        select new KodawariMenuModel
                        {
                          KodawariMenuId = mo.KodawariMenuId,
                          KodawariMenuName = mo.KodawariMenuName,
                          KodawariMenuSequence = mo.KodawariMenuSequence,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedAt = mo.UpdatedAt,
                          UpdatedBy = mo.UpdatedBy
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateKodawariMenuAsync(KodawariMenuUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateKodawariMenuByNameAsync(model.KodawariMenuName, model.KodawariMenuId);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var kodawariMenuToUpdate = await this.DB.KodawariMenus.Where(Q => Q.KodawariMenuId == model.KodawariMenuId).FirstOrDefaultAsync();

            kodawariMenuToUpdate.KodawariMenuId = model.KodawariMenuId;
            kodawariMenuToUpdate.KodawariMenuName = model.KodawariMenuName;
            kodawariMenuToUpdate.KodawariMenuSequence = model.KodawariMenuSequence;
            kodawariMenuToUpdate.UpdatedAt = thisDate;
            kodawariMenuToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch(Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateUpdateKodawariMenuByNameAsync(string kodawariMenuName, Guid kodawariMenuId)
    {
      var data = await this
          .DB
          .KodawariMenus
          .AsNoTracking()
          .Where(Q => Q.KodawariMenuId == kodawariMenuId)
          .FirstOrDefaultAsync();

      var isChange = data.KodawariMenuName != kodawariMenuName;

      var isExist = await this.ValidateKodawariMenuByNameAsync(kodawariMenuName);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteKodawariMenuAsync(DeleteKodawariMenuModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.KodawariMenus.Where(Q => Q.KodawariMenuId == model.KodawariMenuId).FirstOrDefaultAsync();

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
