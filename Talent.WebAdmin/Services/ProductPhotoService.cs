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
  public class ProductPhotoService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly FileService FileServiceMan;
    private readonly ApprovalService ApprovalMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;
    private readonly PushNotificationService PNService;

    public ProductPhotoService(TalentContext db, IContextualService contextual, FileService fs, ApprovalService approvalService, IFileStorageService fm, ClaimHelper hm, BlobService bm,PushNotificationService pn)
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

    public async Task<ProductPhotoPaginate> GetAllProductPhoto(ProductPhotoGridFilter filter)
    {
      if (filter.ProductId != Guid.Empty)
      {
        var query = (from sl in this.DB.ProductPhotos
                     join sli in this.DB.ProductPhoto360Mappings on sl.ProductPhotoId equals sli.ProductPhotoId
                     join sla in this.DB.Products on sli.ProductId equals sla.ProductId
                     join blb in this.DB.Blobs on sl.BlobId equals blb.BlobId
                     where sl.IsDeleted == false && sli.ProductId == filter.ProductId
                     select new ProductPhotoModel
                     {
                       ProductPhotoId = sl.ProductPhotoId,
                       BlobId = sl.BlobId,
                       BlobName = blb.Name,
                       Is360Photos = sl.Is360Photo,
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

        if (!string.IsNullOrEmpty(filter.BlobName))
        {
          query = query.Where(Q => Q.BlobName == filter.BlobName);
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
        var productPhotos = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

        var totalData = await query.CountAsync();

        var index = 0;
        foreach (var datum in productPhotos)
        {

          var imageURL = "";
          var blobData = await this.BlobService.GetBlobById(datum.BlobId);

          imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

          productPhotos[index].ImageUrl = imageURL;
          productPhotos[index].Blob = blobData;
          index++;
        }

        return new ProductPhotoPaginate
        {
          ProductPhotos = productPhotos,
          TotalProductPhotos = totalData
        };
      }
      else
      {
        var query = (from sl in this.DB.ProductPhotos
                     join sli in this.DB.ProductPhoto360Mappings on sl.ProductPhotoId equals sli.ProductPhotoId
                     join sla in this.DB.Products on sli.ProductId equals sla.ProductId
                     join blb in this.DB.Blobs on sl.BlobId equals blb.BlobId
                     where sl.IsDeleted == false
                     select new ProductPhotoModel
                     {
                       ProductPhotoId = sl.ProductPhotoId,
                       BlobId = sl.BlobId,
                       BlobName = blb.Name,
                       Is360Photos = sl.Is360Photo,
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

        if (filter.BlobName != null)
        {
          query = query.Where(Q => Q.BlobName == filter.BlobName);
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
        var productPhotos = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

        var totalData = await query.CountAsync();

        var index = 0;
        foreach (var datum in productPhotos)
        {

          var imageURL = "";
          var blobData = await this.BlobService.GetBlobById(datum.BlobId);

          imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

          productPhotos[index].ImageUrl = imageURL;
          index++;
        }

        return new ProductPhotoPaginate
        {
          ProductPhotos = productPhotos,
          TotalProductPhotos = totalData
        };
      }

    }

    public async Task<ActionResult<BaseResponse>> InsertProductPhoto(ProductPhotoCreateModel model)
    {
      try
      {
        //Validation if coach exist
        var findProduct = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId && Q.IsDeleted == false).FirstOrDefaultAsync();

        if (findProduct == null)
        {
          return BadRequest(BaseResponse.BadRequest(model));
        }

        var userLogin = this.HelperMan.GetLoginUserId();

        var getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);

        if (model.ProductFileContent != null)
        {
          if (string.IsNullOrEmpty(model.ProductFileContent.Base64) == false)
          {
            getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);
          }
        }

        var insertProductPhoto = new ProductPhotos
        {
          BlobId = getUploadModuleBlob,
          Is360Photo = model.Is360Photo,
          ApprovedAt = model.ApprovedAt,
          CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
          UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
          CreatedBy = userLogin,
          UpdatedBy = userLogin,
        };

        this.DB.ProductPhotos.Add(insertProductPhoto);

        var ProductPhotoId = insertProductPhoto.ProductPhotoId;

        var insertProduct = new ProductPhoto360Mappings
        {
          ProductId = model.ProductId,
          ProductPhotoId = ProductPhotoId
        };

        this.DB.ProductPhoto360Mappings.Add(insertProduct);

        await this.DB.SaveChangesAsync();

        if(model.ApprovedAt != null)
        {
            var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
            var ProductTypeId = await this.DB.ProductTypes.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductTypeId).FirstOrDefaultAsync();
            var groupPositionList = new List<string>();
            var manPowerPositionList = new List<string>();
            var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
            var PushNotificationMyTools = new VMPushNotificationAdd();
            PushNotificationMyTools.Title = "New Product Photo from "+ getProductName;
            PushNotificationMyTools.Body = "A new product photo has been added.";
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

    public async Task<ActionResult<BaseResponse>> UpdateProductPhoto(ProductPhotoUpdateModel model)
    {
        try
        {
            var findProductPhoto = await this.DB.ProductPhotos.Where(Q => Q.ProductPhotoId == model.ProductPhotoId).FirstOrDefaultAsync();
            if (findProductPhoto == null)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var userLogin = this.HelperMan.GetLoginUserId();

            if (model.ProductFileContent != null)
            {
                if (!String.IsNullOrEmpty(model.ProductFileContent.Base64))
                {
                    findProductPhoto.BlobId = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);
                }
            }

            findProductPhoto.ApprovedAt = model.ApprovedAt;
            findProductPhoto.Is360Photo = model.Is360Photo;
            findProductPhoto.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            findProductPhoto.UpdatedBy = userLogin;

            var queryGetCurrentPhotoMapping = this.DB.ProductPhoto360Mappings.Where(Q => Q.ProductId == model.ProductId && Q.ProductPhotoId == model.ProductPhotoId).FirstOrDefault();

            this.DB.ProductPhoto360Mappings.Remove(queryGetCurrentPhotoMapping);

            var insert = new ProductPhoto360Mappings
            {
                ProductId = queryGetCurrentPhotoMapping.ProductId,
                ProductPhotoId = findProductPhoto.ProductPhotoId
            };

            this.DB.ProductPhoto360Mappings.Add(insert);

            await this.DB.SaveChangesAsync();
            if(model.ApprovedAt != null)
            {
                var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
                var ProductTypeId = await this.DB.ProductTypes.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductTypeId).FirstOrDefaultAsync();
                var groupPositionList = new List<string>();
                var manPowerPositionList = new List<string>();
                var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                var PushNotificationMyTools = new VMPushNotificationAdd();
                PushNotificationMyTools.Title = "Update Product Photo from "+ getProductName;
                PushNotificationMyTools.Body = "Product photo has been updated.";
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
    public async Task<ProductPhotoModel> GetProductPhotoById(Guid productPhotoId)
    {
      var data = await (from mo in DB.ProductPhotos
                        join blb in this.DB.Blobs on mo.BlobId equals blb.BlobId
                        where mo.ProductPhotoId == productPhotoId && mo.IsDeleted == false
                        select new ProductPhotoModel
                        {
                          ProductPhotoId = mo.ProductPhotoId,
                          BlobId = mo.BlobId,
                          BlobName = blb.Name,
                          Is360Photos = mo.Is360Photo,
                          ApprovedAt = mo.ApprovedAt,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedAt = mo.UpdatedAt,
                          UpdatedBy = mo.UpdatedBy
                        }).FirstOrDefaultAsync();

      var imageURL = "";
      var blobData = await this.BlobService.GetBlobById(data.BlobId);

      imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

      data.ImageUrl = imageURL;

      return data;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductPhoto(DeleteProductPhotoModel model)
    {
        try
        {
            var dataProductPhotoMapping = await this
                        .DB
                        .ProductPhoto360Mappings
                        .Where(Q => Q.ProductId == model.ProductPhotoId)
                        .ToListAsync();

            this.DB.ProductPhoto360Mappings.RemoveRange(dataProductPhotoMapping);


            var dataProductPhoto = await this
            .DB
            .ProductPhotos
            .Where(Q => Q.ProductPhotoId == model.ProductPhotoId)
            .FirstOrDefaultAsync();

            this.DB.ProductPhotos.Remove(dataProductPhoto);
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
