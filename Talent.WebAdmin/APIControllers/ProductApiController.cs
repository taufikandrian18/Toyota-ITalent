using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/Products")]
    public class ProductApiController : Controller
    {
        private readonly ProductService ProductMan;
        public ProductApiController(ProductService productService)
        {
            this.ProductMan = productService;
        }
        [HttpGet("get-all-products")]
        public async Task<ActionResult<ProductPaginate>> GetAllProductAsync([FromQuery] ProductGridFilter filter)
        {
            var data = await this.ProductMan.GetAllProduct(filter);
            return Ok(data);
        }
        [HttpGet("get-all-competitor-products")]
        public async Task<ActionResult<List<ProductIsCompetitorDropdownView>>> GetAllCompetitorProductAsync()
        {
            var data = await this.ProductMan.GetProductIsCompetitor();
            return Ok(data);
        }
        [HttpPost("insert-product", Name = "InsertProductAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductAsync([FromBody] ProductCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductMan.InsertProductAsync(model);

            return success;
        }
        [HttpGet("get-product-by-id", Name = "GetProductByIdAsync")]
        public async Task<ActionResult<ProductModel>> GetProductByIdAsync([FromQuery] Guid ProductId)
        {
            var data = await this.ProductMan.GetProductById(ProductId);

            return Ok(data);
        }
        [HttpPost("update-product-by-id", Name = "UpdateProductAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductAsync([FromBody] ProductUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductMan.UpdateProductAsync(model);

            return success;
        }
        [HttpDelete("delete-product", Name = "DeleteProductAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductAsync([FromBody] DeleteProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductMan.DeleteProductAsync(model);

            return getData;
        }
    }
}
