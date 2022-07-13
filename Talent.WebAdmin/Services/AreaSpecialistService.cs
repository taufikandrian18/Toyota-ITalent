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
  public class AreaSpecialistService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly ClaimHelper HelperMan;

    public AreaSpecialistService(TalentContext db, IContextualService contextual, ClaimHelper hm)
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
    public async Task<AreaSpecialistPaginate> GetAllAreaSpecialist(AreaSpecialistGridFilter filter)
    {

      var query = (from sl in this.DB.AreaSpecialists
                   where sl.IsDeleted == false
                   select new AreaSpecialistModel
                   {
                     AreaSpecialistId = sl.AreaSpecialistId,
                     AreaSpecialistName = sl.AreaSpecialistName,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt,
                     Outlets = DB.Outlets.Include(x => x.AreaSpecialist).Where(x => x.AreaSpecialist.AreaSpecialistId == sl.AreaSpecialistId).ToList()
                   }).AsQueryable();

      if (!String.IsNullOrWhiteSpace(filter.AreaSpecialistName))
      {
        query = query.Where(Q => Q.AreaSpecialistName.Contains(filter.AreaSpecialistName)).AsNoTracking();
      }

      //sort
      //   switch (filter.SortBy)
      //   {
      //     case "AreaSpecialistNameAsc":
      //       query = query.OrderBy(Q => Q.AreaSpecialistName);
      //       break;
      //     case "CreatedAtAsc":
      //       query = query.OrderBy(Q => Q.CreatedAt);
      //       break;
      //     case "UpdatedAtAsc":
      //       query = query.OrderBy(Q => Q.UpdatedAt);
      //       break;
      //     case "AreaSpecialistNameDesc":
      //       query = query.OrderByDescending(Q => Q.AreaSpecialistName);
      //       break;
      //     case "CreatedAtDesc":
      //       query = query.OrderByDescending(Q => Q.CreatedAt);
      //       break;
      //     case "UpdatedAtDesc":
      //       query = query.OrderByDescending(Q => Q.UpdatedAt);
      //       break;
      //     default:
      //       query = query.OrderBy(Q => Q.AreaSpecialistName);
      //       break;
      //   }      
      var totalData = query.Count();
      var areaSpecialists = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();



      return new AreaSpecialistPaginate
      {
        AreaSpecialists = areaSpecialists,
        TotalAreaSpecialists = totalData
      };
    }

    public async Task<ActionResult<BaseResponse>> InsertAreaSpecialistAsync(AreaSpecialistCreateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isExist = await ValidateAreaSpecialistByNameAsync(model.AreaSpecialistName.Trim());

        if (isExist)
        {
          return BadRequest(BaseResponse.BadRequest(model));
        }

        Guid guid = Guid.NewGuid();
        byte[] bytes = guid.ToByteArray();
        string encoded = Convert.ToBase64String(bytes);

        var newAreaSpecialists = new AreaSpecialists
        {
          AreaSpecialistId = encoded,
          AreaSpecialistName = model.AreaSpecialistName.Trim(),
          CreatedAt = thisDate,
          UpdatedAt = thisDate,
          IsDeleted = false,
          CreatedBy = this.HelperMan.GetLoginUserId(),
          UpdatedBy = this.HelperMan.GetLoginUserId()
        };

        this.DB.AreaSpecialists.Add(newAreaSpecialists);

        foreach (var item in model.OutletId)
        {
          var outletToUpdate = await this.DB.Outlets.Where(Q => Q.OutletId == item).FirstOrDefaultAsync();
          outletToUpdate.AreaSpecialistId = newAreaSpecialists.AreaSpecialistId;
          await this.DB.SaveChangesAsync();
        }

        await this.DB.SaveChangesAsync();
        return BaseResponse.ResponseOk();
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }
    public async Task<bool> ValidateAreaSpecialistByNameAsync(string areaSpecialistName)
    {
      var isExist = await this
          .DB
          .AreaSpecialists
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.AreaSpecialistName == areaSpecialistName);

      return isExist;
    }
    public async Task<AreaSpecialistModel> GetAreaSpecialistById(string areaSpecialistId)
    {
      var data = await (from mo in DB.AreaSpecialists
                        where mo.AreaSpecialistId == areaSpecialistId.Trim() && mo.IsDeleted == false
                        select new AreaSpecialistModel
                        {
                          AreaSpecialistId = mo.AreaSpecialistId,
                          AreaSpecialistName = mo.AreaSpecialistName,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedAt = mo.UpdatedAt,
                          UpdatedBy = mo.UpdatedBy,
                          Outlets = DB.Outlets.Include(x => x.AreaSpecialist).Where(x => x.AreaSpecialist.AreaSpecialistId == mo.AreaSpecialistId).ToList()
                        }).FirstOrDefaultAsync();

      return data;
    }

    public async Task<ActionResult<BaseResponse>> UpdateAreaSpecialistAsync(AreaSpecialistUpdateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isChangedAndExist = await ValidateUpdateAreaSpecialistNameAsync(model.AreaSpecialistName, model.AreaSpecialistId);

        if (isChangedAndExist)
        {
          return BadRequest(BaseResponse.BadRequest(model));
        }

        var areaSpecialistToUpdate = await this.DB.AreaSpecialists.Where(Q => Q.AreaSpecialistId == model.AreaSpecialistId).FirstOrDefaultAsync();

        areaSpecialistToUpdate.AreaSpecialistId = model.AreaSpecialistId;
        areaSpecialistToUpdate.AreaSpecialistName = model.AreaSpecialistName;
        areaSpecialistToUpdate.UpdatedAt = thisDate;
        areaSpecialistToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

        foreach (var item in model.OutletId)
        {
          var outletToUpdate = await this.DB.Outlets.Where(Q => Q.OutletId == item).FirstOrDefaultAsync();
          outletToUpdate.AreaSpecialistId = model.AreaSpecialistId;
          await this.DB.SaveChangesAsync();
        }

        await this.DB.SaveChangesAsync();
        return BaseResponse.ResponseOk();
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }

    public async Task<bool> ValidateUpdateAreaSpecialistNameAsync(string areaSpecialistName, string areaSpecialistId)
    {
      var data = await this
          .DB
          .AreaSpecialists
          .AsNoTracking()
          .Where(Q => Q.AreaSpecialistId == areaSpecialistId)
          .FirstOrDefaultAsync();

      var isChange = data.AreaSpecialistName != areaSpecialistName;

      var isExist = await this.ValidateAreaSpecialistByNameAsync(areaSpecialistName);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }

    public async Task<ActionResult<BaseResponse>> DeleteAreaSpecialistAsync(DeleteAreaSpecialistModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.AreaSpecialists.Where(Q => Q.AreaSpecialistId == model.AreaSpecialistId).FirstOrDefaultAsync();

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
