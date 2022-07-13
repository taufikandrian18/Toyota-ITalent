using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.UserSide.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-popquiz")]
    public class UserSidePopQuizController : Controller
    {
        private readonly UserSidePopQuizService PopQuizMan;
        private readonly UserSideAuthService AuthMan;

        public UserSidePopQuizController(UserSidePopQuizService service, UserSideAuthService authService)
        {
            this.PopQuizMan = service;
            this.AuthMan = authService;
        }

        [HttpGet("get-all-popquiz")]
        public async Task<ActionResult<List<UserSidePopQuizModel>>> GetAllPopQuizAsync([FromQuery] UserSidePopQuizFilterModel filter)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthMan.VerifyMobileToken(token);

            if (response == null)
            {
               return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var popquizzes = await PopQuizMan.GetAllPopQuiz(filter, response.EmployeeId);

            return Ok(popquizzes);
        }

    }
}
