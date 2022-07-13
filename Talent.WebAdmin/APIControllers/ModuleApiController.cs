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
    [Route("api/v1/modules")]
    public class ModuleApiController : Controller
    {
        private readonly ModuleService moduleMan;

        public ModuleApiController(ModuleService moduleService)
        {
            this.moduleMan = moduleService;
        }

        [HttpGet("get-all-module", Name = "GetModuleTableAsync")]
        public async Task<ActionResult<ModuleGridModel>> GetModuleAsync([FromQuery] ModuleGridFilter filter)
        {
            var data = await this.moduleMan.GetAllModulesAsync(filter);

            return Ok(data);
        }

        [HttpPost("insert-module", Name = "InsertModuleAsync")]
        public async Task<ActionResult<bool>> InsertModuleAsync([FromBody] ModuleCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.moduleMan.InsertModuleAsync(model);

            return Ok(success);
        }
        
        [HttpGet("get-module-by-id", Name = "GetModuleByIdAsync")]
        public async Task<ActionResult<ModuleViewDetailModel>> GetModuleByIdAsync([FromQuery] int moduleId)
        {
            var data = await this.moduleMan.GetModuleById(moduleId);

            return Ok(data);
        }

        [HttpPost("update-module-by-id", Name = "UpdateModuleAsync")]
        public async Task<ActionResult<bool>> UpdateModuleAsync([FromBody] ModuleUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.moduleMan.UpdateModuleAsync(model);

            return Ok(success);
        }

        [HttpDelete("delete-module", Name = "DeleteModuleAsync")]
        public async Task<ActionResult> DeleteModuleAsync([FromBody] DeleteModuleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await this.moduleMan.DeleteModuleAsync(model);

            return Ok();
        }
    }
}
