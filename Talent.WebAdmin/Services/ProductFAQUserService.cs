using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
  public class ProductFAQUserService
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly FileService FileServiceMan;
    private readonly ApprovalService ApprovalMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;

    public ProductFAQUserService(TalentContext db, IContextualService contextual, FileService fs, ApprovalService approvalService, IFileStorageService fm, ClaimHelper hm)
    {
      this.DB = db;
      this.ContextMan = contextual;
      this.FileServiceMan = fs;
      this.ApprovalMan = approvalService;
      this.FileMan = fm;
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
    public async Task<ProductFAQUserPaginate> GetAllProductFAQUser(ProductFAQUserGridFilter filter)
    {
      var query = (from sl in this.DB.ProductFAQUsers
                   join sli in this.DB.ProductFAQCategories on sl.ProductFaqCategoryId equals sli.ProductFaqCategoryId
                   join emp in this.DB.EmployeePositionMappings on sl.CreatedBy equals emp.EmployeeId
                   join em in this.DB.Employees on emp.EmployeeId equals em.EmployeeId
                   join ot in this.DB.Outlets on em.OutletId equals ot.OutletId into grpjoin_sl_ot
                   from ot in grpjoin_sl_ot.DefaultIfEmpty()
                   join pos in this.DB.Positions on emp.PositionId equals pos.PositionId
                   where sl.IsDeleted == false
                   select new ProductFAQUserModel
                   {
                     ProductFAQUserId = sl.ProductFAQUserId,
                     ProductId = sl.ProductId,
                     ProductFaqCategoryId = sl.ProductFaqCategoryId,
                     ProductFaqCategoryName = sli.ProductFaqCategoryName,
                     ProductFAQUserQuestion = sl.ProductFAQUserQuestion,
                     ContributorName = sl.CreatedBy,
                     OutletName = ot.Name,
                     PositionName = pos.PositionName,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (filter.StartDate != null && filter.EndDate != null)
      {
        var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
        var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

        var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
        var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

        query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt < endDate) ||
                                 (Q.UpdatedAt >= startDate && Q.UpdatedAt < endDate));
      }

      if (filter.ProductId != Guid.Empty)
      {
        query = query.Where(Q => Q.ProductId == filter.ProductId);
      }

      //sort
      switch (filter.SortBy)
      {
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
      }
      var productFAQUsers = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new ProductFAQUserPaginate
      {
        ProductFAQUsers = productFAQUsers,
        TotalProductFAQUsers = totalData
      };
    }
  }
}
