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
  public class ProductFeatureService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public ProductFeatureService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bm)
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
    public async Task<ProductFeaturePaginate> GetAllProductFeature(ProductFeatureGridFilter filter)
    {

      var query = (from sl in this.DB.ProductFeatures
                   where sl.IsDeleted == false
                   select new ProductFeatureModel
                   {
                     ProductFeatureId = sl.ProductFeatureId,
                     ProductFeatureName = sl.ProductFeatureName,
                     BlobId = sl.BlobId,
                     ProductId = sl.ProductId,
                     IsApproved = sl.IsApproved,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt,
                     ApprovedAt = sl.ApprovedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.ProductFeatureName))
      {
        query = query.Where(Q => Q.ProductFeatureName == filter.ProductFeatureName);
      }

      //if (filter.ProductId != Guid.Empty)
      //{
      //  query = query.Where(Q => Q.ProductId == filter.ProductId);
      //}

      //sort
      switch (filter.SortBy)
      {
        case "ProductCategoryNameAsc":
          query = query.OrderBy(Q => Q.ProductFeatureName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "ProductCategoryNameDesc":
          query = query.OrderByDescending(Q => Q.ProductFeatureName);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.ProductFeatureName);
          break;
      }
      var productFeatures = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();
      var index = 0;
      foreach (var datum in productFeatures)
      {

        var imageURL = "";
        var blobData = new BlobModel();
        if (datum.BlobId != null)
        {
          blobData = await this.BlobService.GetBlobById(datum.BlobId);

          imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
        }

        productFeatures[index].ImageUrl = imageURL;
        productFeatures[index].Blob = blobData;

        index++;
      }

      return new ProductFeaturePaginate
      {
        ProductFeatures = productFeatures,
        TotalProductFeatures = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertProductFeatureAsync(ProductFeatureCreateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isExist = await ValidateProductFeatureByNameAsync(model.ProductFeatureName);

        if (isExist)
        {
          return BadRequest(BaseResponse.BadRequest(model));
        }

        var getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductFeatureFileContent);

        if (model.ProductFeatureFileContent != null)
        {
          if (string.IsNullOrEmpty(model.ProductFeatureFileContent.Base64) == false)
          {
            getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductFeatureFileContent);
          }
        }

        var newProductFeatures = new ProductFeatures
        {
          ProductFeatureName = model.ProductFeatureName,
          BlobId = getUploadModuleBlob,
          ProductId = model.ProductId,
          IsApproved = "Published",
          ApprovedAt = model.ApprovedAt,
          CreatedAt = thisDate,
          UpdatedAt = thisDate,
          IsDeleted = false,
          CreatedBy = this.HelperMan.GetLoginUserId(),
          UpdatedBy = this.HelperMan.GetLoginUserId()
        };

        this.DB.ProductFeatures.Add(newProductFeatures);

        await this.DB.SaveChangesAsync();
        return BaseResponse.ResponseOk();
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }
    public async Task<bool> ValidateProductFeatureByNameAsync(string productFeatureName)
    {
      var isExist = await this
          .DB
          .ProductFeatures
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ProductFeatureName == productFeatureName);

      return isExist;
    }
    public async Task<ProductFeatureModel> GetProductFeatureById(Guid productFeatureId)
    {
      var data = await (from mo in DB.ProductFeatures
                        where mo.ProductFeatureId == productFeatureId && mo.IsDeleted == false
                        select new ProductFeatureModel
                        {
                          ProductFeatureId = mo.ProductFeatureId,
                          ProductFeatureName = mo.ProductFeatureName,
                          BlobId = mo.BlobId,
                          ProductId = mo.ProductId,
                          IsApproved = mo.IsApproved,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedAt = mo.UpdatedAt,
                          UpdatedBy = mo.UpdatedBy,
                          ApprovedAt = mo.ApprovedAt
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductFeatureAsync(ProductFeatureUpdateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isChangedAndExist = await ValidateUpdateProductFeatureNameAsync(model.ProductFeatureName, model.ProductFeatureId);

        if (isChangedAndExist)
        {
          return BadRequest(BaseResponse.BadRequest(model));
        }

        var productFeatureToUpdate = await this.DB.ProductFeatures.Where(Q => Q.ProductFeatureId == model.ProductFeatureId).FirstOrDefaultAsync();

        if (model.ProductFeatureFileContent != null)
        {
          if (!String.IsNullOrEmpty(model.ProductFeatureFileContent.Base64))
          {
            productFeatureToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.ProductFeatureFileContent);
          }
        }

        productFeatureToUpdate.ProductFeatureId = model.ProductFeatureId;
        productFeatureToUpdate.ProductFeatureName = model.ProductFeatureName;
        productFeatureToUpdate.ProductId = model.ProductId;
        productFeatureToUpdate.ApprovedAt = model.ApprovedAt;
        productFeatureToUpdate.UpdatedAt = thisDate;
        productFeatureToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

        await this.DB.SaveChangesAsync();
        return BaseResponse.ResponseOk();
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }
    public async Task<bool> ValidateUpdateProductFeatureNameAsync(string productFeatureName, Guid productFeatureId)
    {
      var data = await this
          .DB
          .ProductFeatures
          .AsNoTracking()
          .Where(Q => Q.ProductFeatureId == productFeatureId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductFeatureName != productFeatureName;

      var isExist = await this.ValidateProductFeatureByNameAsync(productFeatureName);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductFeatureAsync(DeleteProductFeatureModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.ProductFeatures.Where(Q => Q.ProductFeatureId == model.ProductFeatureId).FirstOrDefaultAsync();

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
    public async Task<ActionResult<BaseResponse>> UpdateProductFeatureByApprovalAsync(UpdateProductFeatureApprovedModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.ProductFeatures.Where(Q => Q.ProductFeatureId == model.ProductFeatureId).FirstOrDefaultAsync();

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
