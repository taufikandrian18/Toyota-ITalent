using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/training-score-report")]
    public class TrainingScoreReportApiController : Controller
    {

        private readonly TrainingScoreReportService TrainingScoreReportMan;

        public TrainingScoreReportApiController(TrainingScoreReportService trainingScoreReportService)
        {
            this.TrainingScoreReportMan = trainingScoreReportService;
        }

        [HttpGet("get-training-score-report", Name = "get-training-score-report")]
        public async Task<ActionResult<ReportTrainingScoreViewModel>> GetReportTrainingScore([FromQuery] ReportTrainingScoreFilterModel filter)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model State is Not Valid");
            }

            var result = await this.TrainingScoreReportMan.GetTrainingScoreReport(filter);

            return Ok(result);
        }

        [HttpGet("get-program-types", Name = "get-program-types")]
        public async Task<ActionResult<List<ProgramTypeModel>>> GetAllProgramTypes()
        {
            var result = await this.TrainingScoreReportMan.GetAllProgramTypes();

            return Ok(result);
        }

        [HttpGet("download-training-score-report/{id}", Name = "download-training-score-report")]
        public async Task<ActionResult> DownloadTrainingScoreReportById(int id)
        {
            var MS = await this.TrainingScoreReportMan.GenerateReportTrainingScore(id);

            return File(MS, FormatDocumentEnum.ExcelFormat, "Training Score Report.xlsx");
        }
    }
}
