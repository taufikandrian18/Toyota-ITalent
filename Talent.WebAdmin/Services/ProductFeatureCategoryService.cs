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
    public class ProductFeatureCategoryService : Controller
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper HelperMan;
        private readonly BlobService BlobService;

        public ProductFeatureCategoryService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bm)
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
        public async Task<ProductFeatureCategoryPaginate> GetAllProductFeatureCategory(ProductFeatureCategoryGridFilter filter)
        {

            var query = (from sl in this.DB.ProductFeatureCategories
                         where sl.IsDeleted == false
                         select new ProductFeatureCategoryModel
                         {
                             ProductFeatureCategoryId = sl.ProductFeatureCategoryId,
                             ProductFeatureCategoryName = sl.ProductFeatureCategoryName
                         }).AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(filter.ProductFeatureCategoryName))
            {
                query = query.Where(Q => Q.ProductFeatureCategoryName == filter.ProductFeatureCategoryName);
            }

            //if (filter.ProductId != Guid.Empty)
            //{
            //  query = query.Where(Q => Q.ProductId == filter.ProductId);
            //}

            //sort
            switch (filter.SortBy)
            {
                case "ProductFeatureCategoryNameAsc":
                    query = query.OrderBy(Q => Q.ProductFeatureCategoryName);
                    break;
                case "ProductFeatureCategoryNameDesc":
                    query = query.OrderByDescending(Q => Q.ProductFeatureCategoryName);
                    break;
                default:
                    query = query.OrderBy(Q => Q.ProductFeatureCategoryName);
                    break;
            }
            var productFeatureCategories = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalData = await query.CountAsync();

            return new ProductFeatureCategoryPaginate
            {
                ProductFeatureCategories = productFeatureCategories,
                TotalProductFeatureCategories = totalData
            };
        }
        public async Task<ActionResult<BaseResponse>> InsertProductFeatureCategoryAsync(ProductFeatureCategoryCreateModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var isExist = await ValidateProductFeatureCategoryByNameAsync(model.ProductFeatureCategoryName);

                if (isExist)
                {
                    return BadRequest(BaseResponse.BadRequest(model));
                }

                var newProductFeatureCategories = new ProductFeatureCategories
                {
                    ProductFeatureCategoryName = model.ProductFeatureCategoryName,
                    CreatedAt = thisDate,
                    UpdatedAt = thisDate,
                    IsDeleted = false,
                    CreatedBy = this.HelperMan.GetLoginUserId(),
                    UpdatedBy = this.HelperMan.GetLoginUserId()
                };

                this.DB.ProductFeatureCategories.Add(newProductFeatureCategories);

                await this.DB.SaveChangesAsync();
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
        public async Task<bool> ValidateProductFeatureCategoryByNameAsync(string productFeatureCategoryName)
        {
            var isExist = await this
                .DB
                .ProductFeatureCategories
                .AsNoTracking()
                .Where(Q => Q.IsDeleted == false)
                .AnyAsync(Q => Q.ProductFeatureCategoryName == productFeatureCategoryName);

            return isExist;
        }
        public async Task<ProductFeatureCategoryModel> GetProductFeatureCategoryById(Guid productFeatureCategoryId)
        {
            var data = await (from mo in DB.ProductFeatureCategories
                              where mo.ProductFeatureCategoryId == productFeatureCategoryId && mo.IsDeleted == false
                              select new ProductFeatureCategoryModel
                              {
                                  ProductFeatureCategoryId = mo.ProductFeatureCategoryId,
                                  ProductFeatureCategoryName = mo.ProductFeatureCategoryName
                              }).FirstOrDefaultAsync();

            return data;
        }
        public async Task<ActionResult<BaseResponse>> UpdateProductFeatureCategoryAsync(ProductFeatureCategoryUpdateModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var isChangedAndExist = await ValidateUpdateProductFeatureCategoryNameAsync(model.ProductFeatureCategoryName, model.ProductFeatureCategoryId);

                if (isChangedAndExist)
                {
                    return BadRequest(BaseResponse.BadRequest(model));
                }

                var productFeatureCategoryToUpdate = await this.DB.ProductFeatureCategories.Where(Q => Q.ProductFeatureCategoryId == model.ProductFeatureCategoryId).FirstOrDefaultAsync();

                productFeatureCategoryToUpdate.ProductFeatureCategoryId = model.ProductFeatureCategoryId;
                productFeatureCategoryToUpdate.ProductFeatureCategoryName = model.ProductFeatureCategoryName;
                productFeatureCategoryToUpdate.UpdatedAt = thisDate;
                productFeatureCategoryToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

                await this.DB.SaveChangesAsync();
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
        public async Task<bool> ValidateUpdateProductFeatureCategoryNameAsync(string productFeatureCategoryName, Guid productFeatureCategoryId)
        {
            var data = await this
                .DB
                .ProductFeatureCategories
                .AsNoTracking()
                .Where(Q => Q.ProductFeatureCategoryId == productFeatureCategoryId)
                .FirstOrDefaultAsync();

            var isChange = data.ProductFeatureCategoryName != productFeatureCategoryName;

            var isExist = await this.ValidateProductFeatureCategoryByNameAsync(productFeatureCategoryName);

            var isTrue = isChange == true && isExist == true;

            return isTrue;
        }
        public async Task<ActionResult<BaseResponse>> DeleteProductFeatureCategoryAsync(DeleteProductFeatureCategoryModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var module = await this.DB.ProductFeatureCategories.Where(Q => Q.ProductFeatureCategoryId == model.ProductFeatureCategoryId).FirstOrDefaultAsync();

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
