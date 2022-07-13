using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-taskanswer")]
    public class UserSideTaskAnswerController : Controller
    {
        private UserSideTaskAnswerService _taskAnswerMan;
        private UserSideAuthService UserSideMan;
        private UserSideBulkCalculatingLearningService CalculateMan;
        private readonly AppSettings Settings;

        public UserSideTaskAnswerController(UserSideTaskAnswerService userSideTaskAnswerService, UserSideAuthService userSide, UserSideBulkCalculatingLearningService calculate, AppSettings appSettings)
        {
            this._taskAnswerMan = userSideTaskAnswerService;
            this.UserSideMan = userSide;
            this.CalculateMan = calculate;
            this.Settings = appSettings;
        }
       
        [HttpPost("insert-task-answer")]
        public async Task<ActionResult<bool>> InsertTaskAnswer([FromBody]TaskAnswerInsertModel insert, bool? withTest = true)
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
            var data = await this._taskAnswerMan.InsertTaskAnswer(insert, employeeId, withTest.GetValueOrDefault());
            return Ok(data);
        }

        [HttpPost("finish-module")]
        public async Task<ActionResult<bool>> FinishModule(int setupModuleId, string employeeId, int trainingId)
        {  
            var data = await this._taskAnswerMan.FinishModule(setupModuleId, employeeId, trainingId);
            return Ok(data);
        }


        [HttpPost("calculate-bulk/{rangeDate}")]
        public async Task<ActionResult<bool>> CalculateBulk(int rangeDate)
        {
            var hastoken = Request.Headers.ContainsKey("Authorization");
            if (hastoken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["Authorization"].FirstOrDefault();

            var isTokenValid = token == this.Settings.TokenSecretKey;

            if (isTokenValid == false)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await CalculateMan.BulkCalculating(rangeDate);
            return Ok(data);
        }

        [HttpPost("calculate-bulk-final-score")]
        public async Task<ActionResult<bool>> CalculateBulkFinalScore()
        {
            var data = await CalculateMan.BulkCalculatingFinalScore();

            return Ok(data);
        }

         [HttpPost("calculate-bulk-final-score-hierarki")]
        public async Task<ActionResult<bool>> CalculateBulkFinalScoreHierarki()
        {
            var datum = await CalculateMan.BulkCalculatingFinalScoreHierarki();

            return Ok(datum);
        }
    }
}
