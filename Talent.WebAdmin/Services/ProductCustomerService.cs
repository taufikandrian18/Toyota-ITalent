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
  public class ProductCustomerService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly FileService FileServiceMan;
    private readonly ApprovalService ApprovalMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly PushNotificationService PNService;

    public ProductCustomerService(TalentContext db, IContextualService contextual, FileService fs, ApprovalService approvalService, IFileStorageService fm, ClaimHelper hm,PushNotificationService pn)
    {
      this.DB = db;
      this.ContextMan = contextual;
      this.FileServiceMan = fs;
      this.ApprovalMan = approvalService;
      this.FileMan = fm;
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

    public async Task<ProductCustomerPaginate> GetAllProductCustomer(ProductCustomerGridFilter filter)
    {
      var query = (from sl in this.DB.ProductCustomers
                   join sli in this.DB.ProductCustomerTypes on sl.ProductCustomerTypeId equals sli.ProductCustomerTypeId
                   where sl.IsDeleted == false
                   select new ProductCustomerModel
                   {
                     ProductCustomerTypeId = sl.ProductCustomerTypeId,
                     ProductCustomerTypeName = sli.ProductCustomerTypeName,
                     ProductId = sl.ProductId,
                     ProductCustomerId = sl.ProductCustomerId,
                     ProductCustomerPenghasilan = sl.ProductCustomerPenghasilan,
                     ProductCustomerStatus = sl.ProductCustomerStatus,
                     ProductCustomerKebutuhan = sl.ProductCustomerKebutuhan,
                     ProductCustomerPekerjaan = sl.ProductCustomerPekerjaan,
                     ProductCustomerUmur = sl.ProductCustomerUmur,
                     ProductCustomerPrevVehicle = sl.ProductCustomerPrevVehicle,
                     ProductCustomerProspectSource = sl.ProductCustomerProspectSource,
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
      var productCustomers = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new ProductCustomerPaginate
      {
        ProductCustomers = productCustomers,
        TotalProductCustomers = totalData
      };
    }

    public async Task<ActionResult<BaseResponse>> InsertProductCustomerAsync(ProductCustomerCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            // var isExist = await ValidateProductByProductCustomerTypeAsync(model.ProductCustomerTypeId);

            // if (isExist)
            // {
            //   return false;
            // }

            var newProductCustomers = new ProductCustomers
            {
                ProductCustomerTypeId = model.ProductCustomerTypeId,
                ProductId = model.ProductId,
                ProductCustomerStatus = model.ProductCustomerStatus,
                ProductCustomerKebutuhan = model.ProductCustomerKebutuhan,
                ProductCustomerUmur = model.ProductCustomerUmur,
                ProductCustomerPekerjaan = model.ProductCustomerPekerjaan,
                ProductCustomerPenghasilan = model.ProductCustomerPenghasilan,
                ProductCustomerPrevVehicle = model.ProductCustomerPrevVehicle,
                ProductCustomerProspectSource = model.ProductCustomerProspectSource,
                ApprovedAt = model.ApprovedAt,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.ProductCustomers.Add(newProductCustomers);

            await this.DB.SaveChangesAsync();
            if(model.ApprovedAt != null)
            {
                var getProductCustName = await this.DB.ProductCustomerTypes.Where(Q => Q.ProductCustomerTypeId == model.ProductCustomerTypeId).Select(Q => Q.ProductCustomerTypeName).FirstOrDefaultAsync();
                var ProductTypeId = await this.DB.ProductTypes.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductTypeId).FirstOrDefaultAsync();
                var groupPositionList = new List<string>();
                var manPowerPositionList = new List<string>();
                var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                var PushNotificationMyTools = new VMPushNotificationAdd();
                PushNotificationMyTools.Title = "New Product Customer";
                PushNotificationMyTools.Body = "A new product Customer Type "+ getProductCustName + " has been added.";
                PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
                PushNotificationMyTools.IsPublished = true;
                PushNotificationMyTools.GroupPositions = groupPositionList;
                PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

                PushNotificationDataMyTools.Category = "ProductProp";
                PushNotificationDataMyTools.DataID = model.ProductId;
                PushNotificationDataMyTools.DataSecondId = ProductTypeId;

                await this.PNService.CreatePushNotificationMyTools(PushNotificationMyTools, PushNotificationDataMyTools);
            }
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }

    public async Task<bool> ValidateProductByProductCustomerTypeAsync(Guid productCustomerTypeId)
    {
      var isExist = await this
          .DB
          .ProductCustomers
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ProductCustomerTypeId == productCustomerTypeId);

      return isExist;
    }

    public async Task<ProductCustomerModel> GetProductCustomerById(Guid productCustomerId)
    {
      var data = await (from mo in DB.ProductCustomers
                        where mo.ProductCustomerId == productCustomerId && mo.IsDeleted == false
                        select new ProductCustomerModel
                        {
                          ProductCustomerId = mo.ProductCustomerId,
                          ProductId = mo.ProductId,
                          ProductCustomerTypeId = mo.ProductCustomerTypeId,
                          ProductCustomerStatus = mo.ProductCustomerStatus,
                          ProductCustomerKebutuhan = mo.ProductCustomerKebutuhan,
                          ProductCustomerPenghasilan = mo.ProductCustomerPenghasilan,
                          ProductCustomerUmur = mo.ProductCustomerUmur,
                          ProductCustomerPekerjaan = mo.ProductCustomerPekerjaan,
                          ProductCustomerPrevVehicle = mo.ProductCustomerPrevVehicle,
                          ProductCustomerProspectSource = mo.ProductCustomerProspectSource,
                          ApprovedAt = mo.ApprovedAt,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedBy = mo.UpdatedBy,
                          UpdatedAt = mo.UpdatedAt
                        }).FirstOrDefaultAsync();

      return data;
    }

    public async Task<ActionResult<BaseResponse>> UpdateProductCustomerAsync(ProductCustomerUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            // var isChangedAndExist = await ValidateUpdateProductCustomerAsync(model.ProductCustomerTypeId, model.ProductCustomerId);

            // if (isChangedAndExist)
            // {
            //   return false;
            // }

            var productCustomerToUpdate = await this.DB.ProductCustomers.Where(Q => Q.ProductCustomerId == model.ProductCustomerId).FirstOrDefaultAsync();

            productCustomerToUpdate.ProductCustomerId = model.ProductCustomerId;
            productCustomerToUpdate.ProductId = model.ProductId;
            productCustomerToUpdate.ProductCustomerTypeId = model.ProductCustomerTypeId;
            productCustomerToUpdate.ProductCustomerStatus = model.ProductCustomerStatus;
            productCustomerToUpdate.ProductCustomerKebutuhan = model.ProductCustomerKebutuhan;
            productCustomerToUpdate.ProductCustomerPenghasilan = model.ProductCustomerPenghasilan;
            productCustomerToUpdate.ProductCustomerUmur = model.ProductCustomerUmur;
            productCustomerToUpdate.ProductCustomerPekerjaan = model.ProductCustomerPekerjaan;
            productCustomerToUpdate.ProductCustomerPrevVehicle = model.ProductCustomerPrevVehicle;
            productCustomerToUpdate.ProductCustomerProspectSource = model.ProductCustomerProspectSource;
            productCustomerToUpdate.ApprovedAt = model.ApprovedAt;
            productCustomerToUpdate.UpdatedAt = thisDate;
            productCustomerToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            if(model.ApprovedAt != null)
            {
                var getProductCustName = await this.DB.ProductCustomerTypes.Where(Q => Q.ProductCustomerTypeId == model.ProductCustomerTypeId).Select(Q => Q.ProductCustomerTypeName).FirstOrDefaultAsync();
                var ProductTypeId = await this.DB.ProductTypes.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductTypeId).FirstOrDefaultAsync();
                var groupPositionList = new List<string>();
                var manPowerPositionList = new List<string>();
                var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                var PushNotificationMyTools = new VMPushNotificationAdd();
                PushNotificationMyTools.Title = "Product Customer Update";
                PushNotificationMyTools.Body = "A new product Customer Type "+ getProductCustName + " has been updated.";
                PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
                PushNotificationMyTools.IsPublished = true;
                PushNotificationMyTools.GroupPositions = groupPositionList;
                PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

                PushNotificationDataMyTools.Category = "ProductProp";
                PushNotificationDataMyTools.DataID = model.ProductId;
                PushNotificationDataMyTools.DataSecondId = ProductTypeId;

                await this.PNService.CreatePushNotificationMyTools(PushNotificationMyTools, PushNotificationDataMyTools);
            }
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }

    public async Task<bool> ValidateUpdateProductCustomerAsync(Guid productCustomerTypeId, Guid productCustomerId)
    {
      var data = await this
          .DB
          .ProductCustomers
          .AsNoTracking()
          .Where(Q => Q.ProductCustomerId == productCustomerId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductCustomerTypeId != productCustomerTypeId;

      var isExist = await this.ValidateProductByProductCustomerTypeAsync(productCustomerTypeId);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }

    public async Task<ActionResult<BaseResponse>> DeleteProductCustomerAsync(DeleteProductCustomerModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.ProductCustomers.Where(Q => Q.ProductCustomerId == model.ProductCustomerId).FirstOrDefaultAsync();

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
