using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-coach-schedule")]
    [ApiController]
    public class UserSideCoachScheduleController : ControllerBase
    {
        private readonly UserSideScheduleService UserSideScheduleService;
        private readonly UserSideAuthService AuthService;

        public UserSideCoachScheduleController(UserSideScheduleService service, UserSideAuthService authService)
        {
            this.UserSideScheduleService = service;
            this.AuthService = authService;
        }

        /// <summary>
        /// Get data schedule by employee ID.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>List of schedule data in <seealso cref="List{UserSideCoachScheduleViewModel}"/> format.</returns>
        [HttpGet("get-schedule-by-id/{employeeId}")]
        public async Task<ActionResult<List<UserSideCoachScheduleViewModel>>> GetScheduleByIdAsync(string employeeId)
        {
            var data = await this.UserSideScheduleService.GetSchedule(employeeId);
            return Ok(data);
        }

        /// <summary>
        /// Get data schedule by Token
        /// </summary>
        /// <returns>List of schedule data in <seealso cref="List{UserSideCoachScheduleViewModel}"/> format.</returns>
        [HttpGet("get-schedule-by-token")]
        public async Task<ActionResult<List<UserSideCoachScheduleViewModel>>> GetScheduleByTokenAsync()
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
                var retdata = new List<UserSideCoachScheduleViewModel>();
                return Ok(retdata);
                //return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.UserSideScheduleService.GetSchedule(response.EmployeeId);
            if (data == null)
            {
                data = new List<UserSideCoachScheduleViewModel>();
            }
            return Ok(data);
        }

        /// <summary>
        /// Create new schedule.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="coachId"></param>
        /// <returns></returns>
        [HttpPost("create-schedule")]
        public async Task<IActionResult> CreateSchedule([FromForm] UserSideCoachScheduleFormModel model, int coachId)
        {
            await this.UserSideScheduleService.CreateSchedule(model, coachId);
            return Ok();
        }

        /// <summary>
        /// Delete schedule data coach schedule id.
        /// </summary>
        /// <param name="coachScheduleId"></param>
        /// <returns></returns>
        [HttpDelete("delete-schedule")]
        public async Task<IActionResult> DeleteSchedule([FromQuery] int coachScheduleId)
        {
            var isSuccess = await this.UserSideScheduleService.DeleteSchedule(coachScheduleId);

            if (isSuccess == false)
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Update data schedule.
        /// </summary>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        [HttpPut("update-schedule")]
        public async Task<ActionResult> UpdateAsync([FromForm] UserSideCoachScheduleUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var isSuccess = await this.UserSideScheduleService.UpdateSchedule(updateModel);

            if (isSuccess == false)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}