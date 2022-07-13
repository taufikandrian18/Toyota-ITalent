using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/competency-type")]
    public class CompetencyTypeApiController : Controller
    {
        private readonly CompetencyTypeService CompetencyTypeMan;

        public CompetencyTypeApiController(CompetencyTypeService competencyTypeService)
        {
            this.CompetencyTypeMan = competencyTypeService;
        }

        [HttpGet("get-all-competency-types",Name = "get-all-competency-types")]
        public async Task<ActionResult<CompetencyTypeViewModel>> GetAllCompetencyTypes()
        {
            var result = await this.CompetencyTypeMan.GetAllCompetencyTypes();
            return Ok(result);
        }

        [HttpGet("get-competency-type-by-id/{id}", Name = "get-competency-type-by-id")]
        public async Task<ActionResult<CompetencyTypeModel>> GetCompetencyTypeById(int id)
        {
            var result = await CompetencyTypeMan.GetCompetencyTypeById(id);
            return Ok(result);
        }
    }
}
