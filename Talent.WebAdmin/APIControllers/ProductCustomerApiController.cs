using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductCustomers")]
    public class ProductCustomerApiController : Controller
    {
        private readonly ProductCustomerService ProductCustomerMan;

        public ProductCustomerApiController(ProductCustomerService productCustomerService)
        {
            this.ProductCustomerMan = productCustomerService;
        }

        [HttpGet("get-all-product-customers")]
        public async Task<ActionResult<ProductCustomerPaginate>> GetAllProductCustomerAsync([FromQuery] ProductCustomerGridFilter filter)
        {
            var data = await this.ProductCustomerMan.GetAllProductCustomer(filter);
            return Ok(data);
        }

        [HttpPost("insert-product-customer", Name = "InsertProductCustomerAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductCustomerAsync([FromBody] ProductCustomerCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductCustomerMan.InsertProductCustomerAsync(model);

            return success;
        }
        [HttpGet("get-product-customer-by-id", Name = "GetProductCustomerByIdAsync")]
        public async Task<ActionResult<ProductCustomerModel>> GetProductCustomerByIdAsync([FromQuery] Guid ProductCustomerId)
        {
            var data = await this.ProductCustomerMan.GetProductCustomerById(ProductCustomerId);

            return Ok(data);
        }
        [HttpPost("update-product-customer-by-id", Name = "UpdateProductCustomerAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductCustomerAsync([FromBody] ProductCustomerUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductCustomerMan.UpdateProductCustomerAsync(model);

            return success;
        }
        [HttpDelete("delete-product-customer", Name = "DeleteProductCustomerAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductCustomerAsync([FromBody] DeleteProductCustomerModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductCustomerMan.DeleteProductCustomerAsync(model);

            return getData;
        }
    }
}
