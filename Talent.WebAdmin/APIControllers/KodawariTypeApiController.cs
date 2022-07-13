using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/KodawariTypes")]
    public class KodawariTypeApiController : Controller
    {
        private readonly KodawariTypeService KodawariTypeMan;

        public KodawariTypeApiController(KodawariTypeService kodawariTypeService)
        {
            this.KodawariTypeMan = kodawariTypeService;
        }
        [HttpGet("get-all-kodawari-types")]
        public async Task<ActionResult<KodawariTypePaginate>> GetAllKodawariTypeAsync([FromQuery] KodawariTypeGridFilter filter)
        {
            var data = await this.KodawariTypeMan.GetAllKodawariType(filter);
            return Ok(data);
        }
        [HttpPost("insert-kodawari-type", Name = "InsertKodawariTypeAsync")]
        public async Task<ActionResult<BaseResponse>> InsertKodawariTypeAsync([FromBody] KodawariTypeCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KodawariTypeMan.InsertKodawariTypeAsync(model);

            return success;
        }
        [HttpGet("get-kodawari-type-by-id", Name = "GetKodawariTypeByIdAsync")]
        public async Task<ActionResult<KodawariTypeModel>> GetKodawariTypeByIdAsync([FromQuery] Guid kodawariTypeId)
        {
            var data = await this.KodawariTypeMan.GetKodawariTypeById(kodawariTypeId);

            return Ok(data);
        }
        [HttpPost("update-kodawari-type-by-id", Name = "UpdateKodawariTypeAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateKodawariTypeAsync([FromBody] KodawariTypeUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KodawariTypeMan.UpdateKodawariTypeAsync(model);

            return success;
        }
        [HttpDelete("delete-kodawari-types", Name = "DeleteKodawariTypeAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteKodawariTypeAsync([FromBody] DeleteKodawariTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.KodawariTypeMan.DeleteKodawariTypeAsync(model);

            return getData;
        }
    }
}
