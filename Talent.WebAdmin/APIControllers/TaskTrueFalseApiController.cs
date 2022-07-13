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
    [Route("api/v1/true-false")]
    public class TaskTrueFalseApiController : Controller
    {

        private readonly TaskTrueFalseService TaskTrueFalseMan;
        public TaskTrueFalseApiController(TaskTrueFalseService taskTrueFalseService)
        {
            this.TaskTrueFalseMan = taskTrueFalseService;
        }

        [HttpGet("get-task-true/{id}")]
        public async Task<ActionResult<TrueFalseTypeViewDetails>> GetTrueFalseTaskById(int id)
        {
            var result = await this.TaskTrueFalseMan.GetTaskTruFalseTypeById(id);
            return result;
        }


        [HttpPost("insert-true-false")]
        public async Task<ActionResult<bool>> InsertTrueFalseTypes([FromBody] TaskTrueFalseFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var isSuccess = await this.TaskTrueFalseMan.InsertTrueFalse(model);

            return Ok();
        }

        [HttpPost("update-true-false")]
        public async Task<ActionResult<bool>> UpdateTrueFalseTask([FromBody] TaskTrueFalseFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }
            var result = await this.TaskTrueFalseMan.UpdateTaskTrueFalse(model);

            if (result == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


    }
}
