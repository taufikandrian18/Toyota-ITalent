using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/matching")]
    public class TaskMatchingTypeApiController : Controller
    {
        private readonly TaskMatchingTypeService TaskMatchingTypeMan;

        public TaskMatchingTypeApiController(TaskMatchingTypeService taskMatchingTypeService)
        {
            this.TaskMatchingTypeMan = taskMatchingTypeService;
        }

        [HttpGet("get-task-matching-type-by-id/{id}", Name = "get-task-matching-type-by-id")]
        public async Task<ActionResult<TaskMatchingTypeFormModel>> GetTaskMatchingTypeById(int id)
        {
            var result = await TaskMatchingTypeMan.GetTaskMatchingTypeById(id);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<int>> CreateTaskMatchingType([FromBody] TaskMatchingTypeFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await TaskMatchingTypeMan.CreateTaskMatchingType(model);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<ActionResult<bool>> UpdateTaskMatchingType([FromBody] TaskMatchingTypeFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await TaskMatchingTypeMan.UpdateTaskMatchingType(model);
            if (result == false)
            {
                return BadRequest("Data not available");
            }
            return Ok(result);
        }
    }
}
