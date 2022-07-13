using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-review")]
    public class UserSideReviewApiController : Controller
    {
        private readonly UserSideReviewService revMan;
        private readonly UserSideAuthService AuthService;

        public UserSideReviewApiController(UserSideReviewService reviewService, UserSideAuthService authService)
        {
            this.revMan = reviewService;
            this.AuthService = authService;
        }

        [HttpGet("get-all-coach")]
        public async Task<ActionResult<List<UserSideCoachReviewModel>>> GetCoachReviewAsync([FromQuery] int trainingId)
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

            var data = await this.revMan.GetUserSideCoachReviewAsync(trainingId);

            return data;
        }

        [HttpPost("insert-training-review")]
        public async Task<ActionResult> InsertTrainingReviewAysnc([FromBody] UserSideTrainingReviewSubmitModel model)
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

            await this.revMan.InsertNewTrainingRatingAsync(model, response.EmployeeId);

            return Ok();
        }

        [HttpPost("insert-coach-review")]
        public async Task<ActionResult> InsertCoachReviewAysnc([FromBody] UserSideCoachReviewSubmitModel model)
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

            await this.revMan.InsertNewCoachRatingAsync(model, response.EmployeeId);

            return Ok();
        }
    }
}
