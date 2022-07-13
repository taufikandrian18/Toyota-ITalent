using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductFeatureCategories")]
    public class ProductFeatureCategoryApiController : Controller
    {
        private readonly ProductFeatureCategoryService ProductFeatureCategoryMan;

        public ProductFeatureCategoryApiController(ProductFeatureCategoryService productFeatureCategoryService)
        {
            this.ProductFeatureCategoryMan = productFeatureCategoryService;
        }

        [HttpGet("get-all-product-feature-categories")]
        public async Task<ActionResult<ProductFeatureCategoryPaginate>> GetAllProductFeatureCategoryAsync([FromQuery] ProductFeatureCategoryGridFilter filter)
        {
            var data = await this.ProductFeatureCategoryMan.GetAllProductFeatureCategory(filter);
            return Ok(data);
        }
        [HttpPost("insert-product-feature-category", Name = "InsertProductFeatureCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductFeatureAsync([FromBody] ProductFeatureCategoryCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductFeatureCategoryMan.InsertProductFeatureCategoryAsync(model);

            return success;
        }
        [HttpGet("get-product-feature-category-by-id", Name = "GetProductFeatureCategoryByIdAsync")]
        public async Task<ActionResult<ProductFeatureCategoryModel>> GetProductFeatureCategoryByIdAsync([FromQuery] Guid productFeatureCategoryId)
        {
            var data = await this.ProductFeatureCategoryMan.GetProductFeatureCategoryById(productFeatureCategoryId);

            return Ok(data);
        }
        [HttpPost("update-product-feature-category-by-id", Name = "UpdateProductFeatureCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductFeatureCategoryAsync([FromBody] ProductFeatureCategoryUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductFeatureCategoryMan.UpdateProductFeatureCategoryAsync(model);

            return success;
        }
        [HttpDelete("delete-product-feature-category", Name = "DeleteProductFeatureCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductFeatureCategoryAsync([FromBody] DeleteProductFeatureCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductFeatureCategoryMan.DeleteProductFeatureCategoryAsync(model);

            return getData;
        }
    }
}
