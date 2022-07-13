using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ServiceTips")]
    public class ServiceTipApiController : Controller
    {
        private readonly ServiceTipService ServiceTipMan;

        public ServiceTipApiController(ServiceTipService serviceTipService)
        {
            this.ServiceTipMan = serviceTipService;
        }
        [HttpGet("get-all-service-tips")]
        public async Task<ActionResult<ServiceTipPaginate>> GetAllServiceTipAsync([FromQuery] ServiceTipGridFilter filter)
        {
            var data = await this.ServiceTipMan.GetAllServiceTips(filter);
            return Ok(data);
        }
        [HttpPost("insert-service-tips", Name = "InsertServiceTipAsync")]
        public async Task<ActionResult<BaseResponse>> InsertServiceTipAsync([FromBody] ServiceTipCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ServiceTipMan.InsertServiceTipAsync(model);

            return success;
        }
        [HttpGet("get-service-tip-by-id", Name = "GetServiceTipByIdAsync")]
        public async Task<ActionResult<ServiceTipModel>> GetServiceTipByIdAsync([FromQuery] Guid serviceTipId)
        {
            var data = await this.ServiceTipMan.GetServiceTipById(serviceTipId);

            return Ok(data);
        }
        [HttpPost("update-service-tip-by-id", Name = "UpdateServiceTipAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateServiceTipAsync([FromBody] ServiceTipUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ServiceTipMan.UpdateServiceTipAsync(model);

            return success;
        }
        [HttpDelete("delete-service-tips", Name = "DeleteServiceTipAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteServiceTipAsync([FromBody] DeleteServiceTipModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ServiceTipMan.DeleteServiceTipAsync(model);

            return getData;
        }
    }
}
