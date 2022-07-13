using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductSPWACategories")]
    public class ProductSPWACategoryApiController : Controller
    {
        private readonly ProductSPWACategoryService ProductSPWACategoryMan;

        public ProductSPWACategoryApiController(ProductSPWACategoryService productSPWACategoryService)
        {
            this.ProductSPWACategoryMan = productSPWACategoryService;
        }

        [HttpGet("get-all-product-spwa-category")]
        public async Task<ActionResult<ProductSPWACategoryPaginate>> GetAllProductSPWACategoryAsync([FromQuery] ProductSPWACategoryGridFilter filter)
        {
            var data = await this.ProductSPWACategoryMan.GetAllProductSPWACategory(filter);
            return Ok(data);
        }
        [HttpPost("insert-product-spwa-category", Name = "InsertProductSPWACategoryAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductSPWACategoryAsync([FromBody] ProductSPWACategoryCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductSPWACategoryMan.InsertProductSPWACategoryAsync(model);

            return success;
        }
        [HttpGet("get-product-spwa-category-by-id", Name = "GetProductSPWACategoryByIdAsync")]
        public async Task<ActionResult<ProductSPWACategoryModel>> GetProductSPWACategoryByIdAsync([FromQuery] Guid ProductSPWACategoryId)
        {
            var data = await this.ProductSPWACategoryMan.GetProductSPWACategoryById(ProductSPWACategoryId);

            return Ok(data);
        }
        [HttpPost("update-product-spwa-category-by-id", Name = "UpdateProductSPWACategoryAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductSPWACategoryAsync([FromBody] ProductSPWACategoryUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductSPWACategoryMan.UpdateProductSPWACategoryAsync(model);

            return success;
        }
        [HttpDelete("delete-product-spwa-category", Name = "DeleteProductSPWACategoryAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductSPWACategoryAsync([FromBody] DeleteProductSPWACategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductSPWACategoryMan.DeleteProductSPWACategoryAsync(model);

            return getData;
        }
    }
}
