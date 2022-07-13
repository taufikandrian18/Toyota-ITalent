using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductCustomerTypes")]
    public class ProductCustomerTypeApiController : Controller
    {
        private readonly ProductCustomerTypeService ProductCustomerTypeMan;

        public ProductCustomerTypeApiController(ProductCustomerTypeService productCustomerTypeService)
        {
            this.ProductCustomerTypeMan = productCustomerTypeService;
        }

        [HttpGet("get-all-product-customer-types")]
        public async Task<ActionResult<ProductCustomerTypePaginate>> GetAllProductCustomerTypeAsync([FromQuery] ProductCustomerTypeGridFilter filter)
        {
            var data = await this.ProductCustomerTypeMan.GetAllProductCustomerType(filter);
            return Ok(data);
        }
        [HttpPost("insert-product-customer-type", Name = "InsertProductCustomerTypeAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductCustomerTypeAsync([FromBody] ProductCustomerTypeCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductCustomerTypeMan.InsertProductCustomerTypeAsync(model);

            return success;
        }
        [HttpGet("get-product-customer-type-by-id", Name = "GetProductCustomerTypeByIdAsync")]
        public async Task<ActionResult<ProductCustomerTypeModel>> GetProductCustomerTypeByIdAsync([FromQuery] Guid ProductCustomerTypeId)
        {
            var data = await this.ProductCustomerTypeMan.GetProductCustomerTypeById(ProductCustomerTypeId);

            return Ok(data);
        }
        [HttpPost("update-product-customer-type-by-id", Name = "UpdateProductCustomerTypeAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductCustomerTypeAsync([FromBody] ProductCustomerTypeUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductCustomerTypeMan.UpdateProductCustomerTypeAsync(model);

            return success;
        }
        [HttpDelete("delete-product-customer-type", Name = "DeleteProductCustomerTypeAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductCustomerTypeAsync([FromBody] DeleteProductCustomerTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductCustomerTypeMan.DeleteProductCustomerTypeAsync(model);

            return getData;
        }
    }
}
