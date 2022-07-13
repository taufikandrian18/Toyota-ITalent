using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.Entities.DbQueryModels;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/report-tracking")]
    public class ReportTrackingController : Controller
    {
        private readonly ReportTrackingService ReportTrackingService;

        public ReportTrackingController(ReportTrackingService service)
        {
            this.ReportTrackingService = service;
        }

        [HttpGet("competency")]
        public async Task<ActionResult<BaseResponse>> GetDataExport(GetTrackingParameter model)
        {
            var data = await this.ReportTrackingService.ExportExcel(model);
            return data;
        }

        [HttpGet("competency-api")]
        public async Task<ActionResult> GetDataExportAPI(GetTrackingParameter model)
        {
            return await this.ReportTrackingService.CompetencyTracking(model);
        }

        [HttpGet("kpi_backup")]
        public async Task<ActionResult<BaseResponse>> GetDataExportKPI(GetKPIParameter model)
        {
            var data = await this.ReportTrackingService.ExportExcelKPI(model);
            return data;
        }

        [HttpGet("kpi")]
        public async Task<ActionResult<BaseResponse>> GetDataExportKPI2(GetKPIParameter model)
        {
            var data = await this.ReportTrackingService.ExportExcelKPI2(model);
            return data;
        }
    }
}