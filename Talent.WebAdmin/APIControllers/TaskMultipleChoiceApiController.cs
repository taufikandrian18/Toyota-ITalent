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
    [Route("api/v1/multiple-choice")]
    public class TaskMultipleChoiceApiController : Controller
    {
        private readonly TaskMultipleChoiceService TaskMultipleChoiceMan;

        public TaskMultipleChoiceApiController(TaskMultipleChoiceService taskMultipleChoiceService)
        {
            this.TaskMultipleChoiceMan = taskMultipleChoiceService;
        }

        [HttpGet("get-task-multiple-choice-type-by-id/{id}")]
        public async Task<ActionResult<TaskMultipleChoiceTypeModel>> GetTaskMultipleChoiceTypeById(int id)
        {
            var data = await this.TaskMultipleChoiceMan.GetTaskMultipleChoiceTypeById(id);
            return Ok(data);
        }

        [HttpGet("get-task-multiple-choice-choice-by-id/{id}")]
        public async Task<ActionResult<List<TaskMultipleChoiceTypeModel>>> GetTaskMultipleChoiceChoiceById(int id)
        {
            var data = await this.TaskMultipleChoiceMan.GetTaskMultipleChoiceChoiceById(id);
            return Ok(data);
        }

        // POST api/<controller>
        [HttpPost("insert-task-multiple-choice")]
        public async Task<ActionResult<bool>> InsertTaskMultipleChoice([FromBody]TaskMultipleChoiceModel insert)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            var data = await this.TaskMultipleChoiceMan.InsertTaskMultipleChoice(insert);
            return Ok(true);
        }

        [HttpPost("update-task-multiple-choice/{id}")]
        public async Task<ActionResult<bool>> UpdateTaskMultipleChoice(int id, [FromBody]TaskMultipleChoiceModel insert)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            var data = await this.TaskMultipleChoiceMan.UpdateTaskMultipleChoice(id, insert);
            return Ok(true);
        }
    }
}
