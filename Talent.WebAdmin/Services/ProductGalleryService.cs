using System;
using System.Threading.Tasks;
using System.Linq;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Talent.WebAdmin.Services
{
  public class ProductGalleryService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly FileService FileServiceMan;
    private readonly ApprovalService ApprovalMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;
    private readonly PushNotificationService PNService;

    public ProductGalleryService(TalentContext db, IContextualService contextual, FileService fs, ApprovalService approvalService, IFileStorageService fm, ClaimHelper hm, BlobService bm, PushNotificationService pn)
    {
      this.DB = db;
      this.ContextMan = contextual;
      this.FileServiceMan = fs;
      this.ApprovalMan = approvalService;
      this.FileMan = fm;
      this.HelperMan = hm;
      this.BlobService = bm;
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
    public async Task<ProductGalleryPaginate> GetAllProductGalleryContributor(ProductGalleryGridFilter filter)
    {

      var empId = this.HelperMan.GetLoginUserId();

      var rolesStr = new List<string>();
      rolesStr.Add("Super Admin");
      rolesStr.Add("Officer");
      rolesStr.Add("Section Head");
      rolesStr.Add("Division Head");
      rolesStr.Add("Department Head");
      rolesStr.Add("Exec GM");
      rolesStr.Add("Dealer HO");

      var query = (from sl in this.DB.ProductGalleries
                   join emp in this.DB.EmployeePositionMappings on sl.CreatedBy equals emp.EmployeeId
                   join em in this.DB.Employees on emp.EmployeeId equals em.EmployeeId
                   join ot in this.DB.Outlets on em.OutletId equals ot.OutletId into grpjoin_sl_ot
                   from ot in grpjoin_sl_ot.DefaultIfEmpty()
                   join pos in this.DB.Positions on emp.PositionId equals pos.PositionId
                   join sli in this.DB.ProductTypes on sl.ProductTypeId equals sli.ProductTypeId
                   where sl.IsDeleted == false && sl.IsApproved == "Draft"
                   //where sl.IsDeleted == false && (rol.Name.Trim() != "Super Admin" || rol.Name.Trim() != "Officer" || rol.Name.Trim() != "Section Head" || rol.Name.Trim() != "Division Head" || rol.Name.Trim() != "Department Head" || rol.Name.Trim() != "Exec GM" || rol.Name.Trim() != "Dealer HO")
                   select new ProductGalleryModel
                   {
                     ProductGalleryId = sl.ProductGalleryId,
                     ProductId = sl.ProductId,
                     BlobId = sl.BlobId,
                     ContributorName = sl.CreatedBy,
                     OutletName = ot.Name,
                     PositionName = pos.PositionName,
                     ProductTypeId = sl.ProductTypeId,
                     ProductTypeName = sli.ProductTypeName,
                     ProductGalleryColorCode = sl.ProductGalleryColorCode,
                     ProductGalleryColorName = sl.ProductGalleryColorName,
                     IsColor = sl.IsColor,
                     IsApproved = sl.IsApproved,
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

      if (!string.IsNullOrEmpty(filter.ProductGalleryColorName))
      {
        query = query.Where(Q => Q.ProductGalleryColorName == filter.ProductGalleryColorName);
      }

      if (filter.ProductId != Guid.Empty)
      {
        query = query.Where(Q => Q.ProductId == filter.ProductId);
      }

      //sort
      switch (filter.SortBy)
      {
        case "ProductCategoryNameAsc":
          query = query.OrderBy(Q => Q.ProductGalleryColorName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "ProductCategoryNameDesc":
          query = query.OrderByDescending(Q => Q.ProductGalleryColorName);
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
      var productGalleries = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in productGalleries)
      {

        var imageURL = "";
        var blobData = await this.BlobService.GetBlobById(datum.BlobId);

        imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

        productGalleries[index].ImageUrl = imageURL;
        index++;
      }

      return new ProductGalleryPaginate
      {
        ProductGalleries = productGalleries,
        TotalProductGalleries = totalData
      };
    }
    public async Task<ProductGalleryPaginate> GetAllProductGallery(ProductGalleryGridFilter filter)
    {

      var empId = this.HelperMan.GetLoginUserId();

      var query = (from sl in this.DB.ProductGalleries
                   join emp in this.DB.EmployeePositionMappings on sl.CreatedBy equals emp.EmployeeId
                   join em in this.DB.Employees on emp.EmployeeId equals em.EmployeeId
                   join ot in this.DB.Outlets on em.OutletId equals ot.OutletId into grpjoin_sl_ot
                   from ot in grpjoin_sl_ot.DefaultIfEmpty()
                   join pos in this.DB.Positions on emp.PositionId equals pos.PositionId
                   join sli in this.DB.ProductTypes on sl.ProductTypeId equals sli.ProductTypeId
                   //where sl.IsDeleted == false && (rol.Name.Trim() == "Super Admin" || rol.Name.Trim() == "Officer" || rol.Name.Trim() == "Section Head" || rol.Name.Trim() == "Division Head" || rol.Name.Trim() == "Department Head" || rol.Name.Trim() == "Exec GM" || rol.Name.Trim() == "Dealer HO")
                   where sl.IsDeleted == false && sl.IsApproved == "Published"
                   select new ProductGalleryModel
                   {
                     ProductGalleryId = sl.ProductGalleryId,
                     ProductId = sl.ProductId,
                     BlobId = sl.BlobId,
                     ContributorName = sl.CreatedBy,
                     OutletName = ot.Name,
                     PositionName = pos.PositionName,
                     ProductTypeId = sl.ProductTypeId,
                     ProductTypeName = sli.ProductTypeName,
                     ProductGalleryColorCode = sl.ProductGalleryColorCode,
                     ProductGalleryColorName = sl.ProductGalleryColorName,
                     IsColor = sl.IsColor,
                     IsApproved = sl.IsApproved,
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

      if (filter.ProductGalleryColorName != null)
      {
        query = query.Where(Q => Q.ProductGalleryColorName == filter.ProductGalleryColorName);
      }

      if (filter.ProductId != Guid.Empty)
      {
        query = query.Where(Q => Q.ProductId == filter.ProductId);
      }

      //sort
      switch (filter.SortBy)
      {
        case "ProductCategoryNameAsc":
          query = query.OrderBy(Q => Q.ProductGalleryColorName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "ProductCategoryNameDesc":
          query = query.OrderByDescending(Q => Q.ProductGalleryColorName);
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
      var productGalleries = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in productGalleries)
      {

        var imageURL = "";
        var blobData = await this.BlobService.GetBlobById(datum.BlobId);

        imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

        productGalleries[index].ImageUrl = imageURL;
        index++;
      }

      return new ProductGalleryPaginate
      {
        ProductGalleries = productGalleries,
        TotalProductGalleries = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertProductGalleryAsync(ProductGalleryCreateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        //var isExist = await ValidateProductGalleryByColorCodeAsync(model.ProductGalleryColorCode,model.ProductId);

        //if (isExist)
        //{
        //    return false;
        //}

        var getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductGalleryFileContent);

        if (model.ProductGalleryFileContent != null)
        {
          if (string.IsNullOrEmpty(model.ProductGalleryFileContent.Base64) == false)
          {
            getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductGalleryFileContent);
          }
        }

        var newProductGalleries = new ProductGalleries
        {
          ProductId = model.ProductId,
          ProductTypeId = model.ProductTypeId,
          BlobId = getUploadModuleBlob,
          ProductGalleryColorCode = model.ProductGalleryColorCode,
          ProductGalleryColorName = model.ProductGalleryColorName,
          IsColor = model.IsColor,
          IsApproved = "Published",
          ApprovedAt = model.ApprovedAt,
          CreatedAt = thisDate,
          UpdatedAt = thisDate,
          IsDeleted = false,
          CreatedBy = this.HelperMan.GetLoginUserId(),
          UpdatedBy = this.HelperMan.GetLoginUserId()
        };

        this.DB.ProductGalleries.Add(newProductGalleries);

        await this.DB.SaveChangesAsync();
        if (model.ApprovedAt != null)
        {
          var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
          var groupPositionList = new List<string>();
          var manPowerPositionList = new List<string>();
          var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
          var PushNotificationMyTools = new VMPushNotificationAdd();
          PushNotificationMyTools.Title = "New Product Gallery";
          PushNotificationMyTools.Body = "A new product Gallery Color " + model.ProductGalleryColorName + " has been added.";
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
    public async Task<bool> ValidateProductGalleryByColorCodeAsync(string ProductGalleryColorCode, Guid ProductId)
    {
      var isExist = await this
          .DB
          .ProductGalleries
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false && Q.ProductId == ProductId)
          .AnyAsync(Q => Q.ProductGalleryColorCode == ProductGalleryColorCode);

      return isExist;
    }
    public async Task<ProductGalleryModel> GetProductGalleryById(Guid productGalleryId)
    {
      var data = await (from mo in DB.ProductGalleries
                        join emp in this.DB.EmployeePositionMappings on mo.CreatedBy equals emp.EmployeeId
                        join em in this.DB.Employees on emp.EmployeeId equals em.EmployeeId
                        join ot in this.DB.Outlets on em.OutletId equals ot.OutletId
                        join pos in this.DB.Positions on emp.PositionId equals pos.PositionId
                        join rol in this.DB.Roles on pos.PositionId equals rol.PositionId
                        where mo.ProductGalleryId == productGalleryId && mo.IsDeleted == false
                        select new ProductGalleryModel
                        {
                          ProductGalleryId = mo.ProductGalleryId,
                          ProductId = mo.ProductId,
                          BlobId = mo.BlobId,
                          ContributorName = mo.CreatedBy,
                          OutletName = ot.Name,
                          PositionName = pos.PositionName,
                          ProductTypeId = mo.ProductTypeId,
                          ProductGalleryColorCode = mo.ProductGalleryColorCode,
                          ProductGalleryColorName = mo.ProductGalleryColorName,
                          IsColor = mo.IsColor,
                          IsApproved = mo.IsApproved,
                          ApprovedAt = mo.ApprovedAt,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedBy = mo.UpdatedBy,
                          UpdatedAt = mo.UpdatedAt
                        }).FirstOrDefaultAsync();

      var imageURL = "";
      var blobData = await this.BlobService.GetBlobById(data.BlobId);

      imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

      data.ImageUrl = imageURL;

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductGalleryAsync(ProductGalleryUpdateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        //var isChangedAndExist = await ValidateUpdateProductGalleryCodeAsync(model.ProductId, model.ProductGalleryId,model.ProductGalleryColorCode);

        //if (isChangedAndExist)
        //{
        //    return false;
        //}

        var productGalleryToUpdate = await this.DB.ProductGalleries.Where(Q => Q.ProductGalleryId == model.ProductGalleryId).FirstOrDefaultAsync();

        if (model.ProductGalleryFileContent != null)
        {
          if (!String.IsNullOrEmpty(model.ProductGalleryFileContent.Base64))
          {
            productGalleryToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.ProductGalleryFileContent);
          }
        }

        productGalleryToUpdate.ProductTypeId = model.ProductTypeId;
        productGalleryToUpdate.ProductGalleryColorCode = model.ProductGalleryColorCode;
        productGalleryToUpdate.ProductGalleryColorName = model.ProductGalleryColorName;
        productGalleryToUpdate.IsColor = model.IsColor;
        productGalleryToUpdate.ApprovedAt = model.ApprovedAt;
        productGalleryToUpdate.UpdatedAt = thisDate;
        productGalleryToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

        await this.DB.SaveChangesAsync();
        if (model.ApprovedAt != null)
        {
          var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
          var groupPositionList = new List<string>();
          var manPowerPositionList = new List<string>();
          var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
          var PushNotificationMyTools = new VMPushNotificationAdd();
          PushNotificationMyTools.Title = "Product Gallery Update";
          PushNotificationMyTools.Body = "Product Gallery Color " + model.ProductGalleryColorName + " has been updated.";
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
    public async Task<bool> ValidateUpdateProductGalleryCodeAsync(Guid ProductId, Guid productGalleryId, string ProductGalleryColorCode)
    {
      var data = await this
          .DB
          .ProductGalleries
          .AsNoTracking()
          .Where(Q => Q.ProductGalleryId == productGalleryId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductId != ProductId;

      var isExist = await this.ValidateProductGalleryByColorCodeAsync(ProductGalleryColorCode, ProductId);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductGalleryAsync(DeleteProductGalleryModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.ProductGalleries.Where(Q => Q.ProductGalleryId == model.ProductGalleryId).FirstOrDefaultAsync();

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
    public async Task<ActionResult<BaseResponse>> UpdateProductGalleryByApprovalAsync(UpdateProductGalleryApproval model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.ProductGalleries.Where(Q => Q.ProductGalleryId == model.ProductGalleryId).FirstOrDefaultAsync();

        module.IsApproved = model.IsApproved;
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
