using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-badges")]
    public class UserSideBadgesController : ControllerBase
    {
        private readonly UserSideBadgesService Badges;
        private readonly UserSideAuthService AuthService;

        public UserSideBadgesController(UserSideBadgesService service, UserSideAuthService authService)
        {
            this.Badges = service;
            this.AuthService = authService;
        }

        [HttpGet("get-badges")]
        public async Task<ActionResult<UserSideBadgesViewModel>> GetBadgesAsync([FromQuery] string employeeId)
        {
            var data = await this.Badges.GetBadges(employeeId);

            return Ok(data);
        }

        [HttpGet("get-badges-by-token")]
        public async Task<ActionResult<UserSideBadgesViewModel>> GetBadgesByTokenAsync()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var responseToken =  this.AuthService.VerifyMobileToken(token);

            if (responseToken == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }
            var data = await this.Badges.GetBadges(responseToken.EmployeeId);

            return Ok(data);
        }
    }
}