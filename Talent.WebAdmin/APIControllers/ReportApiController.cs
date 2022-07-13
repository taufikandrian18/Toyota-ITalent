using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/report")]
    public class ReportApiController : Controller
    {
        private readonly ReportService ReportMan;

        public ReportApiController(ReportService reportService)
        {
            this.ReportMan = reportService;
        }

        [HttpGet("get-all-report", Name = "get-all-report")]
        public async Task<ActionResult<List<ReportModel>>> GetAllReport()
        {
            var result = await this.ReportMan.GetAllItalentReport();

            return Ok(result);
        }
    }
}
