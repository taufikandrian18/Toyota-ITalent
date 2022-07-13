using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductFeatureMappings")]
    public class ProductFeatureMappingApiController : Controller
    {
        private readonly ProductFeatureMappingService ProductFeatureMappingMan;

        public ProductFeatureMappingApiController(ProductFeatureMappingService productFeatureMappingService)
        {
            this.ProductFeatureMappingMan = productFeatureMappingService;
        }

        [HttpGet("get-all-product-feature-mappings")]
        public async Task<ActionResult<ProductFeatureMappingPaginate>> GetAllProductFeatureMappingAsync([FromQuery] ProductFeatureMappingGridFilter filter)
        {
            var data = await this.ProductFeatureMappingMan.GetAllProductFeatureMapping(filter);
            return Ok(data);
        }
        [HttpPost("insert-product-feature-mappings", Name = "InsertProductFeatureMappingAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductFeatureMappingAsync([FromBody] ProductFeatureMappingCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductFeatureMappingMan.InsertProductFeatureMappingAsync(model);

            return success;
        }
        [HttpGet("get-product-feature-mapping-by-id", Name = "GetProductFeatureMappingByIdAsync")]
        public async Task<ActionResult<ProductFeatureMappingModel>> GetProductFeatureMappingByIdAsync([FromQuery] Guid ProductFeatureMappingId)
        {
            var data = await this.ProductFeatureMappingMan.GetProductFeatureMappingById(ProductFeatureMappingId);

            return Ok(data);
        }
        [HttpPost("update-product-feature-mapping-by-id", Name = "UpdateProductFeatureMappingAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductFeatureMappingAsync([FromBody] ProductFeatureMappingUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductFeatureMappingMan.UpdateProductFeatureMapping(model);

            return success;
        }
        [HttpDelete("delete-product-feature-mapping", Name = "DeleteProductFeatureMappingAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductFeatureMappingAsync([FromBody] DeleteProductFeatureMappingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductFeatureMappingMan.DeleteProductFeatureMappingAsync(model);

            return getData;

        }
        [HttpPost("update-approval-product-feature-mapping", Name = "UpdateApprovalProductFeatureMappingAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateApprovalProductFeatureMappingAsync([FromBody] ProductFeatureMappingUpdateApprovalModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductFeatureMappingMan.UpdateApprovalProductFeatureMappingAsync(model);

            return getData;

        }
    }
}
