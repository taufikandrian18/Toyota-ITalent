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
    public class ProductTipService : Controller
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;
        private readonly FileService FileServiceMan;
        private readonly ApprovalService ApprovalMan;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper HelperMan;
        private readonly BlobService BlobService;
        private readonly PushNotificationService PNService;

        public ProductTipService(TalentContext db, IContextualService contextual, FileService fs, ApprovalService approvalService, IFileStorageService fm, ClaimHelper hm, BlobService bm, PushNotificationService pn)
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
        public async Task<ProductTipPaginate> GetAllProductTip(ProductTipGridFilter filter)
        {
            var query = (from sl in this.DB.ProductTips
                         join ot in this.DB.Outlets on sl.OutletId equals ot.OutletId into grpjoin_sl_ot
                         from ot in grpjoin_sl_ot.DefaultIfEmpty()
                         join em in this.DB.EmployeePositionMappings on sl.CreatedBy equals em.EmployeeId
                         join pos in this.DB.Positions on em.PositionId equals pos.PositionId
                         join sli in this.DB.ProductTipCategories on sl.ProductTipCategoryId equals sli.ProductTipCategoryId
                         where sl.IsDeleted == false
                         select new ProductTipModel
                         {
                             ProductTipId = sl.ProductTipId,
                             ProductId = sl.ProductId,
                             BlobId = sl.BlobId,
                             ProductTipCategoryId = sl.ProductTipCategoryId,
                             ProductTipCategoryName = sli.ProductTipCategoryName,
                             OutletName = ot.Name,
                             ContributorName = sl.CreatedBy,
                             PositionName = pos.PositionName,
                             IsSequence = sl.IsSequence,
                             IsApproved = sl.IsApproved,
                             ProductTipTitle = sl.ProductTipTitle,
                             ProductTipDescription = sl.ProductTipDescription,
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

            if (!string.IsNullOrEmpty(filter.ProductTipTitle))
            {
                query = query.Where(Q => Q.ProductTipTitle.Contains(filter.ProductTipTitle));
            }

      //sort
      switch (filter.SortBy)
      {
        case "SequenceAtAsc":
          query = query.OrderBy(Q => Q.IsSequence);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "SequenceAtDesc":
          query = query.OrderByDescending(Q => Q.IsSequence);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.IsSequence);
          break;
      }
      var productTips = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in productTips)
      {

        var imageURL = "";
        var blobData = await this.BlobService.GetBlobById(datum.BlobId);

        imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

        productTips[index].ImageUrl = imageURL;
        productTips[index].Blob = blobData;
        index++;
      }

      return new ProductTipPaginate
      {
        ProductTips = productTips,
        TotalProductTips = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertProductTipAsync(ProductTipCreateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isExist = await ValidateProductTipBySeqAsync(model.IsSequence, model.ProductTipTitle, model.ProductId, model.ProductTipCategoryId);

        if (isExist)
        {
          return BadRequest(BaseResponse.BadRequest(model));
        }

        var getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductGalleryFileContent);

        if (model.ProductGalleryFileContent != null)
        {
          if (string.IsNullOrEmpty(model.ProductGalleryFileContent.Base64) == false)
          {
            getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductGalleryFileContent);
          }
        }

        var employeeId = GetUserLogin();

        var outletId = await DB.Employees.Where(x => x.EmployeeId == employeeId).Select(x => x.OutletId).FirstOrDefaultAsync();

        var newProductTips = new ProductTips
        {
          ProductId = model.ProductId,
          ProductTipCategoryId = model.ProductTipCategoryId,
          BlobId = getUploadModuleBlob,
          OutletId = outletId,
          IsSequence = model.IsSequence,
          IsApproved = "Published",
          ProductTipTitle = model.ProductTipTitle,
          ProductTipDescription = model.ProductTipDescription,
          ApprovedAt = model.ApprovedAt,
          CreatedAt = thisDate,
          UpdatedAt = thisDate,
          IsDeleted = false,
          CreatedBy = this.HelperMan.GetLoginUserId(),
          UpdatedBy = this.HelperMan.GetLoginUserId()
        };

        this.DB.ProductTips.Add(newProductTips);

        await this.DB.SaveChangesAsync();
        if (model.ApprovedAt != null)
        {
          var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
          var ProductTypeId = await this.DB.ProductTypes.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductTypeId).FirstOrDefaultAsync();
          var groupPositionList = new List<string>();
          var manPowerPositionList = new List<string>();
          var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
          var PushNotificationMyTools = new VMPushNotificationAdd();
          PushNotificationMyTools.Title = "New Product Tip for " + getProductName;
          PushNotificationMyTools.Body = "A new product Tip " + model.ProductTipTitle + " has been added.";
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
    public async Task<bool> ValidateProductTipBySeqAsync(int IsSequence, string ProductTipTitle, Guid ProductId, Guid ProductTipCategoryId)
    {
      var isExist = await this
          .DB
          .ProductTips
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false && Q.ProductId == ProductId && Q.ProductTipCategoryId == ProductTipCategoryId)
          .AnyAsync(Q => Q.ProductTipTitle == ProductTipTitle);

      return isExist;
    }
    public async Task<ProductTipModel> GetProductTipById(Guid ProductTipId)
    {
      var data = await (from mo in DB.ProductTips
                        join ot in this.DB.Outlets on mo.OutletId equals ot.OutletId
                        join em in this.DB.EmployeePositionMappings on mo.CreatedBy equals em.EmployeeId
                        join pos in this.DB.Positions on em.PositionId equals pos.PositionId
                        where mo.ProductTipId == ProductTipId && mo.IsDeleted == false
                        select new ProductTipModel
                        {
                          ProductTipId = mo.ProductTipId,
                          ProductId = mo.ProductId,
                          BlobId = mo.BlobId,
                          OutletName = ot.Name,
                          ContributorName = mo.CreatedBy,
                          PositionName = pos.PositionName,
                          ProductTipCategoryId = mo.ProductTipCategoryId,
                          IsSequence = mo.IsSequence,
                          IsApproved = mo.IsApproved,
                          ProductTipTitle = mo.ProductTipTitle,
                          ProductTipDescription = mo.ProductTipDescription,
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
    public async Task<ActionResult<BaseResponse>> UpdateProductTipAsync(ProductTipUpdateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isChangedAndExist = await ValidateUpdateProductTipAsync(model.ProductId, model.ProductTipId, model.ProductTipTitle, model.IsSequence);

        if (isChangedAndExist)
        {
          return BadRequest(BaseResponse.BadRequest(model));
        }

        var productTipToUpdate = await this.DB.ProductTips.Where(Q => Q.ProductTipId == model.ProductTipId).FirstOrDefaultAsync();

        if (model.ProductGalleryFileContent != null)
        {
          if (!String.IsNullOrEmpty(model.ProductGalleryFileContent.Base64))
          {
            productTipToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.ProductGalleryFileContent);
          }
        }

        var employeeId = GetUserLogin();

        var outletId = await DB.Employees.Where(x => x.EmployeeId == employeeId).Select(x => x.OutletId).FirstOrDefaultAsync();

        productTipToUpdate.ProductTipCategoryId = model.ProductTipCategoryId;
        productTipToUpdate.ProductId = model.ProductId;
        productTipToUpdate.OutletId = outletId;
        productTipToUpdate.IsSequence = model.IsSequence;
        productTipToUpdate.ProductTipTitle = model.ProductTipTitle;
        productTipToUpdate.ProductTipDescription = model.ProductTipDescription;
        productTipToUpdate.ApprovedAt = model.ApprovedAt;
        productTipToUpdate.UpdatedAt = thisDate;
        productTipToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

        await this.DB.SaveChangesAsync();
        if (model.ApprovedAt != null)
        {
          var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
          var ProductTypeId = await this.DB.ProductTypes.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductTypeId).FirstOrDefaultAsync();
          var groupPositionList = new List<string>();
          var manPowerPositionList = new List<string>();
          var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
          var PushNotificationMyTools = new VMPushNotificationAdd();
          PushNotificationMyTools.Title = "Product Tip Update ";
          PushNotificationMyTools.Body = "A product Tip " + model.ProductTipTitle + " for " + getProductName + " has been updated.";
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
    public async Task<bool> ValidateUpdateProductTipAsync(Guid ProductId, Guid productTipId, string ProductTipTitle, int ProductTipSequence)
    {
      var data = await this
          .DB
          .ProductTips
          .AsNoTracking()
          .Where(Q => Q.ProductTipId == productTipId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductTipTitle != ProductTipTitle;

      var isExist = await this.ValidateProductTipBySeqAsync(ProductTipSequence, ProductTipTitle, ProductId, data.ProductTipCategoryId);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductTipAsync(DeleteProductTipModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.ProductTips.Where(Q => Q.ProductTipId == model.ProductTipId).FirstOrDefaultAsync();

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
    public async Task<ActionResult<BaseResponse>> UpdateProductTipByApprovalAsync(UpdateProductTipApproval model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.ProductTips.Where(Q => Q.ProductTipId == model.ProductTipId).FirstOrDefaultAsync();

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
