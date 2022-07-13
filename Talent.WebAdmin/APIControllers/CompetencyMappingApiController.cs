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
    [Route("api/v1/competency-mapping")]
    public class CompetencyMappingApiController : Controller
    {
        private readonly CompetencyMappingService _CompetencyMappingMan;

        public CompetencyMappingApiController(CompetencyMappingService competencyMappingService)
        {
            this._CompetencyMappingMan = competencyMappingService;
        }
        // GET: api/<controller>
        [HttpGet("get-all-competency-mapping-paginate-async")]
        public async Task<ActionResult<CompetencyMappingPaginate>> GetAllCompetencyMappingPaginateAsync([FromQuery] CompetencyMappingGridFilter filter)
        {
            //if(ModelState.IsValid == false)
            //{
            //    return BadRequest();
            //}
            var data = await this._CompetencyMappingMan.GetAllCompetencyMappingPaginateAsync(filter);
            return Ok(data);

        }

        [HttpGet("get-all-competency-mapping-by-competency-id-and-evaluation-id-paginate-async")]
        public async Task<ActionResult<CompetencyMapping>> GetAllCompetencyMappingByCompetencyIdandEvaluationIdPaginateAsync([FromQuery] CompetencyMappingGetDataGridFilter filter)
        {
            var data = await this._CompetencyMappingMan.GetAllCompetencyMappingByCompetencyIdandEvaluationIdPaginateAsync(filter);
            return Ok(data);
        }


        [HttpPost("insert-competency-mapping-async")]
        public async Task<ActionResult<bool>> InsertCompetencyMappingAsync([FromBody] CompetencyMappingInsertModel insert)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            var data = await this._CompetencyMappingMan.InsertCompetencyMappingAsync(insert);

            return Ok(data);
        }

        [HttpPost("update-competency-mapping-async")]
        public async Task<ActionResult<bool>> UpdateCompetencyMappingAsync([FromQuery]CompetencyMappingGetDataGridFilter filter, [FromBody]CompetencyMappingInsertModel update)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            var isSuccess = await this._CompetencyMappingMan.UpdateCompetencyMappingAsync(filter, update);
            //if(isSuccess == false)
            //{
            //    return BadRequest("Not found");
            //}
            return Ok(isSuccess);
        }

        [HttpDelete("delete-competency-mapping-async")]
        public async Task<ActionResult<bool>> DeleteCompetencyMappingAsync([FromQuery] CompetencyMappingGetDataGridFilter filter)
        {
            var data = await this._CompetencyMappingMan.DeleteCompetencyMappingAsync(filter);
            return Ok(data);
        }

    }
}
