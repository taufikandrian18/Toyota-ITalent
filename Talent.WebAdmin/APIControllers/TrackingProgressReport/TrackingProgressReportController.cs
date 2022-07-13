using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Models.TrackingProgressReport;
using Talent.WebAdmin.Services.TrackingProgressReport;

namespace Talent.WebAdmin.APIControllers.TrackingProgressReport
{
    [Route("api/v1/tracking-progress-report")]
    public class TrackingProgressReportController : Controller
    {
        private readonly TrackingReportProgressReportService TrackingReportService;

        public TrackingProgressReportController(TrackingReportProgressReportService service)
        {
            this.TrackingReportService = service;
        }

        [HttpGet("export-list-progress-tracking")]
        public async Task<ActionResult<BaseResponse>> GetDataProgressTrackingExport(GetAccountParameterProgressTracking model, string filetype)
        {

            var data = await this.TrackingReportService.ExportExcelTrackingReportProgress(model, filetype);
            return data;
        }

        [HttpGet("export-list-progress-tracking-detail")]
        public async Task<ActionResult<BaseResponse>> GetDataProgressTrackingDetailExport(int trainingId, int setupModuleId, string employeeId, string filetype, string flagging)
        {
            var data = await this.TrackingReportService.ExportExcelTrackingReportDetailProgress(trainingId,setupModuleId,employeeId,filetype,flagging);
            return data;
        }

        [HttpGet("export-list-progress-tracking-json")]
        public async Task<ActionResult<BaseResponse>> GetDataProgressTrackingJson(GetAccountParameterProgressTracking model)
        {
            var data = await this.TrackingReportService.GetDataProgressTrackingReportJson(model);
            return data;
        }

        [HttpGet("export-list-progress-tracking-detail-json")]
        public async Task<ActionResult<BaseResponse>> GetDataProgressTrackingDetailJson(int trainingId, int setupModuleId, string employeeId, string flagging)
        {
            var data = await this.TrackingReportService.ExportExcelTrackingReportDetailProgressJson(trainingId, setupModuleId, employeeId, flagging);
            return data;
        }
    }
}
