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
    [Route("api/master/position-map")]
    public class PositionMappingApiController : Controller
    {
        private readonly PositionMappingService ServiceMan;
        public PositionMappingApiController(PositionMappingService service)
        {
            this.ServiceMan = service;
        }

        // GET: api/<controller>
        [HttpGet("get-detail-mapping/{id}", Name = "Get-Detail-Position-Mapping-Async")]
        public async Task<PositionMapFormModel> GetDetailPositionMappingAsync(int id)
        {
            var getData = await ServiceMan.getDetailPosMappingAsync(id);
            return getData;
        }

        [HttpGet("get-detail/{positionId}", Name = "Get-Detail-Position-Competency-Async")]
        public async Task<PositionMapFormModel> GetDetailPositionCompetencyAsync(int positionId)
        {
            var getData = await ServiceMan.getDetailPositionCompetencyAsync(positionId);
            return getData;
        }

        [HttpGet("get-list/{positionId}", Name = "Get-Competency-List-Async")]
        public async Task<List<DropDownModel>> GetCompetencyListAsync(int positionId)
        {
            var getData = await ServiceMan.GetCompetencyListAsync(positionId);
            return getData;
        }

        [HttpPost("get-paginate", Name = "Get-Position-Mapping-Paginate-Async")]
        public async Task<ResponsePositionMapModel> GetPositionMappingPaginateAsync([FromBody] PositionMapFilterModel filter)
        {
            var getData = await ServiceMan.GetPositionMapPaginateAsync(filter);
            return getData;
        }

        [HttpPost("insert", Name = "Insert-Position-Mapping-Async")]
        public async Task<string> InsertPositionMappingAsync([FromBody] PositionMapFormModel model)
        {
            var response = await ServiceMan.InsertPosMappingAsync(model);
            return response;
        }

        [HttpPost("update", Name = "Update-Position-Mapping-Async")]
        public async Task<string> UpdatePositionMappingAsync([FromBody] PositionMapFormModel model)
        {
            var response = await ServiceMan.UpdatePosMappingAsync(model);
            return response;
        }

        [HttpDelete("delete/{id}", Name = "Delete-Position-Mapping-Async")]
        public async Task<string> DeletePositionMappingAsync(int id)
        {
            var response = await ServiceMan.DeletePosMappingAsync(id);
            return response;
        }

        [HttpDelete("delete-model", Name = "Delete-Position-Mapping-Model-Async")]
        public async Task<string> DeletePositionMappingModelAsync([FromBody] PositionMapDeleteModel deleteModel)
        {
            var response = await ServiceMan.DeletePositionCompetencyAsync(deleteModel);
            return response;
        }
    }
}
