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
    [Route("api/v1/rating")]
    public class TaskRatingApiController : Controller
    {
        private readonly TaskRatingService TaskRatingMan;

        public TaskRatingApiController(TaskRatingService taskRatingService)
        {
            this.TaskRatingMan = taskRatingService;
        }

        [HttpPost("insert-task-rating")]
        public async Task<ActionResult<bool>> InsertTaskRating([FromBody] TaskRatingModel insert)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            var data = await this.TaskRatingMan.InsertTaskRatingAsync(insert);
            return Ok(data);
        }

        [HttpPost("update-task-rating/{id}")]
        public async Task<ActionResult<bool>> UpdateTaskRating(int id, [FromBody] TaskRatingModel insert)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            var data = await this.TaskRatingMan.UpdateTaskRating(id, insert);
            return Ok(data);
        }

        [HttpGet("get-task-rating-type-by-id/{id}")]
        public async Task<ActionResult<TaskRatingTypeModel>> GetTaskRatingTypeById(int id)
        {
            var data = await this.TaskRatingMan.GetTaskRatingTypeById(id);
            return Ok(data);
        }

        [HttpGet("get-task-rating-choice-by-id/{id}")]
        public async Task<ActionResult<List<TaskRatingTypeModel>>> GetTaskRatingChoiceById(int id)
        {
            var data = await this.TaskRatingMan.GetTaskRatingChoiceById(id);
            return Ok(data);
        }
        [HttpDelete("delete-task-rating-async/{id}")]
        public async Task<ActionResult<bool>> DeleteTaskRatingAsync(int id)
        {
            var data = await this.TaskRatingMan.DeleteTaskRatingAsync(id);
            return Ok(data);
        }
    }
}
