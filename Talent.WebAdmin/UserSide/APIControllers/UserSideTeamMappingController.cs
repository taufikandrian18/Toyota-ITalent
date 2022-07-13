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
    [Route("api/v1/userside-team-mapping")]
    [ApiController]
    public class UserSideTeamMappingController : ControllerBase
    {
        private readonly UserSideTeamMappingService TeamMappingMan;
        private readonly UserSideAuthService AuthService;

        public UserSideTeamMappingController(UserSideTeamMappingService service, UserSideAuthService authService)
        {
            this.TeamMappingMan = service;
            this.AuthService = authService;
        }

        [HttpGet("get-team-position")]
        public async Task<ActionResult<List<UserSidePositionModel>>> GetTeamPositionByTeamId([FromQuery] int teamId)
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
                var retdata = new List<UserSidePositionModel>();
                return Ok(retdata);
                //return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.TeamMappingMan.GetTeamPosition(response.EmployeeId, teamId);

            return Ok(data);
        }

        [HttpGet("get-team-mapping-by-id")]
        public async Task<ActionResult<UserSideTeamMappingModel>> GetTeamMappingByIdAsync([FromQuery] int teamId, int positionId)
        {
            var data = await this.TeamMappingMan.GetTeamMappingByPositionId(teamId, positionId);

            return Ok(data);
        }

        [HttpGet("get-team-mapping-by-position-name")]
        public async Task<ActionResult<UserSideTeamMappingModel>> GetTeamMapping([FromQuery] int teamId, string positionName)
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
            var data = await this.TeamMappingMan.GetTeamMappingByPositionName(response.EmployeeId, teamId, positionName);

            return Ok(data);
        }
    }
}