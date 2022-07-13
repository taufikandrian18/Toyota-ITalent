using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-team-course")]
    [ApiController]
    public class UserSideTeamCourseController : ControllerBase
    {
        private readonly UserSideTeamCourseService TeamCourseService;
        private readonly UserSideAuthService AuthService;

        public UserSideTeamCourseController(UserSideTeamCourseService teamCourseService, UserSideAuthService authService)
        {
            this.TeamCourseService = teamCourseService;
            this.AuthService = authService;
        }

        /// <summary>
        /// Get data course with pagination and search;
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("{employeeId}")]
        public async Task<ActionResult<UserSideMyProfileCourseResponseModel>> GetCourseTeamAsync([FromQuery]UserSideMyProfileCourseFilterModel filter, string employeeId)
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

            var response = await this.TeamCourseService.GetCoursesPaginationAsync(filter, employeeId);

            return response;
        }

        /// <summary>
        /// Get data course with pagination and search;
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("by-token")]
        public async Task<ActionResult<UserSideMyProfileCourseResponseModel>> GetCourseTeamByTokenAsync([FromQuery]UserSideMyProfileCourseFilterModel filter)
        {
            // Set data per-page.
            if(filter.PageSize < 1)
            {
                filter.PageSize = 10;
            }

            // Set page index (current page).
            if (filter.PageIndex < 1)
            {
                filter.PageIndex = 1;
            }

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

            var response = await this.TeamCourseService.GetCoursesPaginationAsync(filter, responseToken.EmployeeId);

            return response;
        }

        /// <summary>
        /// Assign course.
        /// Get AssignedBy from token. 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AsignCourse([FromForm]UserSideMyProfileAsignLearningModel form)
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

            await this.TeamCourseService.AsignCourse(form, responseToken.EmployeeId);

            return Ok();
        }

        [HttpPost("{assignedBy}")]
        public async Task<ActionResult> AsignCourseById([FromForm]UserSideMyProfileAsignLearningModel form, string assignedBy)
        {
            await this.TeamCourseService.AsignCourse(form, assignedBy);

            return Ok();
        }

        /// <summary>
        /// update asign course.
        /// Get AssignedBy from token.
        /// </summary>
        /// <param name="assignedLearningId"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPut("{assignedLearningId}")]
        public async Task<ActionResult> AsignCourseUpdate(int assignedLearningId, [FromForm]UserSideMyProfileAsignLearningModel form)
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

            await this.TeamCourseService.AsignCourseUpdate(form, assignedLearningId, responseToken.EmployeeId);

            return Ok();
        }
    }
}