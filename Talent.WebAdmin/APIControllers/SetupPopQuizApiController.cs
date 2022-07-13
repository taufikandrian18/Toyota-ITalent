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
    [Route("api/v1/setup-pop-quiz")]
    public class SetupPopQuizApiController : Controller
    {
        private SetupPopQuizService popMan;

        public SetupPopQuizApiController(SetupPopQuizService service)
        {
            this.popMan = service;
        }

        [HttpGet("get-competency", Name = "GetCompetencySetupAsync")]
        public async Task<ActionResult<List<CompetencySetupModel>>> GetCompetencyAsync([FromQuery] string name)
        {
            var data = await this.popMan.GetCompetencyOptionAsync(name);

            return Ok(data);
        }

        [HttpGet("get-module", Name = "GetModuleSetupAsync")]
        public async Task<ActionResult<List<ModuleSetupModel>>> GetModuleAsync([FromQuery] string name)
        {
            var data = await this.popMan.GetModuleOptionAsync(name);

            return Ok(data);
        }

        [HttpGet("get-task-code", Name = "GetTaskCodeSetupAsync")]
        public async Task<ActionResult<List<TaskCodeSetupModel>>> GetTaskCodeAsync([FromQuery] TaskCodeFilterModel filter)
        {
            var data = await this.popMan.GetTaskCodeOptionAsync(filter);

            return Ok(data);
        }

        [HttpPost("insert-pop-quiz")]
        public async Task<ActionResult<bool>> InsertPopQuizAsync([FromBody] SetupPopUpQuizCreateModel model)
        {
            var success = await this.popMan.InsertPopUpQuizAsync(model);

            return Ok(success);
        }

        [HttpGet("get-pop-quiz-detail", Name = "GetPopQuizDetailAsync")]
        public async Task<ActionResult<SetupPopQuizDetailModel>> GetPopQuizDetailAsync([FromQuery] int popQuizId)
        {
            var data = await this.popMan.GetPopQuizDetailAsync(popQuizId);

            return Ok(data);
        }

        [HttpPost("edit-pop-quiz")]
        public async Task<ActionResult<bool>> UpdatePopQuizAsync([FromBody] SetupPopQuizUpdateModel model)
        {
            var success = await this.popMan.UpdatePopQuizAsync(model);

            return Ok(success);
        }

        [HttpDelete("remove-pop-quiz/{id}")]
        public async Task<ActionResult<bool>> RemovePopQuiz(int id)
        {
            var data = await this.popMan.RemovePopQuiz(id);
                return data;
        }
    }
}
