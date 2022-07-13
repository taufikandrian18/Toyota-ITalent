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
  public class ProductCustomerTypeService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly ClaimHelper HelperMan;

    public ProductCustomerTypeService(TalentContext db, IContextualService contextual, ClaimHelper hm)
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

    public async Task<ProductCustomerTypePaginate> GetAllProductCustomerType(ProductCustomerTypeGridFilter filter)
    {

      var query = (from sl in this.DB.ProductCustomerTypes
                   where sl.IsDeleted == false
                   select new ProductCustomerTypeModel
                   {
                     ProductCustomerTypeId = sl.ProductCustomerTypeId,
                     ProductCustomerTypeName = sl.ProductCustomerTypeName,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.ProductCustomerTypeName))
      {
        query = query.Where(Q => Q.ProductCustomerTypeName.Contains(filter.ProductCustomerTypeName));
      }

      //sort
      switch (filter.SortBy)
      {
        case "ProductCustomerTypeNameAsc":
          query = query.OrderBy(Q => Q.ProductCustomerTypeName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "ProductCustomerTypeNameDesc":
          query = query.OrderByDescending(Q => Q.ProductCustomerTypeName);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.ProductCustomerTypeName);
          break;
      }
      var productCustomerTypes = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new ProductCustomerTypePaginate
      {
        ProductCustomerTypes = productCustomerTypes,
        TotalProductCustomerTypes = totalData
      };
    }

    public async Task<ActionResult<BaseResponse>> InsertProductCustomerTypeAsync(ProductCustomerTypeCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await ValidateProductCustomerTypeByNameAsync(model.ProductCustomerTypeName);

            if (isExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var newProductCustomerTypes = new ProductCustomerTypes
            {
                ProductCustomerTypeName = model.ProductCustomerTypeName,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.ProductCustomerTypes.Add(newProductCustomerTypes);

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateProductCustomerTypeByNameAsync(string productCustomerTypeName)
    {
      var isExist = await this
          .DB
          .ProductCustomerTypes
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ProductCustomerTypeName == productCustomerTypeName);

      return isExist;
    }
    public async Task<ProductCustomerTypeModel> GetProductCustomerTypeById(Guid productCustomerTypeId)
    {
      var data = await (from mo in DB.ProductCustomerTypes
                        where mo.ProductCustomerTypeId == productCustomerTypeId && mo.IsDeleted == false
                        select new ProductCustomerTypeModel
                        {
                          ProductCustomerTypeId = mo.ProductCustomerTypeId,
                          ProductCustomerTypeName = mo.ProductCustomerTypeName,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedAt = mo.UpdatedAt,
                          UpdatedBy = mo.UpdatedBy
                        }).FirstOrDefaultAsync();

      return data;
    }

    public async Task<ActionResult<BaseResponse>> UpdateProductCustomerTypeAsync(ProductCustomerTypeUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateProductCustomerTypeNameAsync(model.ProductCustomerTypeName, model.ProductCustomerTypeId);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var productCustomerTypeToUpdate = await this.DB.ProductCustomerTypes.Where(Q => Q.ProductCustomerTypeId == model.ProductCustomerTypeId).FirstOrDefaultAsync();

            productCustomerTypeToUpdate.ProductCustomerTypeId = model.ProductCustomerTypeId;
            productCustomerTypeToUpdate.ProductCustomerTypeName = model.ProductCustomerTypeName;
            productCustomerTypeToUpdate.UpdatedAt = thisDate;
            productCustomerTypeToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }

    public async Task<bool> ValidateUpdateProductCustomerTypeNameAsync(string productCustomerTypeName, Guid productCustomerTypeId)
    {
      var data = await this
          .DB
          .ProductCustomerTypes
          .AsNoTracking()
          .Where(Q => Q.ProductCustomerTypeId == productCustomerTypeId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductCustomerTypeName != productCustomerTypeName;

      var isExist = await this.ValidateProductCustomerTypeByNameAsync(productCustomerTypeName);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }

    public async Task<ActionResult<BaseResponse>> DeleteProductCustomerTypeAsync(DeleteProductCustomerTypeModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.ProductCustomerTypes.Where(Q => Q.ProductCustomerTypeId == model.ProductCustomerTypeId).FirstOrDefaultAsync();

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
