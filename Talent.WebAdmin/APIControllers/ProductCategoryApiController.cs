using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductCategories")]
    public class ProductCategoryApiController : Controller
    {
        private readonly ProductCategoryService ProductCategoryMan;

        public ProductCategoryApiController(ProductCategoryService productCategoryService)
        {
            this.ProductCategoryMan = productCategoryService;
        }

        [HttpGet("get-all-productCategories")]
        public async Task<ActionResult<ProductCategoryPaginate>> GetAllProductCategoryAsync([FromQuery] ProductCategoryGridFilter filter)
        {
            var data = await this.ProductCategoryMan.GetAllProductCategory(filter);
            return Ok(data);
        }
        [HttpPost("insert-product-category", Name = "InsertProductCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductCategoryAsync([FromBody] ProductCategoryCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductCategoryMan.InsertProductCategoryAsync(model);

            return success;
        }
        [HttpGet("get-product-category-by-id", Name = "GetProductCategoryByIdAsync")]
        public async Task<ActionResult<ProductCategoryModel>> GetProductCategoryByIdAsync([FromQuery] Guid ProductCategoryId)
        {
            var data = await this.ProductCategoryMan.GetProductCategoryById(ProductCategoryId);

            return Ok(data);
        }
        [HttpPost("update-product-category-by-id", Name = "UpdateProductCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductCategoryAsync([FromBody] ProductCategoryUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductCategoryMan.UpdateProductCategoryAsync(model);

            return success;
        }
        [HttpDelete("delete-product-category", Name = "DeleteProductCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductCategoryAsync([FromBody] DeleteProductCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductCategoryMan.DeleteProductCategoryAsync(model);

            return getData;
        }
    }
}
