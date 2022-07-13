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
    [Route("api/v1/short-answer")]
    public class TaskShortAnswerApiController : Controller
    {
        private readonly TaskShortAnswerService TaskShortAnswerMan;

        public TaskShortAnswerApiController(TaskShortAnswerService taskShortAnswerService)
        {
            this.TaskShortAnswerMan = taskShortAnswerService;
        }

        [HttpPost("insert-short-answer")]
        public async Task<ActionResult<bool>> InsertTaskShortAnswer([FromBody] TaskShortAnswerForm model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model state is not valid");
            }

            var result = await this.TaskShortAnswerMan.InsertShortAnswer(model);

            return Ok(result);
        }

        [HttpGet("get-short-answer-details/{id}")]
        public async Task<ActionResult<TaskShortAnswerViewDetail>> GetTaskShortAnswerDetail(int id)
        {
            var result = await this.TaskShortAnswerMan.GetTaskShortAnswerDetail(id);

            return Ok(result);
        }

        [HttpPost("update-short-answer")]
        public async Task<ActionResult<bool>> UpdateTaskShortAnswer([FromBody] TaskShortAnswerForm model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model state is not valid");
            }

            var result = await this.TaskShortAnswerMan.UpdateTaskShortAnswer(model);

            return Ok(result);
        }

    }
}
