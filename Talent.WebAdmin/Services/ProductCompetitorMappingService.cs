using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
  public class ProductCompetitorMappingService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly ClaimHelper HelperMan;
    private readonly PushNotificationService PNService;

    public ProductCompetitorMappingService(TalentContext db, IContextualService contextual, ClaimHelper hm,PushNotificationService pn)
    {
      this.DB = db;
      this.ContextMan = contextual;
      this.HelperMan = hm;
      this.PNService = pn;
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
    public async Task<ProductCompetitorMappingPaginate> GetAllProductCompetitor(ProductCompetitorMappingGridFilter filter)
    {

      var query = (from sl in this.DB.ProductCompetitorMappings
                   join sli in this.DB.Products on sl.ProductId equals sli.ProductId
                   join sla in this.DB.ProductTypes on sl.ProductTypeId equals sla.ProductTypeId
                   where sl.IsDeleted == false
                   select new ProductCompetitorMappingModel
                   {
                     ProductCompetitorMappingId = sl.ProductCompetitorMappingId,
                     ProductId = sl.ProductId,
                     ProductName = sli.ProductName,
                     ProductTypeId = sl.ProductTypeId,
                     ProductTypeName = sla.ProductTypeName,
                     ProductCompetitorId = sl.ProductCompetitorId,
                     ProductCompetitorName = DB.Products.Where(x => x.ProductId == sl.ProductCompetitorId).Select(x => x.ProductName).FirstOrDefault(),
                     ProductCompetitorTypeId = sl.ProductCompetitorTypeId,
                     ProductTypeCompetitorName = DB.ProductTypes.Where(x => x.ProductTypeId == sl.ProductCompetitorTypeId).Select(x => x.ProductTypeName).FirstOrDefault(),
                     ApprovedAt = sl.ApprovedAt,
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

      if (!string.IsNullOrEmpty(filter.ProductCompetitorName))
      {
        query = query.Where(Q => Q.ProductCompetitorName.Contains(filter.ProductCompetitorName));
      }

      if (filter.ProductId != Guid.Empty)
      {
        query = query.Where(Q => Q.ProductId == filter.ProductId);
      }

      //sort
      switch (filter.SortBy)
      {
        case "ProductSegmentNameAsc":
          query = query.OrderBy(Q => Q.ProductCompetitorName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "ProductSegmentNameDesc":
          query = query.OrderByDescending(Q => Q.ProductCompetitorName);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.ProductCompetitorName);
          break;
      }
      var productCompetitorMappings = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new ProductCompetitorMappingPaginate
      {
        ProductCompetitorMappings = productCompetitorMappings,
        TotalProductCompetitorMapping = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertProductCompetitorAsync(ProductCompetitorMappingCreateUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var userLogin = this.HelperMan.GetLoginUserId();

            var isExist = await ValidateProductCompetitorAsync(model);

            if (isExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var insertProductCompetitor = new ProductCompetitorMappings
            {
                ProductId = model.ProductId,
                ProductTypeId = model.ProductTypeId,
                ProductCompetitorId = model.ProductCompetitorId,
                ProductCompetitorTypeId = model.ProductCompetitorTypeId,
                ApprovedAt = model.ApprovedAt,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = userLogin,
                UpdatedBy = userLogin,
            };

            this.DB.ProductCompetitorMappings.Add(insertProductCompetitor);

            await this.DB.SaveChangesAsync();
            if(model.ApprovedAt != null)
            {
                var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
                var getProductCompetitorName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductCompetitorId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
                var groupPositionList = new List<string>();
                var manPowerPositionList = new List<string>();
                var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                var PushNotificationMyTools = new VMPushNotificationAdd();
                PushNotificationMyTools.Title = "New Product Competitor for "+ getProductName;
                PushNotificationMyTools.Body = "A new product competitor "+ getProductCompetitorName + " has been added.";
                PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
                PushNotificationMyTools.IsPublished = true;
                PushNotificationMyTools.GroupPositions = groupPositionList;
                PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

                PushNotificationDataMyTools.Category = "ProductProp";
                PushNotificationDataMyTools.DataID = model.ProductId;
                PushNotificationDataMyTools.DataSecondId = model.ProductTypeId;

                await this.PNService.CreatePushNotificationMyTools(PushNotificationMyTools, PushNotificationDataMyTools);
            }
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateProductCompetitorAsync(ProductCompetitorMappingCreateUpdateModel model)
    {
      var isExist = await this
          .DB
          .ProductCompetitorMappings
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ProductId == model.ProductId && Q.ProductTypeId == model.ProductTypeId && Q.ProductCompetitorId == model.ProductCompetitorId && Q.ProductCompetitorTypeId == model.ProductCompetitorTypeId && Q.ApprovedAt == model.ApprovedAt);

      return isExist;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductCompetitor(ProductCompetitorMappingUpdateModel model)
    {
        try
        {
            var userLogin = this.HelperMan.GetLoginUserId();

            var findProductCompetitor = await this.DB.ProductCompetitorMappings.Where(Q => Q.ProductCompetitorMappingId == model.ProductCompetitorMappingId).FirstOrDefaultAsync();

            if (findProductCompetitor == null)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }
            else
            {
                this.DB.ProductCompetitorMappings.Remove(findProductCompetitor);

                var insertProductCompetitor = new ProductCompetitorMappings
                {
                    ProductId = model.ProductId,
                    ProductTypeId = model.ProductTypeId,
                    ProductCompetitorId = model.ProductCompetitorId,
                    ProductCompetitorTypeId = model.ProductCompetitorTypeId,
                    ApprovedAt = model.ApprovedAt,
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    CreatedBy = userLogin,
                    UpdatedBy = userLogin,
                };

                this.DB.ProductCompetitorMappings.Add(insertProductCompetitor);

                await this.DB.SaveChangesAsync();
                if (model.ApprovedAt != null)
                {
                    var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
                    var getProductCompetitorName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductCompetitorId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
                    var groupPositionList = new List<string>();
                    var manPowerPositionList = new List<string>();
                    var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                    var PushNotificationMyTools = new VMPushNotificationAdd();
                    PushNotificationMyTools.Title = "Product " + getProductName + " Update";
                    PushNotificationMyTools.Body = "A product "+ getProductCompetitorName + " has been updated as a competitor";
                    PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
                    PushNotificationMyTools.IsPublished = true;
                    PushNotificationMyTools.GroupPositions = groupPositionList;
                    PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

                    PushNotificationDataMyTools.Category = "ProductProp";
                    PushNotificationDataMyTools.DataID = model.ProductId;
                    PushNotificationDataMyTools.DataSecondId = model.ProductTypeId;

                    await this.PNService.CreatePushNotificationMyTools(PushNotificationMyTools, PushNotificationDataMyTools);
                }
                return BaseResponse.ResponseOk();
            }
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<ProductCompetitorMappingModel> GetProductCompetitorById(Guid productCompetitorMappingId)
    {
      var data = await (from mo in DB.ProductCompetitorMappings
                        join sli in this.DB.Products on mo.ProductId equals sli.ProductId
                        join sla in this.DB.ProductTypes on mo.ProductTypeId equals sla.ProductTypeId
                        where mo.IsDeleted == false && mo.ProductCompetitorMappingId == productCompetitorMappingId
                        select new ProductCompetitorMappingModel
                        {
                          ProductCompetitorMappingId = mo.ProductCompetitorMappingId,
                          ProductId = mo.ProductId,
                          ProductName = sli.ProductName,
                          ProductTypeId = mo.ProductTypeId,
                          ProductTypeName = sla.ProductTypeName,
                          ProductCompetitorId = mo.ProductCompetitorId,
                          ProductCompetitorName = DB.Products.Where(x => x.ProductId == mo.ProductCompetitorId).Select(x => x.ProductName).FirstOrDefault(),
                          ProductCompetitorTypeId = mo.ProductCompetitorTypeId,
                          ProductTypeCompetitorName = DB.ProductTypes.Where(x => x.ProductTypeId == mo.ProductCompetitorTypeId).Select(x => x.ProductTypeName).FirstOrDefault(),
                          ApprovedAt = mo.ApprovedAt,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedBy = mo.UpdatedBy,
                          UpdatedAt = mo.UpdatedAt
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductCompetitorAsync(DeleteProductCompetitorModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.ProductCompetitorMappings.Where(Q => Q.ProductCompetitorMappingId == model.ProductCompetitorMappingId).FirstOrDefaultAsync();

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
