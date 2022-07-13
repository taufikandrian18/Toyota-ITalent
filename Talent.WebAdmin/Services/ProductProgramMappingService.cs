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
  public class ProductProgramMappingService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileService;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;
    private readonly PushNotificationService PNService;

    public ProductProgramMappingService(TalentContext db, IContextualService contextual, ClaimHelper hm, BlobService bm, IFileStorageService fm, PushNotificationService pn)
    {
      this.DB = db;
      this.ContextMan = contextual;
      this.HelperMan = hm;
      this.FileService = fm;
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
    public async Task<ProductProgramMappingPaginate> GetAllProductProgramMapping(ProductProgramMappingGridFilter filter)
    {

      var query = (from sl in this.DB.ProductProgramMappings
                   join sla in this.DB.ProductProgramCategories on sl.ProductProgramCategoryId equals sla.ProductProgramCategoryId
                   join sle in this.DB.Cms_Contents on sl.Cms_ContentId equals sle.Cms_ContentId
                   where sl.IsDeleted == false
                   select new ProductProgramMappingModel
                   {
                     ProductProgramMappingId = sl.ProductProgramMappingId,
                     ProductId = sl.ProductId,
                     ProductProgramCategoryId = sl.ProductProgramCategoryId,
                     ProductProgramCategoryName = DB.ProductProgramCategories.Where(x => x.ProductProgramCategoryId == sl.ProductProgramCategoryId).Select(x => x.ProductProgramCategoryName).FirstOrDefault(),
                     Cms_ContentId = sl.Cms_ContentId,
                     Cms_ContentName = DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.Cms_ContentName).FirstOrDefault(),
                     Cms_ContentBlobId = DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.BlobId).FirstOrDefault().HasValue ? DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.BlobId).FirstOrDefault().Value : Guid.Empty,
                     IsSequence = sl.IsSequence,
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

      // search
      if (filter.Query != null)
      {
        query = query.Where(Q => Q.Cms_ContentName.Contains(filter.Query));
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
          query = query.OrderBy(Q => Q.IsSequence);
          break;
      }
      var productProgramMappings = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in productProgramMappings)
      {

        var imageURL = "";
        var blobData = new BlobModel();

        if (datum.Cms_ContentBlobId != null)
        {
          blobData = await this.BlobService.GetBlobById(datum.Cms_ContentBlobId);

          if (blobData != null)
          {
            imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
          }
        }

        productProgramMappings[index].Cms_ContentImageUrl = imageURL;
        index++;
      }

      return new ProductProgramMappingPaginate
      {
        ProductProgramMappings = productProgramMappings,
        TotalProductProgramMapping = totalData
      };
    }
    public async Task<ProductProgramMappingModel> GetProductProgramMappingById(Guid ProductProgramMappingId)
    {
      var data = await (from mo in DB.ProductProgramMappings
                        join sla in this.DB.ProductProgramCategories on mo.ProductProgramCategoryId equals sla.ProductProgramCategoryId
                        join sle in this.DB.Cms_Contents on mo.Cms_ContentId equals sle.Cms_ContentId
                        where mo.IsDeleted == false && mo.ProductProgramMappingId == ProductProgramMappingId
                        select new ProductProgramMappingModel
                        {
                          ProductProgramMappingId = mo.ProductProgramMappingId,
                          ProductId = mo.ProductId,
                          ProductProgramCategoryId = mo.ProductProgramCategoryId,
                          ProductProgramCategoryName = DB.ProductProgramCategories.Where(x => x.ProductProgramCategoryId == mo.ProductProgramCategoryId).Select(x => x.ProductProgramCategoryName).FirstOrDefault(),
                          Cms_ContentId = mo.Cms_ContentId,
                          Cms_ContentName = DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.Cms_ContentName).FirstOrDefault(),
                          Cms_ContentBlobId = DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.BlobId).FirstOrDefault().HasValue ? DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.BlobId).FirstOrDefault().Value : Guid.Empty,
                          IsSequence = mo.IsSequence,
                          ApprovedAt = mo.ApprovedAt,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedBy = mo.UpdatedBy,
                          UpdatedAt = mo.UpdatedAt
                        }).FirstOrDefaultAsync();

      var imageURL = "";
      var blobData = await this.BlobService.GetBlobById(data.Cms_ContentBlobId);

      imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

      data.Cms_ContentImageUrl = imageURL;

      return data;
    }
    public async Task<ActionResult<BaseResponse>> InsertProductProgramMappingAsync(ProductProgramMappingCreateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var userLogin = this.HelperMan.GetLoginUserId();

        var isExist = await ValidateProductProgramMappingAsync(model.ProductId, model.ProductProgramCategoryId, model.Cms_ContentId);

        if (isExist)
        {
          return BadRequest(BaseResponse.BadRequest(model));
        }

        var insertProductProgramMappings = new ProductProgramMappings
        {
          ProductId = model.ProductId,
          ProductProgramCategoryId = model.ProductProgramCategoryId,
          Cms_MenuId = null,
          Cms_ContentId = model.Cms_ContentId,
          IsSequence = model.IsSequence,
          ApprovedAt = model.ApprovedAt,
          CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
          UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
          CreatedBy = userLogin,
          UpdatedBy = userLogin,
        };

        this.DB.ProductProgramMappings.Add(insertProductProgramMappings);

        await this.DB.SaveChangesAsync();
        if (model.ApprovedAt != null)
        {
          var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
          var ProductTypeId = await this.DB.ProductTypes.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductTypeId).FirstOrDefaultAsync();
          var cms_ContentId = insertProductProgramMappings.Cms_ContentId;
          var getProductSPWAContentName = await this.DB.Cms_Contents.Where(Q => Q.Cms_ContentId == cms_ContentId).Select(Q => Q.Cms_ContentName).FirstOrDefaultAsync();
          var groupPositionList = new List<string>();
          var manPowerPositionList = new List<string>();
          var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
          var PushNotificationMyTools = new VMPushNotificationAdd();
          PushNotificationMyTools.Title = "New Product Program from" + getProductName;
          PushNotificationMyTools.Body = "A new product Program " + getProductSPWAContentName + " has been added.";
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
    public async Task<bool> ValidateProductProgramMappingAsync(Guid productId, Guid productProgramCategoryId, Guid Cms_ContentId)
    {
      var isExist = await this
          .DB
          .ProductProgramMappings
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ProductId == productId && Q.ProductProgramCategoryId == productProgramCategoryId && Q.Cms_ContentId == Cms_ContentId);

      return isExist;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductProgramMapping(ProductProgramMappingUpdateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isChangedAndExist = await ValidateUpdateProductProgramMappingAsync(model.ProductProgramMappingId, model.ProductId, model.ProductProgramCategoryId, model.Cms_ContentId);

        if (isChangedAndExist)
        {
          return BadRequest(BaseResponse.BadRequest(model));
        }

        var productProgramMappingToUpdate = await this.DB.ProductProgramMappings.Where(Q => Q.ProductProgramMappingId == model.ProductProgramMappingId).FirstOrDefaultAsync();

        productProgramMappingToUpdate.ProductProgramMappingId = model.ProductProgramMappingId;
        productProgramMappingToUpdate.ProductId = model.ProductId;
        productProgramMappingToUpdate.ProductProgramCategoryId = model.ProductProgramCategoryId;
        productProgramMappingToUpdate.Cms_MenuId = null;
        productProgramMappingToUpdate.Cms_ContentId = model.Cms_ContentId;
        productProgramMappingToUpdate.IsSequence = model.IsSequence;
        productProgramMappingToUpdate.ApprovedAt = model.ApprovedAt;
        productProgramMappingToUpdate.UpdatedAt = thisDate;
        productProgramMappingToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

        await this.DB.SaveChangesAsync();
        if (model.ApprovedAt != null)
        {
          var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
          var ProductTypeId = await this.DB.ProductTypes.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductTypeId).FirstOrDefaultAsync();
          var cms_ContentId = model.Cms_ContentId;
          var getProductSPWAContentName = await this.DB.Cms_Contents.Where(Q => Q.Cms_ContentId == cms_ContentId).Select(Q => Q.Cms_ContentName).FirstOrDefaultAsync();
          var groupPositionList = new List<string>();
          var manPowerPositionList = new List<string>();
          var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
          var PushNotificationMyTools = new VMPushNotificationAdd();
          PushNotificationMyTools.Title = "Update Product Program from" + getProductName;
          PushNotificationMyTools.Body = "Product Program " + getProductSPWAContentName + " has been updated.";
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
    public async Task<bool> ValidateUpdateProductProgramMappingAsync(Guid productProgramMappingId, Guid productId, Guid productProgramCategoryId, Guid Cms_ContentId)
    {
      var data = await this
          .DB
          .ProductProgramMappings
          .AsNoTracking()
          .Where(Q => Q.ProductProgramMappingId == productProgramMappingId)
          .FirstOrDefaultAsync();

      var isChange = data.Cms_ContentId != Cms_ContentId;

      var isExist = await this.ValidateProductProgramMappingAsync(productId, productProgramCategoryId, Cms_ContentId);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductProgramMappingAsync(DeleteProductProgramMappingModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.ProductProgramMappings.Where(Q => Q.ProductProgramMappingId == model.ProductProgramMappingId).FirstOrDefaultAsync();

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
