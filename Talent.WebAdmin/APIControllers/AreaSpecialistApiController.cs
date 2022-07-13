using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/AreaSpecialists")]
    public class AreaSpecialistApiController : Controller
    {
        private readonly AreaSpecialistService AreaSpecialistMan;

        public AreaSpecialistApiController(AreaSpecialistService areaSpecialistService)
        {
            this.AreaSpecialistMan = areaSpecialistService;
        }

        [HttpGet("get-all-area-specialists")]
        public async Task<ActionResult<AreaSpecialistPaginate>> GetAllAreaSpecialistAsync([FromQuery] AreaSpecialistGridFilter filter)
        {
            var data = await this.AreaSpecialistMan.GetAllAreaSpecialist(filter);
            return Ok(data);
        }
        [HttpPost("insert-area-specialist", Name = "InsertAreaSpecialistAsync")]
        public async Task<ActionResult<bool>> InsertAreaSpecialistAsync([FromBody] AreaSpecialistCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.AreaSpecialistMan.InsertAreaSpecialistAsync(model);

            return Ok(success);
        }
        [HttpGet("get-area-specialist-by-id", Name = "GetAreaSpecialistByIdAsync")]
        public async Task<ActionResult<AreaSpecialistModel>> GetAreaSpecialistByIdAsync([FromQuery] string areaSpecialistId)
        {
            var data = await this.AreaSpecialistMan.GetAreaSpecialistById(areaSpecialistId);

            return Ok(data);
        }
        [HttpPost("update-area-specialist-by-id", Name = "UpdateAreaSpecialistAsync")]
        public async Task<ActionResult<bool>> UpdateAreaSpecialistAsync([FromBody] AreaSpecialistUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.AreaSpecialistMan.UpdateAreaSpecialistAsync(model);

            return Ok(success);
        }
        [HttpDelete("delete-area-specialist", Name = "DeleteAreaSpecialistAsync")]
        public async Task<ActionResult> DeleteAreaSpecialistAsync([FromBody] DeleteAreaSpecialistModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await this.AreaSpecialistMan.DeleteAreaSpecialistAsync(model);

            return Ok();
        }
    }
}
