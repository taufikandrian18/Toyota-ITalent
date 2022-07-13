using System;
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
  public class ProductSPWACategoryService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public ProductSPWACategoryService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bm)
    {
      this.DB = db;
      this.ContextMan = contextual;
      this.HelperMan = hm;
      this.FileMan = fm;
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
    public async Task<ProductSPWACategoryPaginate> GetAllProductSPWACategory(ProductSPWACategoryGridFilter filter)
    {

      var query = (from sl in this.DB.ProductSPWACategories
                   where sl.IsDeleted == false
                   select new ProductSPWACategoryModel
                   {
                     ProductSPWACategoryId = sl.ProductSPWACategoryId,
                     ProductId = sl.ProductId,
                     BlobId = sl.BlobId,
                     ProductSPWACategoryName = sl.ProductSPWACategoryName,
                     ProductSPWACategoryDescription = sl.ProductSPWACategoryDescription,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.ProductSPWACategoryName))
      {
        query = query.Where(Q => Q.ProductSPWACategoryName.ToUpper().Contains(filter.ProductSPWACategoryName.ToUpper()));
      }

      if (filter.ProductId != Guid.Empty)
      {
        query = query.Where(Q => Q.ProductId == filter.ProductId);
      }

      //sort
      switch (filter.SortBy)
      {
        case "ProductSPWACategoryNameAsc":
          query = query.OrderBy(Q => Q.ProductSPWACategoryName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "ProductSPWACategoryNameDesc":
          query = query.OrderByDescending(Q => Q.ProductSPWACategoryName);
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
      var productSPWACategories = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in productSPWACategories)
      {

        var imageURL = "";
        var blobData = new BlobModel();

        if (datum.BlobId != null)
        {
          blobData = await this.BlobService.GetBlobById(datum.BlobId);

          imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
        }


        productSPWACategories[index].ImageUrl = imageURL;
        productSPWACategories[index].Blob = blobData;

        index++;
      }

      return new ProductSPWACategoryPaginate
      {
        ProductSPWACategories = productSPWACategories,
        TotalProductSPWACategories = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertProductSPWACategoryAsync(ProductSPWACategoryCreateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isExist = await ValidateProductSPWACategoryByNameAsync(model.ProductSPWACategoryName, model.ProductId);

        // if (isExist)
        // {
        //   return BadRequest(BaseResponse.BadRequest(model));
        // }

        Guid? getUploadBlob = null;

        if (model.ProductFileContent != null)
        {
          if (string.IsNullOrEmpty(model.ProductFileContent.Base64) == false)
          {
            getUploadBlob = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);
          }
        }

        var newProductSPWACategories = new ProductSPWACategories
        {
          ProductSPWACategoryName = model.ProductSPWACategoryName,
          ProductSPWACategoryDescription = model.ProductSPWACategoryDescription,
          ProductId = model.ProductId,
          BlobId = getUploadBlob,
          CreatedAt = thisDate,
          UpdatedAt = thisDate,
          IsDeleted = false,
          CreatedBy = this.HelperMan.GetLoginUserId(),
          UpdatedBy = this.HelperMan.GetLoginUserId()
        };

        this.DB.ProductSPWACategories.Add(newProductSPWACategories);

        await this.DB.SaveChangesAsync();
        return BaseResponse.ResponseOk();
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }
    public async Task<bool> ValidateProductSPWACategoryByNameAsync(string productSPWACategoryName, Guid productId)
    {
      var isExist = await this
          .DB
          .ProductSPWACategories
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ProductSPWACategoryName == productSPWACategoryName && Q.ProductId == productId);

      return isExist;
    }
    public async Task<ProductSPWACategoryModel> GetProductSPWACategoryById(Guid ProductSPWACategoryId)
    {
      var data = await (from mo in DB.ProductSPWACategories
                        where mo.ProductSPWACategoryId == ProductSPWACategoryId && mo.IsDeleted == false
                        select new ProductSPWACategoryModel
                        {
                          ProductSPWACategoryId = mo.ProductSPWACategoryId,
                          BlobId = mo.BlobId,
                          ProductId = mo.ProductId,
                          ProductSPWACategoryName = mo.ProductSPWACategoryName,
                          ProductSPWACategoryDescription = mo.ProductSPWACategoryDescription,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedBy = mo.UpdatedBy,
                          UpdatedAt = mo.UpdatedAt
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductSPWACategoryAsync(ProductSPWACategoryUpdateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isChangedAndExist = await ValidateUpdateProductSPWACategoryNameAsync(model.ProductSPWACategoryName, model.ProductSPWACategoryId, model.ProductId);

        // if (isChangedAndExist)
        // {
        //   return BadRequest(BaseResponse.BadRequest(model));
        // }

        var productSPWACategoryToUpdate = await this.DB.ProductSPWACategories.Where(Q => Q.ProductSPWACategoryId == model.ProductSPWACategoryId).FirstOrDefaultAsync();

        if (model.ProductFileContent != null)
        {
          if (!String.IsNullOrEmpty(model.ProductFileContent.Base64))
          {
            productSPWACategoryToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);
          }
        }

        productSPWACategoryToUpdate.ProductSPWACategoryId = model.ProductSPWACategoryId;
        productSPWACategoryToUpdate.ProductId = model.ProductId;
        productSPWACategoryToUpdate.ProductSPWACategoryName = model.ProductSPWACategoryName;
        productSPWACategoryToUpdate.ProductSPWACategoryDescription = model.ProductSPWACategoryDescription;
        productSPWACategoryToUpdate.UpdatedAt = thisDate;
        productSPWACategoryToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

        await this.DB.SaveChangesAsync();
        return BaseResponse.ResponseOk();
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }
    public async Task<bool> ValidateUpdateProductSPWACategoryNameAsync(string productSPWACategoryName, Guid ProductSPWACategoryId, Guid productId)
    {
      var data = await this
          .DB
          .ProductSPWACategories
          .AsNoTracking()
          .Where(Q => Q.ProductSPWACategoryId == ProductSPWACategoryId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductSPWACategoryName != productSPWACategoryName;

      var isExist = await this.ValidateProductSPWACategoryByNameAsync(productSPWACategoryName, productId);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductSPWACategoryAsync(DeleteProductSPWACategoryModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.ProductSPWACategories.Where(Q => Q.ProductSPWACategoryId == model.ProductSPWACategoryId).FirstOrDefaultAsync();

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
