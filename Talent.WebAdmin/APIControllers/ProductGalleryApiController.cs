using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductGalleries")]
    public class ProductGalleryApiController : Controller
    {
        private readonly ProductGalleryService ProductGalleryMan;

        public ProductGalleryApiController(ProductGalleryService productGalleryService)
        {
            this.ProductGalleryMan = productGalleryService;
        }

        [HttpGet("get-all-product-gallery-contributors")]
        public async Task<ActionResult<ProductGalleryPaginate>> GetAllProductGalleryContributorsAsync([FromQuery] ProductGalleryGridFilter filter)
        {
            var data = await this.ProductGalleryMan.GetAllProductGalleryContributor(filter);
            return Ok(data);
        }

        [HttpGet("get-all-product-galleries")]
        public async Task<ActionResult<ProductGalleryPaginate>> GetAllProductGalleryAsync([FromQuery] ProductGalleryGridFilter filter)
        {
            var data = await this.ProductGalleryMan.GetAllProductGallery(filter);
            return Ok(data);
        }

        [HttpPost("insert-product-gallery", Name = "InsertProductGalleryAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductGalleryAsync([FromBody] ProductGalleryCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductGalleryMan.InsertProductGalleryAsync(model);

            return success;
        }

        [HttpGet("get-product-gallery-by-id", Name = "GetProductGalleryByIdAsync")]
        public async Task<ActionResult<ProductGalleryModel>> GetProductGalleryByIdAsync([FromQuery] Guid ProductGalleryId)
        {
            var data = await this.ProductGalleryMan.GetProductGalleryById(ProductGalleryId);

            return Ok(data);
        }

        [HttpPost("update-product-gallery-by-id", Name = "UpdateProductGalleryAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductGalleryAsync([FromBody] ProductGalleryUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductGalleryMan.UpdateProductGalleryAsync(model);

            return success;
        }

        [HttpDelete("delete-product-gallery", Name = "DeleteProductGalleryAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductGalleryAsync([FromBody] DeleteProductGalleryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductGalleryMan.DeleteProductGalleryAsync(model);

            return getData;
        }

        [HttpPost("update-product-gallery-by-approval", Name = "UpdateProductGalleryApprovalAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductGalleryByApprovalAsync([FromBody] UpdateProductGalleryApproval model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductGalleryMan.UpdateProductGalleryByApprovalAsync(model);

            return getData;
        }
    }
}
