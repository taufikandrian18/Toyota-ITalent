using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ServiceTipMenus")]
    public class ServiceTipMenuApiController : Controller
    {
        private readonly ServiceTipMenuService ServiceTipMenuMan;

        public ServiceTipMenuApiController(ServiceTipMenuService serviceTipMenuService)
        {
            this.ServiceTipMenuMan = serviceTipMenuService;
        }

        [HttpGet("get-all-service-tip-menus")]
        public async Task<ActionResult<ServiceTipMenuPaginate>> GetAllProductCategoryAsync([FromQuery] ServiceTipMenuGridFilter filter)
        {
            var data = await this.ServiceTipMenuMan.GetAllServiceTipMenu(filter);
            return Ok(data);
        }
        [HttpPost("insert-service-tip-menu", Name = "InsertServiceTipMenuAsync")]
        public async Task<ActionResult<BaseResponse>> InsertServiceTipMenuAsync([FromBody] ServiceTipMenuCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ServiceTipMenuMan.InsertServiceTipMenuAsync(model);

            return success;
        }
        [HttpGet("get-service-tip-menu-by-id", Name = "GetServiceTipMenuByIdAsync")]
        public async Task<ActionResult<ServiceTipMenuModel>> GetServiceTipMenuByIdAsync([FromQuery] Guid serviceTipMenuId)
        {
            var data = await this.ServiceTipMenuMan.GetServiceTipMenuById(serviceTipMenuId);

            return Ok(data);
        }
        [HttpPost("update-service-tip-menu-by-id", Name = "UpdateServiceTipMenuAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateServiceTipMenuAsync([FromBody] ServiceTipMenuUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ServiceTipMenuMan.UpdateServiceTipMenuAsync(model);

            return success;
        }
        [HttpDelete("delete-service-tip-menu", Name = "DeleteServiceTipMenuAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteServiceTipMenuAsync([FromBody] DeleteServiceTipMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ServiceTipMenuMan.DeleteServiceTipMenuAsync(model);

            return getData;
        }
    }
}
