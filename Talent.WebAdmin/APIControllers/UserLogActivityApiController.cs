using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models.UserLogActivity;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/UserLog")]
    public class UserLogActivityApiController : Controller
    {
        private readonly UserActivityService UserActivityMan;

        public UserLogActivityApiController(UserActivityService userActivityLogService)
        {
            this.UserActivityMan = userActivityLogService;
        }

        [HttpGet("summary-activity")]
        public async Task<ActionResult<GeneralUserLogResponseModel>> GetUserSummaryActivity([FromQuery] string filter)
        {
            var data = await this.UserActivityMan.GetUserActivityLog(filter);
            return Ok(data);
        }
    }
}
