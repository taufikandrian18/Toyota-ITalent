using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-learning-history")]
    [ApiController]
    public class UserSideLearningHistoryController : ControllerBase
    {
        private readonly UserSideLearningHistoryService LearningHistoryService;
        private readonly UserSideAuthService AuthService;

        public UserSideLearningHistoryController(UserSideLearningHistoryService learningHistoryService, UserSideAuthService authService)
        {
            this.LearningHistoryService = learningHistoryService;
            this.AuthService = authService;
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<UserSideLearningHistoryResponseModel>> GetLearningHistory(string employeeId, [FromQuery]UserSideLearningHistoryFilterModel filter)
        {
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

            //var response = await this.LearningHistoryService.GetCompleteLearningHistory(filter, employeeId);
            var response = await this.LearningHistoryService.GetLearningHistory(filter, employeeId);
            return response;
        }

        [HttpGet("by-token")]
        public async Task<ActionResult<UserSideLearningHistoryResponseModel>> GetLearningHistoryByToken([FromQuery]UserSideLearningHistoryFilterModel filter)
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

            var response = await this.LearningHistoryService.GetLearningHistory(filter, responseToken.EmployeeId);
            //var response = await this.LearningHistoryService.GetCompleteLearningHistory(filter, responseToken.EmployeeId);
            return response;
        }

        [HttpGet("complete/{employeeId}")]
        public async Task<ActionResult<UserSideLearningHistoryResponseModel>> GetCompleteLearningHistory(string employeeId, [FromQuery]UserSideLearningHistoryFilterModel filter)
        {
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

            var response = await this.LearningHistoryService.GetCompleteLearningHistory(filter, employeeId);
            //var response = await this.LearningHistoryService.GetLearningHistory(filter, employeeId);
            return response;
        }

        [HttpGet("complete/by-token")]
        public async Task<ActionResult<UserSideLearningHistoryResponseModel>> GetCompleteLearningHistoryByToken([FromQuery]UserSideLearningHistoryFilterModel filter)
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

            //var response = await this.LearningHistoryService.GetLearningHistory(filter, responseToken.EmployeeId);
            var response = await this.LearningHistoryService.GetCompleteLearningHistory(filter, responseToken.EmployeeId);
            return response;
        }

        [HttpGet("detail/{employeeId}/{learningHistoryId}")]
        public async Task<ActionResult<UserSideDetailLearningHistoryModel>> GetDetailLearningHistory(string employeeId, int learningHistoryId)
        {
            var result = await this.LearningHistoryService.GetDetailLearningHistory(employeeId, learningHistoryId);

            return Ok(result);
        }

        [HttpGet("detail-by-token/{learningHistoryId}")]
        public async Task<ActionResult<UserSideDetailLearningHistoryModel>> GetDetailLearningHistoryByToken(int learningHistoryId)
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

            var result = await this.LearningHistoryService.GetDetailLearningHistory(response.EmployeeId, learningHistoryId);

            return Ok(result);
        }

        [HttpGet("filter-value")]
        public async Task<ActionResult<UserSideLearningHistoryFilterValueModel>> GetLearningHistoryFilterValue()
        {
            var data = await this.LearningHistoryService.GetlearningHistoryFilterValue();
            if (data == null)
            {
                data = new UserSideLearningHistoryFilterValueModel();
                data.LearningTypes = new System.Collections.Generic.List<UserSideLearningHistoryFilterValueTypeModel>();
                data.MaterialTypes = new System.Collections.Generic.List<UserSideLearningHistoryFilterValueTypeModel>();
                data.ProgramTypes = new System.Collections.Generic.List<UserSideLearningHistoryFilterValueTypeModel>();
            }
            return Ok(data);
        }
    }
}