using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-product-customer")]
    public class UserSideProductCustomerController : Controller
    {
        private readonly UserSideProductCustomerService ProductCustomerMan;
        private readonly UserSideAuthService UserSideMan;

        public UserSideProductCustomerController(UserSideProductCustomerService service, UserSideAuthService userSide)
        {
            this.ProductCustomerMan = service;
            this.UserSideMan = userSide;
        }

        [HttpGet("get-all-product-customer-filtered/{productCustomerTypeName}")]
        public async Task<ActionResult<UserSideProductCustomerPaginate>> GetAllProductCustomerFilteredAsync(int pageSize, int pageIndex, string productCustomerTypeName, Guid productId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.ProductCustomerMan.GetAllUserSideProductCustomerFiltered(pageSize, pageIndex, productCustomerTypeName, productId);

            return Ok(data);
        }
        [HttpGet("get-all-product-customer-type-list")]
        public async Task<ActionResult<List<UserSideProductCustomerTypeListView>>> GetAllProductCustomerTypeListAsync([FromQuery] Guid productId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.ProductCustomerMan.GetAllUserSideProductCustomerTypeName(productId);

            return data;
        }
    }
}
