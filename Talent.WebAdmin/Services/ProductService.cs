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
  public class ProductService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly FileService FileServiceMan;
    private readonly ApprovalService ApprovalMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public ProductService(TalentContext db, IContextualService contextual, FileService fs, ApprovalService approvalService, IFileStorageService fm, ClaimHelper hm, BlobService bm)
    {
      this.DB = db;
      this.ContextMan = contextual;
      this.FileServiceMan = fs;
      this.ApprovalMan = approvalService;
      this.FileMan = fm;
      this.HelperMan = hm;
      this.BlobService = bm;
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
    public async Task<ProductPaginate> GetAllProduct(ProductGridFilter filter)
    {

      var query = (from sl in this.DB.Products
                   join sli in this.DB.ProductCategories on sl.ProductCategoryId equals sli.ProductCategoryId
                   where sl.IsDeleted == false
                   select new ProductModel
                   {
                     ProductId = sl.ProductId,
                     BlobId = sl.BlobId,
                     ProductCategoryId = sl.ProductCategoryId,
                     ProductName = sl.ProductName,
                     ProductCategoryName = sli.ProductCategoryName,
                     ProductSegment = sl.ProductSegment,
                     ApprovedAt = sl.ApprovedAt,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt,
                     IsCompetitor = sl.IsCompetitor
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

      if (!string.IsNullOrEmpty(filter.ProductCategoryName))
      {
        query = query.Where(Q => Q.ProductCategoryName == filter.ProductCategoryName);
      }

      if (!string.IsNullOrEmpty(filter.ProductName))
      {
        query = query.Where(Q => Q.ProductName.Contains(filter.ProductName));
      }

      //sort
      switch (filter.SortBy)
      {
        case "ProductSegmentNameAsc":
          query = query.OrderBy(Q => Q.ProductSegment);
          break;
        case "ProductCategoryNameAsc":
          query = query.OrderBy(Q => Q.ProductCategoryName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "ProductSegmentNameDesc":
          query = query.OrderByDescending(Q => Q.ProductSegment);
          break;
        case "ProductCategoryNameDesc":
          query = query.OrderByDescending(Q => Q.ProductCategoryName);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.ProductSegment);
          break;
      }
      var products = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in products)
      {

        var imageURL = "";
        var blobData = await this.BlobService.GetBlobById(datum.BlobId);

        imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

        products[index].ImageUrl = imageURL;
        index++;
      }

      return new ProductPaginate
      {
        Products = products,
        TotalProducts = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertProductAsync(ProductCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await ValidateProductByNameAsync(model.ProductName);

            if (isExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);

            if (model.ProductFileContent != null)
            {
                if (string.IsNullOrEmpty(model.ProductFileContent.Base64) == false)
                {
                    getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);
                }
            }

            var newProducts = new Products
            {
                ProductName = model.ProductName,
                ProductCategoryId = model.ProductCategoryId,
                BlobId = getUploadModuleBlob,
                ProductSegment = model.ProductSegment,
                IsCompetitor = model.IsCompetitor,
                ApprovedAt = model.ApprovedAt,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.Products.Add(newProducts);

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateProductByNameAsync(string productName)
    {
      var isExist = await this
          .DB
          .Products
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ProductName == productName);

      return isExist;
    }
    public async Task<ProductModel> GetProductById(Guid productId)
    {
      var data = await (from mo in DB.Products
                        join sl in DB.ProductCategories on mo.ProductCategoryId equals sl.ProductCategoryId
                        where mo.ProductId == productId && mo.IsDeleted == false
                        select new ProductModel
                        {
                          ProductId = mo.ProductId,
                          ProductName = mo.ProductName,
                          ProductCategoryId = mo.ProductCategoryId,
                          ProductCategoryName = sl.ProductCategoryName,
                          ProductSegment = mo.ProductSegment,
                          BlobId = mo.BlobId,
                          IsCompetitor = mo.IsCompetitor,
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
    public async Task<List<ProductIsCompetitorDropdownView>> GetProductIsCompetitor()
    {
      var data = await (from mo in DB.Products
                        where mo.IsDeleted == false && mo.IsCompetitor == true
                        select new ProductIsCompetitorDropdownView
                        {
                          ProductId = mo.ProductId,
                          ProductName = mo.ProductName,
                        }).ToListAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductAsync(ProductUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateProductNameAsync(model.ProductName, model.ProductId);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var productToUpdate = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).FirstOrDefaultAsync();

            if (model.ProductFileContent != null)
            {
                if (!String.IsNullOrEmpty(model.ProductFileContent.Base64))
                {
                    productToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);
                }
            }

            productToUpdate.ProductName = model.ProductName;
            productToUpdate.ProductCategoryId = model.ProductCategoryId;
            productToUpdate.ProductSegment = model.ProductSegment;
            productToUpdate.IsCompetitor = model.IsCompetitor;
            productToUpdate.ApprovedAt = model.ApprovedAt;
            productToUpdate.UpdatedAt = thisDate;
            productToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateUpdateProductNameAsync(string productName, Guid productId)
    {
      var data = await this
          .DB
          .Products
          .AsNoTracking()
          .Where(Q => Q.ProductId == productId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductName != productName;

      var isExist = await this.ValidateProductByNameAsync(productName);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductAsync(DeleteProductModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).FirstOrDefaultAsync();

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
