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
    [Route("api/v1/checklist")]
    public class TaskChecklistApiController : Controller
    {
        private readonly ChecklistService checkMan;
        private readonly TaskService taskMan;

        public TaskChecklistApiController(ChecklistService checklistService, TaskService taskService)
        {
            this.checkMan = checklistService;
            this.taskMan = taskService;
        }

        [HttpGet("get-task-code", Name = "GetTaskCodeChecklistAsync")]
        public async Task<ActionResult<List<CompetencyMappingJoinModel>>> GetTaskCodeAsync()
        {
            var data = await this.taskMan.GetAllTaskCode();

            return Ok(data);
        }

        [HttpGet("get-module", Name = "GetModuleAsync")]
        public async Task<ActionResult<List<ModuleForTaskModel>>> GetModuleAsync()
        {
            var data = await this.taskMan.GetAllModules();

            return Ok(data);
        }

        [HttpGet("get-time-point", Name = "GetTimePointAsync")]
        public async Task<ActionResult<List<TimePointTaskModel>>> GetTimePointAsync()
        {
            var data = await this.taskMan.GetAllTaskTimePoints();

            return Ok(data);
        }

        [HttpGet("get-number", Name = "GetNumberAsync")]
        public async Task<ActionResult<int>> GetNumberAsync([FromQuery] GetNumberTaskModel model)
        {
            var data = await this.taskMan.GetNumber(model);

            return Ok(data);
        }

        [HttpPost("insert-checklist-task", Name = "InsertChecklistTaskAsync")]
        public async Task<ActionResult> InsertChecklistTaskAsync([FromBody] ChecklistCreateModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            await this.checkMan.InsertChecklistTask(model);

            return Ok();
        }

        [HttpGet("get-task-checklist", Name = "GetChecklistTaskByIdAsync")]
        public async Task<ActionResult<ChecklistViewDetailModel>> GetChecklistTaskByIdAsync([FromQuery] int taskId)
        {
            var data = await this.checkMan.GetChecklistTaskById(taskId);

            return Ok(data);
        }

        [HttpPost("update-checklist-task", Name = "UpdateChecklistTaskAsync")]
        public async Task<ActionResult> UpdateChecklistTaskAsync([FromBody] ChecklistUpdateModel model, int taskId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            await this.checkMan.UpdateChecklistTask(model, taskId);

            return Ok();
        }
    }
}
