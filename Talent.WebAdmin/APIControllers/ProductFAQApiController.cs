using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductFAQs")]
    public class ProductFAQApiController : Controller
    {
        private readonly ProductFAQService ProductFAQMan;

        public ProductFAQApiController(ProductFAQService productFAQService)
        {
            this.ProductFAQMan = productFAQService;
        }
        [HttpGet("get-all-product-faqs")]
        public async Task<ActionResult<ProductFAQPaginate>> GetAllProductFAQAsync([FromQuery] ProductFAQGridFilter filter)
        {
            var data = await this.ProductFAQMan.GetAllProductFAQ(filter);
            return Ok(data);
        }

        [HttpPost("insert-product-faq", Name = "InsertProductFAQAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductFAQAsync([FromBody] ProductFAQCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductFAQMan.InsertProductFAQAsync(model);

            return success;
        }

        [HttpGet("get-product-faq-by-id", Name = "GetProductFAQByIdAsync")]
        public async Task<ActionResult<ProductFAQModel>> GetProductFAQByIdAsync([FromQuery] Guid ProductFaqId)
        {
            var data = await this.ProductFAQMan.GetProductFAQById(ProductFaqId);

            return Ok(data);
        }

        [HttpPost("update-product-faq-by-id", Name = "UpdateProductFAQAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductFAQAsync([FromBody] ProductFAQUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductFAQMan.UpdateProductFAQAsync(model);

            return success;
        }

        [HttpDelete("delete-product-faq", Name = "DeleteProductFAQAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductFAQAsync([FromBody] DeleteProductFAQModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductFAQMan.DeleteProductFAQAsync(model);

            return getData;
        }
    }
}
