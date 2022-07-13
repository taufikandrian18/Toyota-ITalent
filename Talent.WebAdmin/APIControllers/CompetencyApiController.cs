using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/competency")]
    public class CompetencyApiController : Controller
    {
        private readonly CompetencyService CompetencyMan;
        private readonly CompetencyJoinService CompetencyJoinMan;

        public CompetencyApiController(CompetencyService competencyService, CompetencyJoinService competencyJoinService)
        {
            this.CompetencyMan = competencyService;
            this.CompetencyJoinMan = competencyJoinService;
        }

        [HttpGet("get-all-join-competencies",Name = "get-all-join-competencies")]
        public async Task<ActionResult<CompetencyJoinViewModel>> GetAllJoinCompetencies([FromQuery] CompetencyFilter filter)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await this.CompetencyJoinMan.GetAllCompetencies(filter);
            return Ok(result);
        }

        [HttpGet("get-join-competency-by-id/{id}", Name = "get-join-competency-by-id")]
        public async Task<ActionResult<CompetencyJoinModel>> GetJoinCompetencyById(int id)
        {
            var result = await CompetencyJoinMan.GetCompetencyJoinById(id);
            return Ok(result);
        }

        [HttpGet("all", Name = "get-all-competencies-eagar")]
        public async Task<ActionResult<CompetencyJoinViewModel>> GetAllCompetencies()
        {
            var result = await this.CompetencyMan.GetAllCompetencies();
            return Ok(result);
        }

        [HttpPost("create", Name = "create-competency")]
        public async Task<ActionResult<int>> CreateCompetency([FromBody] CompetencyFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await CompetencyMan.CreateCompetency(model);
            return Ok(result);
        }

        [HttpPost("update", Name = "update-competency")]
        public async Task<ActionResult<bool>> UpdateCompetency([FromBody] CompetencyFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await CompetencyMan.UpdateCompetency(model);
            if (result == false)
            {
                return BadRequest("Data not available");
            }
            return Ok(result);
        }

        [HttpDelete("delete", Name = "delete-competency")]
        public async Task<ActionResult<bool>> DeleteCompetency([FromBody] int id)
        {
            var result = await CompetencyMan.DeleteCompetency(id);
            if (result == false)
            {
                return BadRequest("Data not available");
            }
            return Ok(result);
        }

        [HttpGet("get-all-competencies-async")]
        public async Task<ActionResult<List<CompetencyModel>>> GetAllCompetenciesAsync()
        {
            var data = await this.CompetencyMan.GetAllCompetenciesAsync();
            return Ok(data);
        }

        [HttpGet("get-competencies-by-id-async")]
        public async Task<ActionResult<CompetencyModel>> GetCompetenciesByIdAsync([FromQuery] int id)
        {
            var data = await this.CompetencyMan.GetCompetencyByIdAsync(id);
            return Ok(data);
        }
    }
}
