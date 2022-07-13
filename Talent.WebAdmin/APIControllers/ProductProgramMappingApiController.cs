using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductProgramMappings")]
    public class ProductProgramMappingApiController : Controller
    {
        private readonly ProductProgramMappingService ProductProgramMappingMan;

        public ProductProgramMappingApiController(ProductProgramMappingService productProgramMappingService)
        {
            this.ProductProgramMappingMan = productProgramMappingService;
        }

        [HttpGet("get-all-product-program-mappings")]
        public async Task<ActionResult<ProductProgramMappingPaginate>> GetAllProductProgramMappingAsync([FromQuery] ProductProgramMappingGridFilter filter)
        {
            var data = await this.ProductProgramMappingMan.GetAllProductProgramMapping(filter);
            return Ok(data);
        }
        [HttpPost("insert-product-program-mappings", Name = "InsertProductProgramMappingAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductProgramMappingAsync([FromBody] ProductProgramMappingCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductProgramMappingMan.InsertProductProgramMappingAsync(model);

            return success;
        }
        [HttpGet("get-product-program-mapping-by-id", Name = "GetProductProgramMappingByIdAsync")]
        public async Task<ActionResult<ProductProgramMappingModel>> GetProductProgramMappingByIdAsync([FromQuery] Guid ProductProgramMappingId)
        {
            var data = await this.ProductProgramMappingMan.GetProductProgramMappingById(ProductProgramMappingId);

            return Ok(data);
        }
        [HttpPost("update-product-program-mapping-by-id", Name = "UpdateProductProgramMappingAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductProgramMappingAsync([FromBody] ProductProgramMappingUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductProgramMappingMan.UpdateProductProgramMapping(model);

            return success;
        }
        [HttpDelete("delete-product-program-mapping", Name = "DeleteProductProgramMappingAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductProgramMappingAsync([FromBody] DeleteProductProgramMappingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductProgramMappingMan.DeleteProductProgramMappingAsync(model);

            return getData;
        }
    }
}
