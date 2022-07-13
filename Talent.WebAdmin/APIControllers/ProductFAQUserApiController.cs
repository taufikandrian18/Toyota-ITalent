using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductFAQUsers")]
    public class ProductFAQUserApiController : Controller
    {
        private readonly ProductFAQUserService ProductFAQUserMan;

        public ProductFAQUserApiController(ProductFAQUserService productFAQUserService)
        {
            this.ProductFAQUserMan = productFAQUserService;
        }
        [HttpGet("get-all-product-faq-users")]
        public async Task<ActionResult<ProductFAQUserPaginate>> GetAllProductFAQUserAsync([FromQuery] ProductFAQUserGridFilter filter)
        {
            var data = await this.ProductFAQUserMan.GetAllProductFAQUser(filter);
            return Ok(data);
        }
    }
}
