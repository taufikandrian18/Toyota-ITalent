using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductSPWAMappings")]
    public class ProductSPWAMappingApiController : Controller
    {
        private readonly ProductSPWAMappingService ProductSPWAMappingMan;

        public ProductSPWAMappingApiController(ProductSPWAMappingService productSPWAMappingService)
        {
            this.ProductSPWAMappingMan = productSPWAMappingService;
        }

        [HttpGet("get-all-product-spwa-mappings")]
        public async Task<ActionResult<ProductSPWAMappingPaginate>> GetAllProductSPWAMappingAsync([FromQuery] ProductSPWAMappingGridFilter filter)
        {
            var data = await this.ProductSPWAMappingMan.GetAllProductSPWAMapping(filter);
            return Ok(data);
        }
        [HttpPost("insert-product-spwa-mappings", Name = "InsertProductSPWAMappingAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductSPWAMappingAsync([FromBody] ProductSPWAMappingCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductSPWAMappingMan.InsertProductSPWAMappingAsync(model);

            return success;
        }
        [HttpGet("get-product-spwa-mapping-by-id", Name = "GetProductSPWAMappingByIdAsync")]
        public async Task<ActionResult<ProductSPWAMappingModel>> GetProductSPWAMappingByIdAsync([FromQuery] Guid ProductSPWAMappingId)
        {
            var data = await this.ProductSPWAMappingMan.GetProductSPWAMappingById(ProductSPWAMappingId);

            return Ok(data);
        }
        [HttpPost("update-product-spwa-mapping-by-id", Name = "UpdateProductSPWAMappingAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductSPWAMappingAsync([FromBody] ProductSPWAMappingUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductSPWAMappingMan.UpdateProductSPWAMapping(model);

            return success;
        }
        [HttpDelete("delete-product-spwa-mapping", Name = "DeleteProductSPWAMappingAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductSPWAMappingAsync([FromBody] DeleteProductSPWAMappingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductSPWAMappingMan.DeleteProductSPWAMappingAsync(model);

            return getData;
        }
    }
}
