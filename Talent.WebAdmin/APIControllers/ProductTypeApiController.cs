using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductTypes")]
    public class ProductTypeApiController : Controller
    {
        private readonly ProductTypeService ProductTypeMan;

        public ProductTypeApiController(ProductTypeService productTypeService)
        {
            this.ProductTypeMan = productTypeService;
        }

        [HttpGet("get-all-product-types")]
        public async Task<ActionResult<ProductTypePaginate>> GetAllProductTypeAsync([FromQuery] ProductTypeGridFilter filter)
        {
            var data = await this.ProductTypeMan.GetAllProductType(filter);
            return Ok(data);
        }

        [HttpGet("get-all-product-type-by-product")]
        public async Task<ActionResult<List<ProductTypeDropdownView>>> GetAllProductTypeByProduct([FromQuery] Guid productId)
        {
            var data = await this.ProductTypeMan.GetAllProductTypeByProduct(productId);
            return Ok(data);
        }

        [HttpPost("insert-product-type", Name = "InsertProductTypeAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductTypeAsync([FromBody] ProductTypeCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductTypeMan.InsertProductTypeAsync(model);

            return success;
        }
        [HttpGet("get-product-type-by-id", Name = "GetProductTypeByIdAsync")]
        public async Task<ActionResult<ProductTypeModel>> GetProductTypeByIdAsync([FromQuery] Guid ProductTypeId)
        {
            var data = await this.ProductTypeMan.GetProductTypeById(ProductTypeId);

            return Ok(data);
        }
        [HttpPost("update-product-type-by-id", Name = "UpdateProductTypeAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductTypeAsync([FromBody] ProductTypeUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductTypeMan.UpdateProductTypeAsync(model);

            return success;
        }
        [HttpDelete("delete-product-type", Name = "DeleteProductTypeAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductTypeAsync([FromBody] DeleteProductTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductTypeMan.DeleteProductTypeAsync(model);

            return getData;
        }
    }
}
