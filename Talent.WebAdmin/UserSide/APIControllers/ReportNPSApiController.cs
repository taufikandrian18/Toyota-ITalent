using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    /// <summary>
    /// User Side Banner Web API controller for providing Web APIs such as retrieve banner data.
    /// </summary>
    [Route("api/v1/report-nps")]
    public class ReportNPSApiController : Controller
    {
        private readonly ReportNPSService Service;

        public ReportNPSApiController(ReportNPSService service)
        {
            this.Service = service;
        }

        /// <summary>
        /// Get all Report NPS 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-report-nps", Name = "get-all-report-nps")]
        public async Task<ActionResult<ReportNPSViewModel>> GetReportNPS([FromQuery]ReportNPSFilterModel filterModel)
        {
            if (filterModel.PageNumber < 1)
            {
                filterModel.PageNumber = 1;
            }
            if (filterModel.PageSize < 1)

            {
                filterModel.PageSize = 10;
            }

            var data = await this.Service.GetPaginateReportNPS(filterModel);
            return Ok(data);
        }
    }
}