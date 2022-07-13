using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-product-faq")]
    public class UserSideProductFAQController : Controller
    {
        private readonly UserSideProductFAQService ProductFAQMan;
        private readonly UserSideAuthService UserSideMan;
        private readonly UserActivityService UserActivityMan;


        public UserSideProductFAQController(UserSideProductFAQService service, UserSideAuthService userSide, UserActivityService userActivityLogService)
        {
            this.ProductFAQMan = service;
            this.UserSideMan = userSide;
            this.UserActivityMan = userActivityLogService;
        }

        [HttpGet("get-all-product-faqs-filtered")]
        public async Task<ActionResult<List<UserSideProductFAQPaginate>>> GetAllProductFAQMainPageAsync(Guid ProductId)
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

            var data = await this.ProductFAQMan.GetProductFAQMainPageAsync(ProductId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Spec & Comparison";
            log.Content = "FAQ";

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }

        [HttpPost("contribute-product-faq-users")]
        public async Task<ActionResult<BaseResponse>> ContributeNewProductFAQUserAysnc([FromBody] UserSideProductFAQContributeModel model)
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

            var getData = await this.ProductFAQMan.ContributeNewProductFAQUserAsync(model, response.EmployeeId);

            return getData;
        }
        [HttpGet("get-all-product-faq-category-list")]
        public async Task<ActionResult<List<UserSideProductFAQCategoryView>>> GetAllProductFAQCategoryListAsync([FromQuery] Guid productId)
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

            var data = await this.ProductFAQMan.GetAllUserSideProductFAQCategory(productId);

            return data;
        }
    }
}
