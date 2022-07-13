using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.Entities.Entities;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-level")]
    public class UserSideLevelController : Controller
    {
        private readonly UserSideLevelService LevelMan;
        private readonly UserSideAuthService AuthService;

        public UserSideLevelController(UserSideLevelService userSideLevelService, UserSideAuthService authService)
        {
            this.LevelMan = userSideLevelService;
            this.AuthService = authService;
        }
        
        /// <summary>
        /// Return the user's next level and experience needed to get to the next level
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-next-level")]
        public async Task<IActionResult> GetNextLevel()
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

            var nextLevel = await this.LevelMan.GetNextLevel(response.EmployeeId);

            return Ok(nextLevel);
        }

    }
}
