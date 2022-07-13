using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductFeatures")]
    public class ProductFeatureApiController : Controller
    {
        private readonly ProductFeatureService ProductFeatureMan;

        public ProductFeatureApiController(ProductFeatureService productFeatureService)
        {
            this.ProductFeatureMan = productFeatureService;
        }

        [HttpGet("get-all-product-features")]
        public async Task<ActionResult<ProductFeaturePaginate>> GetAllProductFeatureAsync([FromQuery] ProductFeatureGridFilter filter)
        {
            var data = await this.ProductFeatureMan.GetAllProductFeature(filter);
            return Ok(data);
        }
        [HttpPost("insert-product-feature", Name = "InsertProductFeatureAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductFeatureAsync([FromBody] ProductFeatureCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductFeatureMan.InsertProductFeatureAsync(model);

            return success;
        }
        [HttpGet("get-product-feature-by-id", Name = "GetProductFeatureByIdAsync")]
        public async Task<ActionResult<ProductFeatureModel>> GetProductFeatureByIdAsync([FromQuery] Guid productFeatureId)
        {
            var data = await this.ProductFeatureMan.GetProductFeatureById(productFeatureId);

            return Ok(data);
        }
        [HttpPost("update-product-feature-by-id", Name = "UpdateProductFeatureAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductFeatureAsync([FromBody] ProductFeatureUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductFeatureMan.UpdateProductFeatureAsync(model);

            return success;
        }
        [HttpPost("update-product-feature-by-approval", Name = "UpdateProductFeatureApprovalAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductFeatureByApprovalAsync([FromBody] UpdateProductFeatureApprovedModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductFeatureMan.UpdateProductFeatureByApprovalAsync(model);

            return getData;
        }
        [HttpDelete("delete-product-feature", Name = "DeleteProductFeatureAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductFeatureAsync([FromBody] DeleteProductFeatureModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductFeatureMan.DeleteProductFeatureAsync(model);

            return getData;
        }
    }
}
