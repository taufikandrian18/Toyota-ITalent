using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
  public class ProductFAQCategoryService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly ClaimHelper HelperMan;

    public ProductFAQCategoryService(TalentContext db, IContextualService contextual, ClaimHelper hm)
    {
      this.DB = db;
      this.ContextMan = contextual;
      this.HelperMan = hm;
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
    public async Task<ProductFAQCategoryPaginate> GetAllProductFAQCategory(ProductFAQCategoryGridFilter filter)
    {

      var query = (from sl in this.DB.ProductFAQCategories
                   where sl.IsDeleted == false
                   select new ProductFAQCategoryModel
                   {
                     ProductFaqCategoryId = sl.ProductFaqCategoryId,
                     ProductId = sl.ProductId,
                     ProductFaqCategoryName = sl.ProductFaqCategoryName,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.ProductFaqCategoryName))
      {
        query = query.Where(Q => Q.ProductFaqCategoryName.ToUpper().Contains(filter.ProductFaqCategoryName.ToUpper()));
      }
      if (filter.ProductId != Guid.Empty)
      {
        query = query.Where(Q => Q.ProductId == filter.ProductId);
      }

      //sort
      switch (filter.SortBy)
      {
        case "ProductFAQCategoryNameAsc":
          query = query.OrderBy(Q => Q.ProductFaqCategoryName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "ProductFAQCategoryNameDesc":
          query = query.OrderByDescending(Q => Q.ProductFaqCategoryName);
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
      var productFAQCategories = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new ProductFAQCategoryPaginate
      {
        ProductFAQCategories = productFAQCategories,
        TotalProductFAQCategories = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertProductFAQCategoryAsync(ProductFAQCategoryCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await ValidateProductFAQCategoryByNameAsync(model.ProductFaqCategoryName);

            if (isExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var newProductFAQCategories = new ProductFAQCategories
            {
                ProductFaqCategoryName = model.ProductFaqCategoryName,
                ProductId = model.ProductId,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.ProductFAQCategories.Add(newProductFAQCategories);

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateProductFAQCategoryByNameAsync(string productFAQCategoryName)
    {
      var isExist = await this
          .DB
          .ProductFAQCategories
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ProductFaqCategoryName == productFAQCategoryName);

      return isExist;
    }
    public async Task<ProductFAQCategoryModel> GetProductFAQCategoryById(Guid ProductFaqCategoryId)
    {
      var data = await (from mo in DB.ProductFAQCategories
                        where mo.ProductFaqCategoryId == ProductFaqCategoryId && mo.IsDeleted == false
                        select new ProductFAQCategoryModel
                        {
                          ProductFaqCategoryId = mo.ProductFaqCategoryId,
                          ProductId = mo.ProductId,
                          ProductFaqCategoryName = mo.ProductFaqCategoryName,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedAt = mo.UpdatedAt,
                          UpdatedBy = mo.UpdatedBy
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductFAQCategoryAsync(ProductFAQCategoryUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateProductFAQCategoryNameAsync(model.ProductFaqCategoryName, model.ProductFaqCategoryId);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var productFAQCategoryToUpdate = await this.DB.ProductFAQCategories.Where(Q => Q.ProductFaqCategoryId == model.ProductFaqCategoryId).FirstOrDefaultAsync();

            productFAQCategoryToUpdate.ProductFaqCategoryId = model.ProductFaqCategoryId;
            productFAQCategoryToUpdate.ProductId = model.ProductId;
            productFAQCategoryToUpdate.ProductFaqCategoryName = model.ProductFaqCategoryName;
            productFAQCategoryToUpdate.UpdatedAt = thisDate;
            productFAQCategoryToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateUpdateProductFAQCategoryNameAsync(string productFAQCategoryName, Guid ProductFaqCategoryId)
    {
      var data = await this
          .DB
          .ProductFAQCategories
          .AsNoTracking()
          .Where(Q => Q.ProductFaqCategoryId == ProductFaqCategoryId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductFaqCategoryName != productFAQCategoryName;

      var isExist = await this.ValidateProductFAQCategoryByNameAsync(productFAQCategoryName);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductFAQCategoryAsync(DeleteProductFAQCategoryModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.ProductFAQCategories.Where(Q => Q.ProductFaqCategoryId == model.ProductFaqCategoryId).FirstOrDefaultAsync();

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
