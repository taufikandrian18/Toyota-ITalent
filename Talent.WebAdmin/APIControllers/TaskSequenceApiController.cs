using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/sequence")]
    public class TaskSequenceApiController : Controller
    {
        private readonly SequenceService seqMan;

        public TaskSequenceApiController(SequenceService sequenceService)
        {
            this.seqMan = sequenceService;
        }

        [HttpGet("get-task-code", Name = "GetTaskCodeSequenceAsync")]
        public async Task<ActionResult<List<CompetencyMappingJoinModel>>> GetTaskCode()
        {
            var data = await this.seqMan.GetTaskCode();

            return Ok(data);
        }

        [HttpGet("get-module-option", Name = "GetModuleForTaskAsync")]
        public async Task<ActionResult<List<ModuleForTaskModel>>> GetModuleForTaskAsync()
        {
            var data = await this.seqMan.GetModuleAsync();

            return Ok(data);
        }

        [HttpGet("get-task-number", Name = "GetTaskNumberAsync")]
        public async Task<ActionResult<int>> GetTaskNumberAsync([FromQuery] GetNumberTaskModel model)
        {
            var totalData = await this.seqMan.GetNumber(model);

            return totalData;
        }

        [HttpGet("get-time-point", Name = "GetTimePointTaskAsync")]
        public async Task<ActionResult<List<TimePointTaskModel>>> GetTimePointTaskAsync()
        {
            var data = await this.seqMan.GetAllTaskTimePoints();

            return Ok(data);
        }

        [HttpPost("insert-sequence-task", Name = "InsertSequenceTaskAsync")]
        public async Task<ActionResult> InsertSequenceTaskAsync([FromBody] SequenceCreateModel model)
        {
            await this.seqMan.InsertSequenceTask(model);

            return Ok();
        }

        [HttpGet("get-task-sequence", Name = "GetTaskSequenceByIdAsync")]
        public async Task<ActionResult<SequenceViewDetailModel>> GetTaskSequenceByIdAsync([FromQuery] int taskId)
        {
            var data = await this.seqMan.GetTaskSequenceById(taskId);

            return Ok(data);
        }

        [HttpPost("update-task-sequence", Name = "UpdateTaskSequenceAsync")]
        public async Task<IActionResult> UpdateTaskSequenceAsync([FromBody] SequenceUpdateModel model, int taskId)
        {
            await this.seqMan.UpdateTaskSequenceAsync(model, taskId);

            return Ok();
        }
    }
}
