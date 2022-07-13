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
    [Route("api/v1/hot-spot")]
    public class TaskHotSpotApiController : Controller
    {
        private readonly TaskHotSpotService TaskHotSpotMan;

        public TaskHotSpotApiController(TaskHotSpotService taskHotSpotService)
        {
            this.TaskHotSpotMan = taskHotSpotService;
        }

        [HttpGet("get-task-hotspot-type-by-id/{id}")]
        public async Task<ActionResult<TaskTebakGambarTypesModel>> GetTaskHotSpotTypeById(int id)
        {
            var data = await this.TaskHotSpotMan.GetTaskHotSpotTypeById(id);
            return Ok(data);
        }

        [HttpGet("get-task-hotspot-choice-by-id/{id}")]
        public async Task<ActionResult<List<TaskHotSpotAnswerModel>>> GetTaskHotSpotChoiceById(int id)
        {
            var data = await this.TaskHotSpotMan.GetTaskHotSpotChoiceById(id);
            return Ok(data);
        }

        [HttpPost("insert-task-hotspot")]
        public async Task<ActionResult<bool>> InsertTaskHotspotAsync([FromBody]TaskHotSpotModel insert)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model state is not valid");
            }

            var data = await this.TaskHotSpotMan.InsertTaskHotspotAsync(insert);
            return Ok(data);
        }

        [HttpPost("update-task-hotspot/{id}")]
        public async Task<ActionResult<bool>> UpdateTaskHotspot(int id, [FromBody]TaskHotSpotModel insert)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model state is not valid");
            }

            var data = await this.TaskHotSpotMan.UpdateTaskHotspotAsync(id, insert);
            return Ok(data);
        }
    }
}
