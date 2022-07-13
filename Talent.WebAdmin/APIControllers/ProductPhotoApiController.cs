using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductPhotos")]
    public class ProductPhotoApiController : Controller
    {
        private readonly ProductPhotoService ProductPhotoMan;

        public ProductPhotoApiController(ProductPhotoService productPhotoService)
        {
            this.ProductPhotoMan = productPhotoService;
        }

        [HttpGet("get-all-product-photos")]
        public async Task<ActionResult<ProductPhotoPaginate>> GetAllProductPhotoAsync([FromQuery] ProductPhotoGridFilter filter)
        {
            var data = await this.ProductPhotoMan.GetAllProductPhoto(filter);
            return Ok(data);
        }

        [HttpPost("insert-product-photo", Name = "InsertProductPhotoAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductPhotoAsync([FromBody] ProductPhotoCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductPhotoMan.InsertProductPhoto(model);

            return success;
        }
        [HttpPost("update-product-photo-by-id", Name = "UpdateProductPhotoAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductPhotoAsync([FromBody] ProductPhotoUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductPhotoMan.UpdateProductPhoto(model);

            return success;
        }

        [HttpGet("get-product-photo-by-id", Name = "GetProductPhotoByIdAsync")]
        public async Task<ActionResult<ProductGalleryModel>> GetProductPhotoByIdAsync([FromQuery] Guid ProductPhotoId)
        {
            var data = await this.ProductPhotoMan.GetProductPhotoById(ProductPhotoId);

            return Ok(data);
        }

        [HttpDelete("delete-product-photo")]
        public async Task<ActionResult<BaseResponse>> DeleteProductPhotoByIdAsync([FromBody] DeleteProductPhotoModel model)
        {
            var getData = await this.ProductPhotoMan.DeleteProductPhoto(model);

            return getData;
        }
    }
}
