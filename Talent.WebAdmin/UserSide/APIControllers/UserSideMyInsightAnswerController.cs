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
    [Route("api/v1/userside-myinsightanswer")]
    public class UserSideMyInsightAnswerController : Controller
    {
        private readonly UserSideMyInsightAnswerService _surveyAnswerMan;
        private readonly UserSideAuthService UserSideMan;

        public UserSideMyInsightAnswerController(UserSideMyInsightAnswerService userSideMyInsightAnswerService, UserSideAuthService userSide)
        {
            this._surveyAnswerMan = userSideMyInsightAnswerService;
            this.UserSideMan = userSide;
        }

        [HttpPost("survey-task-answer")]
        public async Task<ActionResult<bool>> InsertTaskAnswer([FromBody]SurveyAnswerInsertModel insert)
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
               return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }
            var employeeId = response.EmployeeId;
            var data = await this._surveyAnswerMan.InsertSurveyAnswer(insert, employeeId);
            return Ok(data);
        }
    }
}
