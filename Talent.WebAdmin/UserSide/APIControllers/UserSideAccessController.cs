using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/userside-access")]
    public class UserSideAccessController : Controller
    {
        private readonly UserSideAccessService AccessMan;
        private readonly UserSideAuthService AuthService;

        public UserSideAccessController(UserSideAccessService access, UserSideAuthService authService)
        {
            this.AccessMan = access;
            this.AuthService = authService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartAccess()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var employeeAccessTimeId = await this.AccessMan.StartAccess(response.EmployeeId);

            return Ok(employeeAccessTimeId);
        }

        [HttpPost("end")]
        public async Task<IActionResult> EndAccess(int employeeAccessTimeId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            await this.AccessMan.EndAccess(employeeAccessTimeId);

            return Ok();
        }


    }
}
