using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-myguide")]
    public class UserSideMyGuideController : Controller
    {
        private readonly UserSideMyGuideService guideMan;
        private readonly UserSideAuthService UserSideMan;

        public UserSideMyGuideController(UserSideMyGuideService myGuideService, UserSideAuthService authService)
        {
            this.guideMan = myGuideService;
            this.UserSideMan = authService;
        }

        [HttpGet("get-all-guide")]
        public async Task<ActionResult<UserSideMyGuideAPIModel>> GetAllGuideAsync()
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
              return  BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.guideMan.GetAllGuideAsync();

            return data;
        }

        [HttpGet("get-tutorial-guide")]
        public async Task<ActionResult<UserSideTutorialGuideModel>> GetTutorialAsync()
        {
            var data = await this.guideMan.GetTutorialAsync();

            return Ok(data);
        }
    }
}
