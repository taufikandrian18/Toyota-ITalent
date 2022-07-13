using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-coach")]
    [ApiController]
    public class UserSideCoachController : ControllerBase
    {
        private readonly UserSideCoachService CoachService;
        private readonly UserSideAuthService PassportService;


        public UserSideCoachController(UserSideCoachService coachService, UserSideAuthService userSideAuthService)
        {
            this.CoachService = coachService;
            this.PassportService = userSideAuthService;
        }

        [HttpGet]
        public async Task<ActionResult<UserSideCoachResponseModel>> GetCoachList([FromQuery]UserSideCoachFilterModel filter)
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

            var response = await this.CoachService.GetCoachList(filter);
            return response;
        }

        [HttpGet("coach-schedule-booking/{coachId}")]
        public async Task<ActionResult<UserSideCoachScheduleBookingModel>> GetCoachScheduleBooking(int coachId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = PassportService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var dataCoach = await this.CoachService.GetCoachScheduleBooking(coachId,response.EmployeeId);

            return Ok(dataCoach);
        }
    }
}