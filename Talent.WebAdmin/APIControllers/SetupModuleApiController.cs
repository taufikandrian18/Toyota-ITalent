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
    [Route("api/v1/setup-module")]
    public class SetupModuleApiController : Controller
    {
        private readonly SetupModuleService SetupModuleMan;

        public SetupModuleApiController(SetupModuleService setupModuleService)
        {
            this.SetupModuleMan = setupModuleService;
        }

        [HttpGet("get-all-module")]
        public async Task<ActionResult<List<SetupModuleModuleViewModel>>> GetModules([FromQuery]string moduleNameSearch)
        {
            var result = await this.SetupModuleMan.GetModulesForSetupModule(moduleNameSearch);
            return Ok(result);
        }

        [HttpGet("get-all-module-for-update/{moduleNameSearch}")]
        public async Task<ActionResult<List<SetupModuleModuleViewModel>>> GetModulesForUpdate(string moduleNameSearch)
        {
            var result = await this.SetupModuleMan.GetAllModulesForSetupModuleUpdate(moduleNameSearch);
            return Ok(result);
        }


        [HttpGet("get-all-time-points")]
        public async Task<ActionResult<List<TimePointTaskModel>>> GetAllTimePointsForModule()
        {
            var result = await this.SetupModuleMan.GetAllLearningTimePoints();

            return Ok(result);
        }

        [HttpGet("get-all-competencies-setup-module/{name}")]
        public async Task<ActionResult<List<CompetencyMappingJoinModel>>> GetAllCompetencies(string name)
        {
            var result = await this.SetupModuleMan.GetAllCompetencies(name);

            return Ok(result);
        }

        [HttpGet("get-all-module-from-task/{moduleNameSearch}", Name = "get-all-module-from-task")]
        public async Task<ActionResult<List<SetupModuleModuleViewModel>>> GetModuleFromTask(string moduleNameSearch)
        {
            var result = await this.SetupModuleMan.GetModuleTaskByName(moduleNameSearch);

            return Ok(result);
        }

        [HttpGet("get-all-task-code/{taskCode}")]
        public async Task<ActionResult<List<TaskForSetupModuleModel>>> GetAllTaskCode(string taskCode)
        {
            var result = await this.SetupModuleMan.GetAllTaskCode(taskCode);

            return Ok(result);
        }

        [HttpGet("get-all-task-code-filtered")]
        public async Task<ActionResult<List<TaskForSetupModuleModel>>> GetAllTaskCodeFiltered([FromQuery] TaskCodeFilteredFormModel model)
        {
            var result = await this.SetupModuleMan.GetAllTaskCodeWithFilter(model);
            return Ok(result);
        }

        [HttpGet("get-all-competency-filtered/{competencyName}")]
        public async Task<ActionResult<List<CompetencyMappingJoinModel>>> GetAllCompetencyFiltered(string competencyName)
        {
            var result = await this.SetupModuleMan.GetCompetencyAutoSuggest(competencyName);
            return Ok(result);
        }

        [HttpGet("genereta-task/{totalTask}")]
        public async Task<ActionResult<List<TaskForSetupModuleModel>>> GenerateTaskCode(int totalTask, [FromQuery] TaskCodeFilteredFormModel model)
        {
            var result = await this.SetupModuleMan.GenerateTaskCode(totalTask, model);
            return Ok(result);
        }

        [HttpPost("insert-setup-module", Name = "insert-setup-module")]
        public async Task<ActionResult<bool>> InsertSetupModule([FromBody] SetupModuleFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model State is not valid");
            }

            var result = await this.SetupModuleMan.InsertSetupModule(model);
            if (result == false)
            {
                return BadRequest("Failed to insert");
            }
            return Ok(result);
        }

        [HttpGet("get-setup-module-update/{id}")]
        public async Task<ActionResult<SetupModuleFormModel>> GetSetupUpdateFormModel(int id)
        {
            var result = await this.SetupModuleMan.GetSetupModuleFormModel(id);
            return Ok(result);
        }

        [HttpPost("update-setup-module")]
        public async Task<ActionResult<bool>> UpdateSetupModule([FromBody] SetupModuleFormModel model)
        {
            var result = await this.SetupModuleMan.UpdateSetupModule(model);

            if (result == false)
            {
                return BadRequest("Something went wrong");
            }

            return Ok(result);
        }

        [HttpDelete("delete-setup-module/{id}", Name = "delete-setup-module")]
        public async Task<ActionResult<bool>> DeleteSetupModule(int id)
        {
            var result = await this.SetupModuleMan.RemoveSetupModule(id);

            if (result == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
