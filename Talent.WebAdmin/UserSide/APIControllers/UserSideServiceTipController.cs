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
    [Route("api/v1/userside-service-tips")]
    public class UserSideServiceTipController : Controller
    {
        private readonly UserSideServiceTipService ServiceTipMan;
        private readonly UserSideAuthService UserSideMan;

        public UserSideServiceTipController(UserSideServiceTipService service, UserSideAuthService userSide)
        {
            this.ServiceTipMan = service;
            this.UserSideMan = userSide;
        }
        [HttpGet("get-all-service-tip-filtered")]
        public async Task<ActionResult<List<UserSideServiceTipPaginateModel>>> GetAllServiceTipFilteredAsync([FromQuery] Guid serviceTipMenuId)
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

            var data = await this.ServiceTipMan.GetServiceTipPaginate(serviceTipMenuId);

            return Ok(data);
        }
        [HttpGet("get-service-tip-detail")]
        public async Task<ActionResult<UserSideServiceTipModel>> GetServiceTipDetailAsync(Guid serviceTipId)
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

            var data = await this.ServiceTipMan.GetUserSideServiceTipDetail(serviceTipId);

            return Ok(data);
        }
        [HttpGet("get-all-service-tip-menu-awal")]
        public async Task<ActionResult<List<UserSideServiceTipMenuAwalModel>>> GetAllServiceTipMenuAwalAsync()
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

            var data = await this.ServiceTipMan.GetServiceTipMenuAwalPaginate();

            return Ok(data);
        }
    }
}
