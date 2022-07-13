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
  public class ProductCategoryService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly ClaimHelper HelperMan;

    public ProductCategoryService(TalentContext db, IContextualService contextual, ClaimHelper hm)
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
    public async Task<ProductCategoryPaginate> GetAllProductCategory(ProductCategoryGridFilter filter)
    {

      var query = (from sl in this.DB.ProductCategories
                   where sl.IsDeleted == false
                   select new ProductCategoryModel
                   {
                     ProductCategoryId = sl.ProductCategoryId,
                     ProductCategoryName = sl.ProductCategoryName,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.ProductCategoryName))
      {
        query = query.Where(Q => Q.ProductCategoryName.Contains(filter.ProductCategoryName));
      }

      //sort
      switch (filter.SortBy)
      {
        case "ProductCategoryNameAsc":
          query = query.OrderBy(Q => Q.ProductCategoryName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
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
          query = query.OrderBy(Q => Q.ProductCategoryName);
          break;
      }
      var productCategories = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new ProductCategoryPaginate
      {
        ProductCategories = productCategories,
        TotalProductCategories = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertProductCategoryAsync(ProductCategoryCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await ValidateProductCategoryByNameAsync(model.ProductCategoryName);

            if (isExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var newProductCategories = new ProductCategories
            {
                ProductCategoryName = model.ProductCategoryName,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.ProductCategories.Add(newProductCategories);

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateProductCategoryByNameAsync(string productCategoryName)
    {
      var isExist = await this
          .DB
          .ProductCategories
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ProductCategoryName == productCategoryName);

      return isExist;
    }
    public async Task<ProductCategoryModel> GetProductCategoryById(Guid productCategoryId)
    {
      var data = await (from mo in DB.ProductCategories
                        where mo.ProductCategoryId == productCategoryId && mo.IsDeleted == false
                        select new ProductCategoryModel
                        {
                          ProductCategoryId = mo.ProductCategoryId,
                          ProductCategoryName = mo.ProductCategoryName,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedAt = mo.UpdatedAt,
                          UpdatedBy = mo.UpdatedBy
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductCategoryAsync(ProductCategoryUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateProductCategoryNameAsync(model.ProductCategoryName, model.ProductCategoryId);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var productToUpdate = await this.DB.ProductCategories.Where(Q => Q.ProductCategoryId == model.ProductCategoryId).FirstOrDefaultAsync();

            productToUpdate.ProductCategoryId = model.ProductCategoryId;
            productToUpdate.ProductCategoryName = model.ProductCategoryName;
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
    public async Task<bool> ValidateUpdateProductCategoryNameAsync(string productCategoryName, Guid productCategoryId)
    {
      var data = await this
          .DB
          .ProductCategories
          .AsNoTracking()
          .Where(Q => Q.ProductCategoryId == productCategoryId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductCategoryName != productCategoryName;

      var isExist = await this.ValidateProductCategoryByNameAsync(productCategoryName);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductCategoryAsync(DeleteProductCategoryModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.ProductCategories.Where(Q => Q.ProductCategoryId == model.ProductCategoryId).FirstOrDefaultAsync();

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
