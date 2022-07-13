using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/matrix")]
    public class TaskMatrixTypeApiController : Controller
    {
        private readonly TaskMatrixTypeService TaskMatrixTypeMan;

        public TaskMatrixTypeApiController(TaskMatrixTypeService taskMatrixTypeService)
        {
            this.TaskMatrixTypeMan = taskMatrixTypeService;
        }

        [HttpGet("get-task-matrix-type-by-id/{id}", Name = "get-task-matrix-type-by-id")]
        public async Task<ActionResult<TaskMatrixTypeFormModel>> GetTaskMatrixTypeById(int id)
        {
            var result = await TaskMatrixTypeMan.GetTaskMatrixTypeById(id);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<int>> CreateTaskMatrixType([FromBody] TaskMatrixTypeFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await TaskMatrixTypeMan.CreateTaskMatrixType(model);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<ActionResult<bool>> UpdateTaskMatrixType([FromBody] TaskMatrixTypeFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var result = await TaskMatrixTypeMan.UpdateTaskMatrixType(model);
            if (result == false)
            {
                return BadRequest("Data not available");
            }
            return Ok(result);
        }
    }
}
