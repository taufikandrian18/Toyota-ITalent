using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/program-type")]
    public class ProgramTypeApiController : Controller
    {
        private readonly ProgramTypeService ProgramTypeMan;

        public ProgramTypeApiController(ProgramTypeService programTypeService)
        {
            this.ProgramTypeMan = programTypeService;
        }

        [HttpGet("get-all-program-types", Name = "get-all-program-types")]
        public async Task<ActionResult<ProgramTypeViewModel>> GetAllProgramTypes()
        {
            var result = await this.ProgramTypeMan.GetAllProgramTypes();
            return Ok(result);
        }

        [HttpGet("get-program-type-by-id/{id}", Name = "get-program-type-by-id")]
        public async Task<ActionResult<ProgramTypeModel>> GetProgramTypeById(int id)
        {
            var result = await ProgramTypeMan.GetProgramTypeById(id);
            return Ok(result);
        }
    }
}
