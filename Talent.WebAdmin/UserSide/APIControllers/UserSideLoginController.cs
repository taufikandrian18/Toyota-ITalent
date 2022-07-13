using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-login")]
    public class UserSideLoginController : Controller
    {
        private readonly UserSideLoginService LoginMan;
        private readonly UserSideAuthService AuthService;

        public UserSideLoginController(UserSideLoginService service, UserSideAuthService authService)
        {
            this.LoginMan = service;
            this.AuthService = authService;
        }

        [HttpGet("get-user-info")]
        public async Task<ActionResult<UserSideLoginReturnModel>> GetUserInfo()
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("ModelState is invalid!");
            }

            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                //Logger.LogError("TAM.Passport JSON Web Token not found!");
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var returnToken = await this.AuthService.VerifyTokenSSOAsync(token);

            // Token Invalid
            if (returnToken == null)
            {
                var err = "Token no longer valid";
                return BadRequest(err);
            }

            var error = AuthService.ValidateMobileTokenSSO(returnToken);
            if (string.IsNullOrEmpty(error) == false)
            {
                return BadRequest(error);
            }

            var data = await this.LoginMan.GetUserInfo(returnToken);
            return Ok(data);
        }

    }
}
