using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/essay")]
    public class TaskEssayTypeApiController : Controller
    {
        private readonly TaskEssayTypeService TaskEssayTypeMan;

        public TaskEssayTypeApiController(TaskEssayTypeService taskEssayTypeService)
        {
            this.TaskEssayTypeMan = taskEssayTypeService;
        }

        [HttpGet("get-task-essay-type-by-id/{id}", Name = "get-task-essay-type-by-id")]
        public async Task<ActionResult<TaskEssayTypeFormModel>> GetTaskEssayTypeById(int id)
        {
            var result = await TaskEssayTypeMan.GetTaskEssayTypeById(id);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<int>> CreateTaskEssayType([FromBody] TaskEssayTypeFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await TaskEssayTypeMan.CreateTaskEssayType(model);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<ActionResult<bool>> UpdateTaskEssayType([FromBody] TaskEssayTypeFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await TaskEssayTypeMan.UpdateTaskEssayType(model);
            if (result == false)
            {
                return BadRequest("Data not available");
            }
            return Ok(result);
        }
    }
}
