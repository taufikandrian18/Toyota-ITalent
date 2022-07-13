using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductFAQCategories")]
    public class ProductFAQCategoryApiController : Controller
    {
        private readonly ProductFAQCategoryService ProductFAQCategoryMan;

        public ProductFAQCategoryApiController(ProductFAQCategoryService productFAQCategory)
        {
            this.ProductFAQCategoryMan = productFAQCategory;
        }

        [HttpGet("get-all-product-faq-category")]
        public async Task<ActionResult<ProductFAQCategoryPaginate>> GetAllProductFAQCategoryAsync([FromQuery] ProductFAQCategoryGridFilter filter)
        {
            var data = await this.ProductFAQCategoryMan.GetAllProductFAQCategory(filter);
            return Ok(data);
        }
        [HttpPost("insert-product-faq-category", Name = "InsertProductFAQCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductFAQCategoryAsync([FromBody] ProductFAQCategoryCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductFAQCategoryMan.InsertProductFAQCategoryAsync(model);

            return success;
        }
        [HttpGet("get-product-faq-category-by-id", Name = "GetProductFAQCategoryByIdAsync")]
        public async Task<ActionResult<ProductFAQCategoryModel>> GetProductFAQCategoryByIdAsync([FromQuery] Guid ProductFaqCategoryId)
        {
            var data = await this.ProductFAQCategoryMan.GetProductFAQCategoryById(ProductFaqCategoryId);

            return Ok(data);
        }
        [HttpPost("update-product-faq-category-by-id", Name = "UpdateProductFAQCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductFAQCategoryAsync([FromBody] ProductFAQCategoryUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductFAQCategoryMan.UpdateProductFAQCategoryAsync(model);

            return success;
        }
        [HttpDelete("delete-product-faq-category", Name = "DeleteProductFAQCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductFAQCategoryAsync([FromBody] DeleteProductFAQCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductFAQCategoryMan.DeleteProductFAQCategoryAsync(model);

            return getData;
        }
    }
}
