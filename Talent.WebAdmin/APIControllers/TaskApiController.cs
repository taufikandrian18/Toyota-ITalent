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
   [Route("api/v1/task")]
   public class TaskApiController : Controller
   {
       private readonly TaskService TaskMan;

       public TaskApiController(TaskService taskService)
       {
           this.TaskMan = taskService;
       }
       [HttpGet("get-all-competencies")]
       public async Task<ActionResult<List<CompetencyMappingJoinModel>>> GetAllCompentencies()
       {
           var result = await this.TaskMan.GetAllTaskCode();
           return Ok(result);
       }

       [HttpGet("get-task-code-by-task-id")]
       public async Task<ActionResult<CompetencyMappingJoinModel>> GetTaskCodeByTaskId(int id)
       {
           var result = await this.TaskMan.GetTaskCodeByTaskId(id);
           return Ok(result);
       }

       [HttpGet("get-all-modules-for-task")]
       public async Task<ActionResult<List<ModuleForTaskModel>>> GetAllModulesForTask()
       {
           var result = await this.TaskMan.GetAllModules();
           return Ok(result);
       }

       [HttpGet("get-all-time-points-for-task")]
       public async Task<ActionResult<List<TimePointTaskModel>>> GetAllTimePointsForTask()
       {
           var result = await this.TaskMan.GetAllTaskTimePoints();
           return Ok(result);
       }

        [HttpGet("get-all-time-points-for-task-update")]
        public async Task<ActionResult<List<TimePointTaskModel>>> GetAllTimePointsForTaskUpdate(List<int> id)
        {
            var result = await this.TaskMan.GetAllTaskTimePointsUpdate(id);
            return Ok(result);
        }

        [HttpGet("get-number")]
       public async Task<ActionResult<int>> GetNumber(GetNumberTaskModel task)
       {
           var result = await this.TaskMan.GetNumber(task);
           return Ok(result);
       }

        [HttpGet("get-task-by-id/{id}")]
        public async Task<ActionResult<TaskInsertModel>> GetTaskById(int id)
        {
            var result = await this.TaskMan.GetTaskById(id);
            return Ok(result);
        }
        [HttpGet("get-view-data", Name = "get-task-view-data")]
        public async Task<ActionResult<TaskPaginationModel>> GetTaskPaginatedData(TaskPagingModel filter)
        {
            var data = await this.TaskMan.GetAllTaskPaginateAsync(filter);
            return Ok(data);
        }

        [HttpDelete("delete-task", Name = "DeleteTaskByIdAsync")]
        public async Task<ActionResult> DeleteAsync([FromQuery] int taskId)
        {
            var isSuccess = await this.TaskMan.DeleteTaskByIdAsync(taskId);

            if (isSuccess == false)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
