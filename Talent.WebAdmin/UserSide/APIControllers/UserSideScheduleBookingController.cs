using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/schedule-booking")]
    public class UserSideScheduleBookingController : ControllerBase
    {
        private readonly UserSideScheduleBookingService ScheduleBookingMan;
        private readonly UserSideAuthService passportService;

        public UserSideScheduleBookingController(
            UserSideScheduleBookingService service,
            UserSideAuthService passportService)
        {
            this.ScheduleBookingMan = service;
            this.passportService = passportService;
        }

        [HttpPost("coach-schedule-booking")]
        public async Task<ActionResult> ScheduleBookingSend(int coachScheduleId, string message)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = passportService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var result = await this.ScheduleBookingMan.BookingScheduleCoach(coachScheduleId, message, response.EmployeeId);

            if (result == false)
            {
                return BadRequest("Failed to send email");
            }

            return Ok();
        }
    }
}