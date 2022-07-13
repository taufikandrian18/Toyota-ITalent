using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/Kodawaris")]
    public class KodawariApiController : Controller
    {
        private readonly KodawariService KodawariMan;

        public KodawariApiController(KodawariService kodawariService)
        {
            this.KodawariMan = kodawariService;
        }
        [HttpGet("get-all-kodawaris")]
        public async Task<ActionResult<KodawariPaginate>> GetAllKodawariAsync([FromQuery] KodawariGridFilter filter)
        {
            var data = await this.KodawariMan.GetAllKodawari(filter);
            return Ok(data);
        }
        [HttpPost("insert-kodawaris", Name = "InsertKodawariAsync")]
        public async Task<ActionResult<BaseResponse>> InsertKodawariAsync([FromBody] KodawariCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KodawariMan.InsertKodawariAsync(model);

            return success;
        }
        [HttpGet("get-kodawari-by-id", Name = "GetKodawariByIdAsync")]
        public async Task<ActionResult<KodawariModel>> GetKodawariByIdAsync([FromQuery] Guid kodawariId)
        {
            var data = await this.KodawariMan.GetKodawariById(kodawariId);

            return Ok(data);
        }
        [HttpPost("update-kodawari-by-id", Name = "UpdateKodawariAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateKodawariAsync([FromBody] KodawariUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KodawariMan.UpdateKodawariAsync(model);

            return success;
        }
        [HttpDelete("delete-kodawaris", Name = "DeleteKodawariAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteKodawariAsync([FromBody] DeleteKodawariModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.KodawariMan.DeleteKodawariAsync(model);

            return getData;
        }
    }
}
