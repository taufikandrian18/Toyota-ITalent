using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    /// <summary>
    /// Team Web API controller for providing Web APIs such as retrieve teams data.
    /// </summary>
    [Route("api/v1/userside-team")]
    public class UserSideTeamController : ControllerBase
    {
        private readonly UserSideTeamService TeamMan;
        private readonly UserSideAuthService AuthService;

        public UserSideTeamController(UserSideTeamService service, UserSideAuthService authService)
        {
            this.TeamMan = service;
            this.AuthService = authService;
        }

        /// <summary>
        /// Get all team details data by employee id.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>List of team data in <seealso cref="List{UserSideTeamViewModel}"/> format.</returns>
        [HttpGet("get-team-by-id")]
        public async Task<ActionResult<UserSideTeamModel>> GetTeamByIdAsync([FromQuery] string employeeId)
        {
            var data = await this.TeamMan.GetTeamByEmployeeId(employeeId);

            return Ok(data);
        }

        /// <summary>
        /// Get all team details data by Token.
        /// </summary>
        /// <returns>List of team data in <seealso cref="List{UserSideTeamViewModel}"/> format.</returns>
        [HttpGet("get-team-by-token")]
        public async Task<ActionResult<UserSideTeamModel>> GetTeamByTokenAsync()
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

            var data = await this.TeamMan.GetTeamByEmployeeId(response.EmployeeId);

            return Ok(data);
        }

        /// <summary>
        /// Get all employee data for show data employee before Rotate Team
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-leader")]
        public async Task<ActionResult<UserSideTeamResponseModel>> GetAllLeader([FromQuery]UserSideTeamFilterModel filter)
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

            // Set data per-page.
            if (filter.PageSize < 1)
            {
                filter.PageSize = 10;
            }

            // Set page index (current page).
            if (filter.PageIndex < 1)
            {
                filter.PageIndex = 1;
            }

            var data = await this.TeamMan.GetAllLeader(filter, response.EmployeeId);
            return Ok(data);
        }

        /// <summary>
        /// Roatate Team by team id & teams
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="teams"></param>
        /// <returns></returns>
        [HttpPut("rotate-team/{teamId}", Name = "rotate-team")]
        public async Task<IActionResult> RotateTeam(int teamId, [FromForm] List<UserSideTeamFormModel> teams)
        {
            await this.TeamMan.RotateTeam(teamId, teams);
            return Ok();
        }

        /// <summary>
        /// Delete my team by employee id.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpDelete("delete-team")]
        public async Task<IActionResult> DeleteMyTeam(string employeeId)
        {
            await this.TeamMan.DeleteMyTeam(employeeId);
            return Ok();
        }

        /// <summary>
        /// Create new team member.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("submit-rotate-by-token")]
        public async Task<IActionResult> SubmitRotateByTokenAsync(int nextTeamId, string employeeId)
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

            await this.TeamMan.CreateRotateAsync(employeeId, nextTeamId, response);
            return Ok();
        }

    }
}