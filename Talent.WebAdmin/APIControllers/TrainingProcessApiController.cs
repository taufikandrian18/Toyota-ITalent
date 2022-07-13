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
    [Route("api/v1/training-process")]
    public class TrainingProcessApiController : Controller
    {
        private readonly TrainingProcessService TrainingProcessMan;

        public TrainingProcessApiController(TrainingProcessService trainingProcessService)
        {
            this.TrainingProcessMan = trainingProcessService;
        }

        [HttpGet("get-training-process", Name = "get-training-process")]
        public async Task<ActionResult<TrainingProcessViewModel>> GetTrainingProcessFiltered([FromQuery] TrainingProcessFilter filter)
        {
            var trainings = await TrainingProcessMan.GetTrainingProcess(filter);
            return Ok(trainings);
        }

        [HttpGet("get-training-process-by-id", Name = "get-training-process-detail-by-id")]
        public async Task<ActionResult<TrainingProcessDetailViewModel>> GetTrainingProcessById(int trainingId)
        {
            var trainings = await TrainingProcessMan.GetTrainingProcessById(trainingId);
            return Ok(trainings);
        }

        [HttpPost("update-training-process-by-id", Name = "update-training-process-detail-by-id")]
        public async Task<ActionResult<bool>> UpdateTrainingProcessById([FromBody] TrainingProcessDetailViewModel model, int trainingId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            var trainings = await TrainingProcessMan.UpdateTrainingProcess(model, trainingId);

            return Ok(true);
        }

        [HttpPost("save-draft-training-process", Name = "save-draft-training-process")]
        public async Task<ActionResult<bool>> SaveAsDraftTrainingProcess([FromBody] TrainingProcessDetailViewModel model, int trainingId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            var result = await this.TrainingProcessMan.SaveAsDraftTrainingProcess(model, trainingId);
            
            return Ok(result);
        }

        [HttpGet("download-training-process/{trainingId}", Name = "download-training-process")]
        public async Task<ActionResult> DownloadTrainingProcess(int trainingId)
        {
            var MS = await TrainingProcessMan.DownloadExcel(trainingId);

            return File(MS, FormatDocumentEnum.ExcelFormat, "Summary of Confirmed Training List.xlsx");
        }

        [HttpPost("export-training-process", Name = "export-training-process")]
        public async Task<ActionResult> ExportTrainingProcess([FromBody] List<VMExportTrainingProcessInput> model)
        {
            var trainings = await TrainingProcessMan.ExportExcel(model);
            return trainings;
        }

    }
}
