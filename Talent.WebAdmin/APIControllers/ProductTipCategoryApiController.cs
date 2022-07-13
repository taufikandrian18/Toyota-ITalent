using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductTipCategories")]
    public class ProductTipCategoryApiController : Controller
    {
        private readonly ProductTipCategoryService ProductTipCategoryMan;

        public ProductTipCategoryApiController(ProductTipCategoryService productTipCategory)
        {
            this.ProductTipCategoryMan = productTipCategory;
        }

        [HttpGet("get-all-product-tip-category")]
        public async Task<ActionResult<ProductTipCategoryPaginate>> GetAllProductTipCategoryAsync([FromQuery] ProductTipCategoryGridFilter filter)
        {
            var data = await this.ProductTipCategoryMan.GetAllProductTipCategory(filter);
            return Ok(data);
        }
        [HttpPost("insert-product-tip-category", Name = "InsertProductTipCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductTipCategoryAsync([FromBody] ProductTipCategoryCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductTipCategoryMan.InsertProductTipCategoryAsync(model);

            return success;
        }
        [HttpGet("get-product-tip-category-by-id", Name = "GetProductTipCategoryByIdAsync")]
        public async Task<ActionResult<ProductTipCategoryModel>> GetProductTipCategoryByIdAsync([FromQuery] Guid ProductTipCategoryId)
        {
            var data = await this.ProductTipCategoryMan.GetProductTipCategoryById(ProductTipCategoryId);

            return Ok(data);
        }
        [HttpPost("update-product-tip-category-by-id", Name = "UpdateProductTipCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductTipCategoryAsync([FromBody] ProductTipCategoryUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductTipCategoryMan.UpdateProductTipCategoryAsync(model);

            return success;
        }
        [HttpDelete("delete-product-tip-category", Name = "DeleteProductTipCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductTipCategoryAsync([FromBody] DeleteProductTipCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductTipCategoryMan.DeleteProductTipCategoryAsync(model);

            return getData;
        }
    }
}
