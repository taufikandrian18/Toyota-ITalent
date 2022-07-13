using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-personal-mapping")]
    [ApiController]
    public class UserSidePersonalMappingController : ControllerBase
    {
        private readonly UserSidePersonalMappingService PersonalMappingMan;
        private readonly UserSideAuthService AuthService;

        public UserSidePersonalMappingController(UserSidePersonalMappingService service, UserSideAuthService authService)
        {
            this.PersonalMappingMan = service;
            this.AuthService = authService;
        }

        [HttpGet("get-personal-mapping-by-employee-id")]
        public async Task<ActionResult<UserSidePersonalMappingModel>> GetPersonalMappingByEmployeeIdAsync([FromQuery] string employeeId)
        {
            var data = await this.PersonalMappingMan.GetPersonalMappingByEmployeeId(employeeId);

            return Ok(data);
        }
        
        [HttpGet("get-personal-mapping-by-token")]
        public async Task<ActionResult<UserSidePersonalMappingModel>> GetPersonalMappingByTokenAsync()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var responseToken = this.AuthService.VerifyMobileToken(token);

            if (responseToken == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.PersonalMappingMan.GetPersonalMappingByEmployeeId(responseToken.EmployeeId);

            return Ok(data);
        }

    }
}