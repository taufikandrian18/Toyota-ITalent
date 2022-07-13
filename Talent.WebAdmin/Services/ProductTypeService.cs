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
  public class ProductTypeService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly FileService FileServiceMan;
    private readonly ApprovalService ApprovalMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;
    private readonly PushNotificationService PNService;

    public ProductTypeService(TalentContext db, IContextualService contextual, FileService fs, ApprovalService approvalService, IFileStorageService fm, ClaimHelper hm, BlobService bm,PushNotificationService pn)
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
    public async Task<List<ProductTypeDropdownView>> GetAllProductTypeByProduct(Guid productId)
    {
      var data = await (from mo in DB.ProductTypes
                        where mo.ProductId == productId && mo.IsDeleted == false
                        select new ProductTypeDropdownView
                        {
                          ProductTypeId = mo.ProductTypeId,
                          ProductTypeName = mo.ProductTypeName
                        }).ToListAsync();

      return data;
    }
    public async Task<ProductTypePaginate> GetAllProductType(ProductTypeGridFilter filter)
    {
      var query = (from sl in this.DB.ProductTypes
                   where sl.IsDeleted == false
                   select new ProductTypeModel
                   {
                     ProductTypeId = sl.ProductTypeId,
                     ProductId = sl.ProductId,
                     BlobId = sl.BlobId,
                     ProductTypeName = sl.ProductTypeName,
                     ProductTypeSalesTalk = sl.ProductTypeSalesTalk,
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

      if (!string.IsNullOrEmpty(filter.ProductTypeName))
      {
        query = query.Where(Q => Q.ProductTypeName.Contains(filter.ProductTypeName));
      }

      if (filter.ProductId != Guid.Empty)
      {
        query = query.Where(Q => Q.ProductId == filter.ProductId);
      }

      //sort
      switch (filter.SortBy)
      {
        case "ProductCategoryNameAsc":
          query = query.OrderBy(Q => Q.ProductTypeName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "ProductCategoryNameDesc":
          query = query.OrderByDescending(Q => Q.ProductTypeName);
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
      var productTypes = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in productTypes)
      {

        var imageURL = "";
        var blobData = await this.BlobService.GetBlobById(datum.BlobId);

        imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

        productTypes[index].ImageUrl = imageURL;
        index++;
      }

      return new ProductTypePaginate
      {
        ProductTypes = productTypes,
        TotalProductTypes = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertProductTypeAsync(ProductTypeCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await ValidateProductTypeByTypeNameAsync(model.ProductTypeName,model.ProductId);

            if (isExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductTypeFileContent);

            if (model.ProductTypeFileContent != null)
            {
                if (string.IsNullOrEmpty(model.ProductTypeFileContent.Base64) == false)
                {
                    getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductTypeFileContent);
                }
            }

            var newProductTypes = new ProductTypes
            {
                ProductId = model.ProductId,
                BlobId = getUploadModuleBlob,
                ProductTypeName = model.ProductTypeName,
                ProductTypeSalesTalk = model.ProductTypeSalesTalk,
                ApprovedAt = model.ApprovedAt,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.ProductTypes.Add(newProductTypes);

            await this.DB.SaveChangesAsync();

            if(model.ApprovedAt != null)
            {
                var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
                var ProductTypeId = newProductTypes.ProductTypeId;
                var groupPositionList = new List<string>();
                var manPowerPositionList = new List<string>();
                var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                var PushNotificationMyTools = new VMPushNotificationAdd();
                PushNotificationMyTools.Title = "New Product "+ getProductName;
                PushNotificationMyTools.Body = "A new product has been added.";
                PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
                PushNotificationMyTools.IsPublished = true;
                PushNotificationMyTools.GroupPositions = groupPositionList;
                PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

                PushNotificationDataMyTools.Category = "Product";
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
    public async Task<bool> ValidateProductTypeByTypeNameAsync(string ProductTypeName, Guid productId)
    {
      var isExist = await this
          .DB
          .ProductTypes
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false && Q.ProductId == productId)
          .AnyAsync(Q => Q.ProductTypeName == ProductTypeName);

      return isExist;
    }
    public async Task<ProductTypeModel> GetProductTypeById(Guid productTypeId)
    {
      var data = await (from mo in DB.ProductTypes
                        where mo.ProductTypeId == productTypeId && mo.IsDeleted == false
                        select new ProductTypeModel
                        {
                          ProductTypeId = mo.ProductTypeId,
                          ProductId = mo.ProductId,
                          BlobId = mo.BlobId,
                          ProductTypeName = mo.ProductTypeName,
                          ProductTypeSalesTalk = mo.ProductTypeSalesTalk,
                          ApprovedAt = mo.ApprovedAt,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedBy = mo.UpdatedBy,
                          UpdatedAt = mo.UpdatedAt
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductTypeAsync(ProductTypeUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateProductTypeAsync(model.ProductTypeName, model.ProductTypeId, model.ProductId);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var productTypeToUpdate = await this.DB.ProductTypes.Where(Q => Q.ProductTypeId == model.ProductTypeId).FirstOrDefaultAsync();

            if (model.ProductTypeFileContent != null)
            {
                if (!String.IsNullOrEmpty(model.ProductTypeFileContent.Base64))
                {
                    productTypeToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.ProductTypeFileContent);
                }
            }

            productTypeToUpdate.ProductTypeId = model.ProductTypeId;
            productTypeToUpdate.ProductId = model.ProductId;
            productTypeToUpdate.ProductTypeName = model.ProductTypeName;
            productTypeToUpdate.ProductTypeSalesTalk = model.ProductTypeSalesTalk;
            productTypeToUpdate.ApprovedAt = model.ApprovedAt;
            productTypeToUpdate.UpdatedAt = thisDate;
            productTypeToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            if(model.ApprovedAt != null)
            {
                var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
                var groupPositionList = new List<string>();
                var manPowerPositionList = new List<string>();
                var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                var PushNotificationMyTools = new VMPushNotificationAdd();
                PushNotificationMyTools.Title = "Product "+ getProductName + " Update";
                PushNotificationMyTools.Body = "A new product has been updated.";
                PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
                PushNotificationMyTools.IsPublished = true;
                PushNotificationMyTools.GroupPositions = groupPositionList;
                PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

                PushNotificationDataMyTools.Category = "ProductUpdate";
                PushNotificationDataMyTools.DataID = model.ProductId;
                PushNotificationDataMyTools.DataSecondId = productTypeToUpdate.ProductTypeId;

                await this.PNService.CreatePushNotificationMyTools(PushNotificationMyTools, PushNotificationDataMyTools);
            }
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateUpdateProductTypeAsync(string productTypeName, Guid productTypeId, Guid productId)
    {
      var data = await this
          .DB
          .ProductTypes
          .AsNoTracking()
          .Where(Q => Q.ProductTypeId == productTypeId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductTypeName != productTypeName;

      var isExist = await this.ValidateProductTypeByTypeNameAsync(productTypeName,productId);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductTypeAsync(DeleteProductTypeModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.ProductTypes.Where(Q => Q.ProductTypeId == model.ProductTypeId).FirstOrDefaultAsync();

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
