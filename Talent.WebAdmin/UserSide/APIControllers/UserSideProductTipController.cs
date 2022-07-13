using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-product-tips")]
    public class UserSideProductTipController : Controller
    {
        private readonly UserSideProductTipService ProductTipMan;
        private readonly UserSideAuthService UserSideMan;
        private readonly UserActivityService UserActivityMan;

        public UserSideProductTipController(UserSideProductTipService service, UserSideAuthService userSide, UserActivityService userActivityLogService)
        {
            this.ProductTipMan = service;
            this.UserSideMan = userSide;
            this.UserActivityMan = userActivityLogService;
        }

        [HttpGet("get-all-product-tip-filtered")]
        public async Task<ActionResult<List<UserSideProductTipPaginateModel>>> GetAllProductTipFilteredAsync([FromQuery] Guid productId)
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

            var data = await this.ProductTipMan.GetProductTipPaginate(productId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Sales Tip";
            log.Content = "Sales Tip " + productId;

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }
        [HttpPost("contribute-new-sales-tip")]
        public async Task<ActionResult<bool>> ContributeNewSalesProductTipAsync([FromBody] UserSideContributeNewSalesTipCreateModel model)
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

            var data = await this.ProductTipMan.ContributeNewSalesTipAsync(model,response.EmployeeId);

            return Ok(data);
        }
        [HttpGet("get-product-sales-tip-detail")]
        public async Task<ActionResult<UserSideProductTipModel>> GetProductSalesTipDetailAsync(Guid productTipId,Guid productId)
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

            var data = await this.ProductTipMan.GetUserSideSalesTipDetail(productTipId, productId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Sales Tip";
            log.Content = "Sales Tip " + productId;

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }
    }
}
