using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/survey-report")]
    public class SurveyReportApiController : Controller
    {
        private readonly SurveyReportService _SurveyReportMan;

        public SurveyReportApiController(SurveyReportService surveyReportService)
        {
            _SurveyReportMan = surveyReportService;
        }

        [HttpGet("get-all-survey-report")]
        public async Task<ActionResult<SurveyReportViewModel>> GetAllSurveyReportData([FromQuery] SurveyReportFilter filter)
        {
            var data = await _SurveyReportMan.GetAllSurveyReportData(filter);

            return Ok(data);
        }

        [HttpGet("generate-excel/{surveyId}")]
        public async Task<ActionResult> DownloadReportExcel(int surveyId)
        {
            var MS = await _SurveyReportMan.GenerateExcel(surveyId);

            return File(MS, FormatDocumentEnum.ExcelFormat, "Report Inquiry.xlsx");
        }
    }
}
