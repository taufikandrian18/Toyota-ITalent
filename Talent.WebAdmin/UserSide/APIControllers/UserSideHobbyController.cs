using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-hobby")]
    [ApiController]
    public class UserSideHobbyController : ControllerBase
    {
        private readonly UserSideHobbyService HobbyService;

        public UserSideHobbyController(UserSideHobbyService hobbyService)
        {
            this.HobbyService = hobbyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserSideHobbyModel>>> GetHobbies()
        {
            var dataHobies = await this.HobbyService.GetHobbies();

            return Ok(dataHobies);
        }
    }
}