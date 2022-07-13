using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-myinsight")]
    public class UserSideMyInsightController : Controller
    {
        private readonly UserSideSurveyService SurveyMan;
        private readonly UserSideMyInsightService InsightMan;
        private readonly UserSideAuthService AuthService;

        public UserSideMyInsightController(UserSideMyInsightService insightService, UserSideAuthService authService, UserSideSurveyService userSideSurveyService)
        {
            this.SurveyMan = userSideSurveyService;
            this.InsightMan = insightService;
            this.AuthService = authService;
        }

        [HttpGet("get-all-survey")]
        public async Task<ActionResult<List<UserSideMyInsightModel>>> GetAllSurveyAsync([FromQuery] string json, List<int> userPositionId)
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

            UserSideMyInsightFilterModel filter = JsonConvert.DeserializeObject<UserSideMyInsightFilterModel>(json);

            var data = await this.InsightMan.GetAllSurveys(filter, userPositionId);

            return Ok(data);
        }

        [HttpGet("user-side-get-survey-by-id")]
        public async Task<ActionResult<object>> UserSideGetSurveyById(int id)
        {
            var data = await this.SurveyMan.UserSideGetSurveyById(id);
            return Ok(data);
        }

        [HttpGet("user-side-get-survey-question-by-id")]
        public async Task<ActionResult<List<int>>> GetAllSurveyQuestionAsync([FromQuery] int surveyId)
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

            var data = await this.InsightMan.GetAllSurveyQuestionAsync(surveyId);

            return Ok(data);
        }
    }
}
