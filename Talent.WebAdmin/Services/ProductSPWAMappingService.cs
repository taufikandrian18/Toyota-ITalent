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
  public class ProductSPWAMappingService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileService;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;
    private readonly PushNotificationService PNService;

    public ProductSPWAMappingService(TalentContext db, IContextualService contextual, ClaimHelper hm, BlobService bm, IFileStorageService fm, PushNotificationService pn)
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
    public async Task<ProductSPWAMappingPaginate> GetAllProductSPWAMapping(ProductSPWAMappingGridFilter filter)
    {

      var query = (from sl in this.DB.ProductSPWAMappings
                   join sla in this.DB.ProductSPWACategories on sl.ProductSPWACategoryId equals sla.ProductSPWACategoryId
                   join slb in this.DB.Cms_Menus on sl.Cms_MenuId equals slb.Cms_MenuId
                   join sle in this.DB.Cms_Contents on sl.Cms_ContentId equals sle.Cms_ContentId
                   where sl.IsDeleted == false
                   select new ProductSPWAMappingModel
                   {
                     ProductSPWAMappingId = sl.ProductSPWAMappingId,
                     ProductId = sl.ProductId,
                     ProductSPWACategoryId = sl.ProductSPWACategoryId,
                     ProductSPWACategoryName = DB.ProductSPWACategories.Where(x => x.ProductSPWACategoryId == sl.ProductSPWACategoryId).Select(x => x.ProductSPWACategoryName).FirstOrDefault(),
                     Cms_ContentId = sl.Cms_ContentId,
                     Cms_ContentName = DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.Cms_ContentName).FirstOrDefault(),
                     Cms_ContentBlobId = DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.BlobId).FirstOrDefault().HasValue ? DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.BlobId).FirstOrDefault().Value : Guid.Empty,
                     Cms_MenuId = sl.Cms_MenuId,
                     Cms_MenuName = DB.Cms_Menus.Where(x => x.Cms_MenuId == sl.Cms_MenuId).Select(x => x.Cms_MenuName).FirstOrDefault(),
                     Cms_MenuCategory = DB.Cms_Menus.Where(x => x.Cms_MenuId == sl.Cms_MenuId).Select(x => x.Cms_MenuCategory).FirstOrDefault(),
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
      if (!string.IsNullOrEmpty(filter.Query))
      {
        query = query.Where(Q => Q.Cms_MenuName.Contains(filter.Query) || Q.Cms_ContentName.Contains(filter.Query));
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
      var productSPWAMappings = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in productSPWAMappings)
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

        productSPWAMappings[index].Cms_ContentImageUrl = imageURL;
        index++;
      }

      return new ProductSPWAMappingPaginate
      {
        ProductSPWAMappings = productSPWAMappings,
        TotalProductSPWAMapping = totalData
      };
    }
    public async Task<ProductSPWAMappingModel> GetProductSPWAMappingById(Guid ProductSPWAMappingId)
    {
      var data = await (from mo in DB.ProductSPWAMappings
                        join sla in this.DB.ProductSPWACategories on mo.ProductSPWACategoryId equals sla.ProductSPWACategoryId
                        join slb in this.DB.Cms_Menus on mo.Cms_MenuId equals slb.Cms_MenuId
                        join sle in this.DB.Cms_Contents on mo.Cms_ContentId equals sle.Cms_ContentId
                        where mo.IsDeleted == false && mo.ProductSPWAMappingId == ProductSPWAMappingId
                        select new ProductSPWAMappingModel
                        {
                          ProductSPWAMappingId = mo.ProductSPWAMappingId,
                          ProductId = mo.ProductId,
                          ProductSPWACategoryId = mo.ProductSPWACategoryId,
                          ProductSPWACategoryName = DB.ProductSPWACategories.Where(x => x.ProductSPWACategoryId == mo.ProductSPWACategoryId).Select(x => x.ProductSPWACategoryName).FirstOrDefault(),
                          Cms_ContentId = mo.Cms_ContentId,
                          Cms_ContentName = DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.Cms_ContentName).FirstOrDefault(),
                          Cms_ContentBlobId = DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.BlobId).FirstOrDefault().HasValue ? DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.BlobId).FirstOrDefault().Value : Guid.Empty,
                          Cms_MenuId = mo.Cms_MenuId,
                          Cms_MenuName = DB.Cms_Menus.Where(x => x.Cms_MenuId == mo.Cms_MenuId).Select(x => x.Cms_MenuName).FirstOrDefault(),
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
    public async Task<ActionResult<BaseResponse>> InsertProductSPWAMappingAsync(ProductSPWAMappingCreateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var userLogin = this.HelperMan.GetLoginUserId();

        var isExist = await ValidateProductSPWAMappingAsync(model.ProductId, model.ProductSPWACategoryId, model.Cms_MenuId, model.Cms_ContentId);

        if (isExist)
        {
          return BadRequest(BaseResponse.BadRequest(model));
        }

        var insertProductSPWAMappings = new ProductSPWAMappings
        {
          ProductId = model.ProductId,
          ProductSPWACategoryId = model.ProductSPWACategoryId,
          Cms_MenuId = model.Cms_MenuId,
          Cms_ContentId = model.Cms_ContentId,
          IsSequence = model.IsSequence,
          ApprovedAt = model.ApprovedAt,
          CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
          UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
          CreatedBy = userLogin,
          UpdatedBy = userLogin,
        };

        this.DB.ProductSPWAMappings.Add(insertProductSPWAMappings);

        await this.DB.SaveChangesAsync();
        if (model.ApprovedAt != null)
        {
          var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
          var ProductTypeId = await this.DB.ProductTypes.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductTypeId).FirstOrDefaultAsync();
          var cms_ContentId = insertProductSPWAMappings.Cms_ContentId;
          var getProductSPWAContentName = await this.DB.Cms_Contents.Where(Q => Q.Cms_ContentId == cms_ContentId).Select(Q => Q.Cms_ContentName).FirstOrDefaultAsync();
          var groupPositionList = new List<string>();
          var manPowerPositionList = new List<string>();
          var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
          var PushNotificationMyTools = new VMPushNotificationAdd();
          PushNotificationMyTools.Title = "New Product SPWA from" + getProductName;
          PushNotificationMyTools.Body = "A new product SPWA " + getProductSPWAContentName + " has been added.";
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
    public async Task<bool> ValidateProductSPWAMappingAsync(Guid productId, Guid productSPWACategoryId, Guid Cms_MenuId, Guid Cms_ContentId)
    {
      var isExist = await this
          .DB
          .ProductSPWAMappings
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ProductId == productId && Q.ProductSPWACategoryId == productSPWACategoryId && Q.Cms_MenuId == Cms_MenuId && Q.Cms_ContentId == Cms_ContentId);

      return isExist;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductSPWAMapping(ProductSPWAMappingUpdateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isChangedAndExist = await ValidateUpdateProductSPWAMappingAsync(model.ProductSPWAMappingId, model.ProductId, model.ProductSPWACategoryId, model.Cms_MenuId, model.Cms_ContentId);

        if (isChangedAndExist)
        {
          return BadRequest(BaseResponse.BadRequest(model));
        }

        var productSPWAMappingToUpdate = await this.DB.ProductSPWAMappings.Where(Q => Q.ProductSPWAMappingId == model.ProductSPWAMappingId).FirstOrDefaultAsync();

        productSPWAMappingToUpdate.ProductSPWAMappingId = model.ProductSPWAMappingId;
        productSPWAMappingToUpdate.ProductId = model.ProductId;
        productSPWAMappingToUpdate.ProductSPWACategoryId = model.ProductSPWACategoryId;
        productSPWAMappingToUpdate.Cms_MenuId = model.Cms_MenuId;
        productSPWAMappingToUpdate.Cms_ContentId = model.Cms_ContentId;
        productSPWAMappingToUpdate.IsSequence = model.IsSequence;
        productSPWAMappingToUpdate.ApprovedAt = model.ApprovedAt;
        productSPWAMappingToUpdate.UpdatedAt = thisDate;
        productSPWAMappingToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

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
          PushNotificationMyTools.Title = "Update Product SPWA from" + getProductName;
          PushNotificationMyTools.Body = "Product SPWA " + getProductSPWAContentName + " has been updated.";
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
    public async Task<bool> ValidateUpdateProductSPWAMappingAsync(Guid productSPWAMappingId, Guid productId, Guid productSPWACategoryId, Guid Cms_MenuId, Guid Cms_ContentId)
    {
      var data = await this
          .DB
          .ProductSPWAMappings
          .AsNoTracking()
          .Where(Q => Q.ProductSPWAMappingId == productSPWAMappingId)
          .FirstOrDefaultAsync();

      var isChange = data.Cms_ContentId != Cms_ContentId;

      var isExist = await this.ValidateProductSPWAMappingAsync(productId, productSPWACategoryId, Cms_MenuId, Cms_ContentId);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductSPWAMappingAsync(DeleteProductSPWAMappingModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.ProductSPWAMappings.Where(Q => Q.ProductSPWAMappingId == model.ProductSPWAMappingId).FirstOrDefaultAsync();

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
