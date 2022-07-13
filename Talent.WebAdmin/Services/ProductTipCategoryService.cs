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
  public class ProductTipCategoryService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly ClaimHelper HelperMan;

    public ProductTipCategoryService(TalentContext db, IContextualService contextual, ClaimHelper hm)
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
    public async Task<ProductTipCategoryPaginate> GetAllProductTipCategory(ProductTipCategoryGridFilter filter)
    {

      var query = (from sl in this.DB.ProductTipCategories
                   where sl.IsDeleted == false
                   select new ProductTipCategoryModel
                   {
                     ProductTipCategoryId = sl.ProductTipCategoryId,
                     ProductId = sl.ProductId,
                     ProductTipCategoryName = sl.ProductTipCategoryName,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.ProductTipCategoryName))
      {
                query = query.Where(Q => Q.ProductTipCategoryName.ToUpper().Contains(filter.ProductTipCategoryName.ToUpper()));
      }
      if(filter.ProductId != Guid.Empty)
      {
                query = query.Where(Q => Q.ProductId == filter.ProductId);
      }

      //sort
      switch (filter.SortBy)
      {
        case "ProductTipCategoryNameAsc":
          query = query.OrderBy(Q => Q.ProductTipCategoryName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "ProductFAQCategoryNameDesc":
          query = query.OrderByDescending(Q => Q.ProductTipCategoryName);
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
      var productTipCategories = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new ProductTipCategoryPaginate
      {
        ProductTipCategories = productTipCategories,
        TotalProductTipCategories = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertProductTipCategoryAsync(ProductTipCategoryCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await ValidateProductTipCategoryByNameAsync(model.ProductTipCategoryName);

            if (isExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var newProductTipCategories = new ProductTipCategories
            {
                ProductTipCategoryName = model.ProductTipCategoryName,
                ProductId = model.ProductId,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.ProductTipCategories.Add(newProductTipCategories);

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateProductTipCategoryByNameAsync(string productTipCategoryName)
    {
      var isExist = await this
          .DB
          .ProductTipCategories
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ProductTipCategoryName == productTipCategoryName);

      return isExist;
    }
    public async Task<ProductTipCategoryModel> GetProductTipCategoryById(Guid ProductTipCategoryId)
    {
      var data = await (from mo in DB.ProductTipCategories
                        where mo.ProductTipCategoryId == ProductTipCategoryId && mo.IsDeleted == false
                        select new ProductTipCategoryModel
                        {
                          ProductTipCategoryId = mo.ProductTipCategoryId,
                          ProductId = mo.ProductId,
                          ProductTipCategoryName = mo.ProductTipCategoryName,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedAt = mo.UpdatedAt,
                          UpdatedBy = mo.UpdatedBy
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductTipCategoryAsync(ProductTipCategoryUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateProductTipCategoryNameAsync(model.ProductTipCategoryName, model.ProductTipCategoryId);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var productTipCategoryToUpdate = await this.DB.ProductTipCategories.Where(Q => Q.ProductTipCategoryId == model.ProductTipCategoryId).FirstOrDefaultAsync();

            productTipCategoryToUpdate.ProductTipCategoryId = model.ProductTipCategoryId;
            productTipCategoryToUpdate.ProductId = model.ProductId;
            productTipCategoryToUpdate.ProductTipCategoryName = model.ProductTipCategoryName;
            productTipCategoryToUpdate.UpdatedAt = thisDate;
            productTipCategoryToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateUpdateProductTipCategoryNameAsync(string productTipCategoryName, Guid ProductTipCategoryId)
    {
      var data = await this
          .DB
          .ProductTipCategories
          .AsNoTracking()
          .Where(Q => Q.ProductTipCategoryId == ProductTipCategoryId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductTipCategoryName != productTipCategoryName;

      var isExist = await this.ValidateProductTipCategoryByNameAsync(productTipCategoryName);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductTipCategoryAsync(DeleteProductTipCategoryModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.ProductTipCategories.Where(Q => Q.ProductTipCategoryId == model.ProductTipCategoryId).FirstOrDefaultAsync();

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
