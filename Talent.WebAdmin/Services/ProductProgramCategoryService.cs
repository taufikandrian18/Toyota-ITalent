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
  public class ProductProgramCategoryService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public ProductProgramCategoryService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bm)
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
    public async Task<ProductProgramCategoryPaginate> GetAllProductProgramCategory(ProductProgramCategoryGridFilter filter)
    {

      var query = (from sl in this.DB.ProductProgramCategories
                   where sl.IsDeleted == false
                   select new ProductProgramCategoryModel
                   {
                     ProductProgramCategoryId = sl.ProductProgramCategoryId,
                     ProductId = sl.ProductId,
                     BlobId = sl.BlobId,
                     ProductProgramCategoryName = sl.ProductProgramCategoryName,
                     ProductProgramCategoryDescription = sl.ProductProgramCategoryDescription,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.ProductProgramCategoryName))
      {
        query = query.Where(Q => Q.ProductProgramCategoryName.ToUpper().Contains(filter.ProductProgramCategoryName.ToUpper()));
      }

      if (filter.ProductId != Guid.Empty)
      {
        query = query.Where(Q => Q.ProductId == filter.ProductId);
      }

      //sort
      switch (filter.SortBy)
      {
        case "ProductProgramCategoryNameAsc":
          query = query.OrderBy(Q => Q.ProductProgramCategoryName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "ProductProgramCategoryNameDesc":
          query = query.OrderByDescending(Q => Q.ProductProgramCategoryName);
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
      var productProgramCategories = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in productProgramCategories)
      {

        var imageURL = "";
        var blobData = new BlobModel();

        if (datum.BlobId != null)
        {
          blobData = await this.BlobService.GetBlobById(datum.BlobId);

          imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
        }


        productProgramCategories[index].ImageUrl = imageURL;
        productProgramCategories[index].Blob = blobData;

        index++;
      }

      return new ProductProgramCategoryPaginate
      {
        ProductProgramCategories = productProgramCategories,
        TotalProductProgramCategories = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertProductProgramCategoryAsync(ProductProgramCategoryCreateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isExist = await ValidateProductProgramCategoryByNameAsync(model.ProductProgramCategoryName);

        // if (isExist)
        // {
        //     return BadRequest(BaseResponse.BadRequest(model));
        // }

        Guid? getUploadBlob = null;

        if (model.ProductFileContent != null)
        {
          if (string.IsNullOrEmpty(model.ProductFileContent.Base64) == false)
          {
            getUploadBlob = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);
          }
        }

        var newProductProgramCategories = new ProductProgramCategories
        {
          ProductProgramCategoryName = model.ProductProgramCategoryName,
          ProductProgramCategoryDescription = model.ProductProgramCategoryDescription,
          BlobId = getUploadBlob,
          ProductId = model.ProductId,
          CreatedAt = thisDate,
          UpdatedAt = thisDate,
          IsDeleted = false,
          CreatedBy = this.HelperMan.GetLoginUserId(),
          UpdatedBy = this.HelperMan.GetLoginUserId()
        };

        this.DB.ProductProgramCategories.Add(newProductProgramCategories);

        await this.DB.SaveChangesAsync();
        return BaseResponse.ResponseOk();
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }
    public async Task<bool> ValidateProductProgramCategoryByNameAsync(string productProgramCategoryName)
    {
      var isExist = await this
          .DB
          .ProductProgramCategories
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ProductProgramCategoryName == productProgramCategoryName);

      return isExist;
    }
    public async Task<ProductProgramCategoryModel> GetProductProgramCategoryById(Guid ProductProgramCategoryId)
    {
      var data = await (from mo in DB.ProductProgramCategories
                        where mo.ProductProgramCategoryId == ProductProgramCategoryId && mo.IsDeleted == false
                        select new ProductProgramCategoryModel
                        {
                          ProductProgramCategoryId = mo.ProductProgramCategoryId,
                          BlobId = mo.BlobId,
                          ProductId = mo.ProductId,
                          ProductProgramCategoryName = mo.ProductProgramCategoryName,
                          ProductProgramCategoryDescription = mo.ProductProgramCategoryDescription,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedBy = mo.UpdatedBy,
                          UpdatedAt = mo.UpdatedAt
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductProgramCategoryAsync(ProductProgramCategoryUpdateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isChangedAndExist = await ValidateUpdateProductProgramCategoryNameAsync(model.ProductProgramCategoryName, model.ProductProgramCategoryId);

        // if (isChangedAndExist)
        // {
        //   return BadRequest(BaseResponse.BadRequest(model));
        // }

        var productProgramCategoryToUpdate = await this.DB.ProductProgramCategories.Where(Q => Q.ProductProgramCategoryId == model.ProductProgramCategoryId).FirstOrDefaultAsync();

        if (model.ProductFileContent != null)
        {
          if (!String.IsNullOrEmpty(model.ProductFileContent.Base64))
          {
            productProgramCategoryToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);
          }
        }

        productProgramCategoryToUpdate.ProductProgramCategoryId = model.ProductProgramCategoryId;
        productProgramCategoryToUpdate.ProductId = model.ProductId;
        productProgramCategoryToUpdate.ProductProgramCategoryName = model.ProductProgramCategoryName;
        productProgramCategoryToUpdate.ProductProgramCategoryDescription = model.ProductProgramCategoryDescription;
        productProgramCategoryToUpdate.UpdatedAt = thisDate;
        productProgramCategoryToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

        await this.DB.SaveChangesAsync();
        return BaseResponse.ResponseOk();
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }
    public async Task<bool> ValidateUpdateProductProgramCategoryNameAsync(string productProgramCategoryName, Guid ProductProgramCategoryId)
    {
      var data = await this
          .DB
          .ProductProgramCategories
          .AsNoTracking()
          .Where(Q => Q.ProductProgramCategoryId == ProductProgramCategoryId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductProgramCategoryName != productProgramCategoryName;

      var isExist = await this.ValidateProductProgramCategoryByNameAsync(productProgramCategoryName);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductProgramCategoryAsync(DeleteProductProgramCategoryModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.ProductProgramCategories.Where(Q => Q.ProductProgramCategoryId == model.ProductProgramCategoryId).FirstOrDefaultAsync();

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
