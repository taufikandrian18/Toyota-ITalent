using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/Product-Competitor-Mappings")]
    public class ProductCompetitorMappingApiController:Controller
    {
        private readonly ProductCompetitorMappingService ProductCompetitorMan;

        public ProductCompetitorMappingApiController(ProductCompetitorMappingService productCompetitorService)
        {
            this.ProductCompetitorMan = productCompetitorService;
        }
        [HttpGet("get-all-product-competitor-mappings")]
        public async Task<ActionResult<ProductCompetitorMappingPaginate>> GetAllProductCompetitorMappingAsync([FromQuery] ProductCompetitorMappingGridFilter filter)
        {
            var data = await this.ProductCompetitorMan.GetAllProductCompetitor(filter);
            return Ok(data);
        }
        [HttpPost("insert-product-competitor-mapping", Name = "InsertProductCompetitorMappingAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductCompetitorMappingAsync([FromBody] ProductCompetitorMappingCreateUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductCompetitorMan.InsertProductCompetitorAsync(model);

            return success;
        }
        [HttpGet("get-product-competitor-mapping-by-id", Name = "GetProductCompetitorMappingByIdAsync")]
        public async Task<ActionResult<ProductCompetitorMappingModel>> GetProductCompetitorMappingByIdAsync([FromQuery] Guid productCompetitorMappingId)
        {
            var data = await this.ProductCompetitorMan.GetProductCompetitorById(productCompetitorMappingId);

            return Ok(data);
        }
        [HttpPost("update-product-competitor-mapping", Name = "UpdateProductCompetitorMappingAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductCompetitorMappingAsync([FromBody] ProductCompetitorMappingUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductCompetitorMan.UpdateProductCompetitor(model);

            return success;
        }
        [HttpDelete("delete-product-competitor-mapping", Name = "DeleteProductCompetitorMappingAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductCompetitorMappingAsync([FromBody] DeleteProductCompetitorModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductCompetitorMan.DeleteProductCompetitorAsync(model);

            return getData;
        }
    }
}
