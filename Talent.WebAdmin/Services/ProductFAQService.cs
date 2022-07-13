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
  public class ProductFAQService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly FileService FileServiceMan;
    private readonly ApprovalService ApprovalMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;
    private readonly PushNotificationService PNService;

    public ProductFAQService(TalentContext db, IContextualService contextual, FileService fs, ApprovalService approvalService, IFileStorageService fm, ClaimHelper hm, BlobService bm,PushNotificationService pn)
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
    public async Task<ProductFAQPaginate> GetAllProductFAQ(ProductFAQGridFilter filter)
    {
      var query = (from sl in this.DB.ProductFAQs
                   join sli in this.DB.ProductFAQCategories on sl.ProductFaqCategoryId equals sli.ProductFaqCategoryId
                   join emp in this.DB.EmployeePositionMappings on sl.CreatedBy equals emp.EmployeeId
                   join em in this.DB.Employees on emp.EmployeeId equals em.EmployeeId
                   join ot in this.DB.Outlets on em.OutletId equals ot.OutletId into grpjoin_sl_ot
                   from ot in grpjoin_sl_ot.DefaultIfEmpty()
                   join pos in this.DB.Positions on emp.PositionId equals pos.PositionId
                   where sl.IsDeleted == false
                   select new ProductFAQModel
                   {
                     ProductFaqId = sl.ProductFaqId,
                     ProductId = sl.ProductId,
                     BlobId = sl.BlobId,
                     ProductFaqCategoryId = sl.ProductFaqCategoryId,
                     ProductFaqCategoryName = sli.ProductFaqCategoryName,
                     ProductFaqSequence = sl.ProductFaqSequence,
                     ProductFaqTitle = sl.ProductFaqTitle,
                     ProductFaqDescription = sl.ProductFaqDescription,
                     ContributorName = sl.CreatedBy,
                     OutletName = ot.Name,
                     PositionName = pos.PositionName,
                     ApprovedAt = sl.ApprovedAt,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();


      if (!string.IsNullOrEmpty(filter.ProductFaqTitle))
      {
        query = query.Where(Q => Q.ProductFaqTitle.Contains(filter.ProductFaqTitle));
      }

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
        case "SequenceAtAsc":
          query = query.OrderBy(Q => Q.ProductFaqSequence);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "SequenceAtDesc":
          query = query.OrderByDescending(Q => Q.ProductFaqSequence);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.ProductFaqSequence);
          break;
      }
      var productFAQs = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in productFAQs)
      {

        var imageURL = "";
        var blobData = await this.BlobService.GetBlobById(datum.BlobId);

        imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

        productFAQs[index].ImageUrl = imageURL;
        productFAQs[index].Blob = blobData;

        index++;
      }

      return new ProductFAQPaginate
      {
        ProductFAQs = productFAQs,
        TotalProductFAQs = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertProductFAQAsync(ProductFAQCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            // var isExist = await ValidateProductFAQBySeqAndTopicCodeAsync(model.ProductFaqSequence, model.ProductFaqTitle, model.ProductId, model.ProductFaqCategoryId);

            // if (isExist)
            // {
            //   return false;
            // }

            var getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductGalleryFileContent);

            if (model.ProductGalleryFileContent != null)
            {
                if (string.IsNullOrEmpty(model.ProductGalleryFileContent.Base64) == false)
                {
                    getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductGalleryFileContent);
                }
            }

            var newProductFAQs = new ProductFAQs
            {
                ProductId = model.ProductId,
                ProductFaqCategoryId = model.ProductFaqCategoryId,
                BlobId = getUploadModuleBlob,
                ProductFaqSequence = model.ProductFaqSequence,
                ProductFaqTitle = model.ProductFaqTitle,
                ProductFaqDescription = model.ProductFaqDescription,
                ApprovedAt = model.ApprovedAt,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.ProductFAQs.Add(newProductFAQs);

            await this.DB.SaveChangesAsync();
            if (model.ApprovedAt != null)
            {
                var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
                var ProductTypeId = await this.DB.ProductTypes.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductTypeId).FirstOrDefaultAsync();
                var ProductFaqId = newProductFAQs.ProductFaqId;
                var getProductFAQTopic = await this.DB.ProductFAQs.Where(Q => Q.ProductFaqId == ProductFaqId).Select(Q => Q.ProductFaqTitle).FirstOrDefaultAsync();
                var groupPositionList = new List<string>();
                var manPowerPositionList = new List<string>();
                var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                var PushNotificationMyTools = new VMPushNotificationAdd();
                PushNotificationMyTools.Title = "New Product FAQ in " + getProductName;
                PushNotificationMyTools.Body = "A new product FAQ " + getProductFAQTopic + " has been added.";
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
    public async Task<bool> ValidateProductFAQBySeqAndTopicCodeAsync(int ProductFAQSequence, string ProductFaqTitle, Guid ProductId, Guid ProductFAQCategoryId)
    {
      var isExist = await this
          .DB
          .ProductFAQs
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false && Q.ProductId == ProductId && Q.ProductFaqCategoryId == ProductFAQCategoryId)
          .AnyAsync(Q => Q.ProductFaqTitle == ProductFaqTitle || Q.ProductFaqSequence == ProductFAQSequence);

      return isExist;
    }
    public async Task<ProductFAQModel> GetProductFAQById(Guid ProductFaqId)
    {
      var data = await (from mo in DB.ProductFAQs
                        join emp in this.DB.EmployeePositionMappings on mo.CreatedBy equals emp.EmployeeId
                        join em in this.DB.Employees on emp.EmployeeId equals em.EmployeeId
                        join ot in this.DB.Outlets on em.OutletId equals ot.OutletId
                        join pos in this.DB.Positions on emp.PositionId equals pos.PositionId
                        where mo.ProductFaqId == ProductFaqId && mo.IsDeleted == false
                        select new ProductFAQModel
                        {
                          ProductFaqId = mo.ProductFaqId,
                          ProductId = mo.ProductId,
                          BlobId = mo.BlobId,
                          ProductFaqCategoryId = mo.ProductFaqCategoryId,
                          ProductFaqTitle = mo.ProductFaqTitle,
                          ProductFaqSequence = mo.ProductFaqSequence,
                          ProductFaqDescription = mo.ProductFaqDescription,
                          ContributorName = mo.CreatedBy,
                          OutletName = ot.Name,
                          PositionName = pos.PositionName,
                          ApprovedAt = mo.ApprovedAt,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedBy = mo.UpdatedBy,
                          UpdatedAt = mo.UpdatedAt
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductFAQAsync(ProductFAQUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            // var isChangedAndExist = await ValidateUpdateProductFAQAsync(model.ProductId, model.ProductFaqId, model.ProductFaqTitle, model.ProductFaqSequence);

            // if (isChangedAndExist)
            // {
            //   return false;
            // }

            var productFaqToUpdate = await this.DB.ProductFAQs.Where(Q => Q.ProductFaqId == model.ProductFaqId).FirstOrDefaultAsync();

            if (model.ProductGalleryFileContent != null)
            {
                if (!String.IsNullOrEmpty(model.ProductGalleryFileContent.Base64))
                {
                    productFaqToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.ProductGalleryFileContent);
                }
            }

            productFaqToUpdate.ProductFaqCategoryId = model.ProductFaqCategoryId;
            productFaqToUpdate.ProductId = model.ProductId;
            productFaqToUpdate.ProductFaqSequence = model.ProductFaqSequence;
            productFaqToUpdate.ProductFaqTitle = model.ProductFaqTitle;
            productFaqToUpdate.ProductFaqDescription = model.ProductFaqDescription;
            productFaqToUpdate.ApprovedAt = model.ApprovedAt;
            productFaqToUpdate.UpdatedAt = thisDate;
            productFaqToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            if (model.ApprovedAt != null)
            {
                var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
                var ProductTypeId = await this.DB.ProductTypes.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductTypeId).FirstOrDefaultAsync();
                var getProductFAQTopic = await this.DB.ProductFAQs.Where(Q => Q.ProductFaqId == model.ProductFaqId).Select(Q => Q.ProductFaqTitle).FirstOrDefaultAsync();
                var groupPositionList = new List<string>();
                var manPowerPositionList = new List<string>();
                var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                var PushNotificationMyTools = new VMPushNotificationAdd();
                PushNotificationMyTools.Title = "New Product FAQ in " + getProductName;
                PushNotificationMyTools.Body = "A new product FAQ " + getProductFAQTopic + " has been added.";
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
    public async Task<bool> ValidateUpdateProductFAQAsync(Guid ProductId, Guid productFaqId, string ProductFAQTitle, int ProductFAQSequence)
    {
      var data = await this
          .DB
          .ProductFAQs
          .AsNoTracking()
          .Where(Q => Q.ProductFaqId == productFaqId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductFaqTitle != ProductFAQTitle;

      var isExist = await this.ValidateProductFAQBySeqAndTopicCodeAsync(ProductFAQSequence, ProductFAQTitle, ProductId, data.ProductFaqCategoryId);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductFAQAsync(DeleteProductFAQModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.ProductFAQs.Where(Q => Q.ProductFaqId == model.ProductFaqId).FirstOrDefaultAsync();

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
