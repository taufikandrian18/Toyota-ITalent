using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-myteam")]
    public class UserSideMyTeamApiController : Controller
    {
        private readonly UserSideMyTeamService teamMan;
        private readonly UserSideAuthService AuthService;

        public UserSideMyTeamApiController(UserSideMyTeamService service, UserSideAuthService authService)
        {
            this.teamMan = service;
            this.AuthService = authService;
        }
        
        [HttpGet("get-all-team-employee")]
        public async Task<ActionResult<UserSideValidAddModel>> GetAllTeamEmployeeAsync([FromQuery] string json)
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

            UserSideMyTeamEmployeeFilterModel filter = JsonConvert.DeserializeObject<UserSideMyTeamEmployeeFilterModel>(json);

            var data = await this.teamMan.GetAllTeamEmployeeAsync(filter, response.EmployeeId);

            var valid = await this.teamMan.ValidateAddEmployee(response.EmployeeId);

            UserSideValidAddModel addModel = new UserSideValidAddModel()
            {
                ableToAdd = valid,
                nonTeamEmployees = data
            };

            return Ok(addModel);
        }

        [HttpPost("request-new-member")]
        public async Task<ActionResult> RequestNewMemberAsync([FromBody] string newEmployeeId)
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

            await this.teamMan.RequestNewMemberAsync(response.EmployeeId, newEmployeeId);

            return Ok();
        }

        
    }
}
