using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductProgramCategories")]
    public class ProductProgramCategoryApiController : Controller
    {
        private readonly ProductProgramCategoryService ProductProgramCategoryMan;

        public ProductProgramCategoryApiController(ProductProgramCategoryService productProgramCategory)
        {
            this.ProductProgramCategoryMan = productProgramCategory;
        }

        [HttpGet("get-all-product-program-category")]
        public async Task<ActionResult<ProductProgramCategoryPaginate>> GetAllProductProgramCategoryAsync([FromQuery] ProductProgramCategoryGridFilter filter)
        {
            var data = await this.ProductProgramCategoryMan.GetAllProductProgramCategory(filter);
            return Ok(data);
        }
        [HttpPost("insert-product-program-category", Name = "InsertProductProgramCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductProgramCategoryAsync([FromBody] ProductProgramCategoryCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductProgramCategoryMan.InsertProductProgramCategoryAsync(model);

            return success;
        }
        [HttpGet("get-product-program-category-by-id", Name = "GetProductProgramCategoryByIdAsync")]
        public async Task<ActionResult<ProductProgramCategoryModel>> GetProductProgramCategoryByIdAsync([FromQuery] Guid ProductProgramCategoryId)
        {
            var data = await this.ProductProgramCategoryMan.GetProductProgramCategoryById(ProductProgramCategoryId);

            return Ok(data);
        }
        [HttpPost("update-product-program-category-by-id", Name = "UpdateProductProgramCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductProgramCategoryAsync([FromBody] ProductProgramCategoryUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductProgramCategoryMan.UpdateProductProgramCategoryAsync(model);

            return success;
        }
        [HttpDelete("delete-product-program-category", Name = "DeleteProductProgramCategoryAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductProgramCategoryAsync([FromBody] DeleteProductProgramCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductProgramCategoryMan.DeleteProductProgramCategoryAsync(model);

            return getData;
        }
    }
}
