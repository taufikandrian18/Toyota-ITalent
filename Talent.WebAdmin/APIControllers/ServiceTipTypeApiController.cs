using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ServiceTipTypes")]
    public class ServiceTipTypeApiController : Controller
    {
        private readonly ServiceTipTypeService ServiceTipTypeMan;

        public ServiceTipTypeApiController(ServiceTipTypeService serviceTipTypeService)
        {
            this.ServiceTipTypeMan = serviceTipTypeService;
        }
        [HttpGet("get-all-service-tip-types")]
        public async Task<ActionResult<ServiceTipTypePaginate>> GetAllServiceTipTypeAsync([FromQuery] ServiceTipTypeGridFilter filter)
        {
            var data = await this.ServiceTipTypeMan.GetAllServiceTipType(filter);
            return Ok(data);
        }
        [HttpPost("insert-service-tip-type", Name = "InsertServiceTipTypeAsync")]
        public async Task<ActionResult<BaseResponse>> InsertServiceTipTypeAsync([FromBody] ServiceTipTypeCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ServiceTipTypeMan.InsertServiceTipTypeAsync(model);

            return success;
        }
        [HttpGet("get-service-tip-type-by-id", Name = "GetServiceTipTypeByIdAsync")]
        public async Task<ActionResult<ServiceTipTypeModel>> GetServiceTipTypeByIdAsync([FromQuery] Guid serviceTipTypeId)
        {
            var data = await this.ServiceTipTypeMan.GetServiceTipTypeById(serviceTipTypeId);

            return Ok(data);
        }
        [HttpPost("update-service-tip-type-by-id", Name = "UpdateServiceTipTypeAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateKServiceTipTypeAsync([FromBody] ServiceTipTypeUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ServiceTipTypeMan.UpdateServiceTipTypeAsync(model);

            return success;
        }
        [HttpDelete("delete-service-tip-type", Name = "DeleteServiceTipTypeAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteServiceTipTypeAsync([FromBody] DeleteServiceTipTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ServiceTipTypeMan.DeleteServiceTipTypeAsync(model);

            return getData;
        }
    }
}
