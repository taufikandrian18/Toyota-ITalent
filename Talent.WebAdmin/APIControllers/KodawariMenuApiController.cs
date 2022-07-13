using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/KodawariMenus")]
    public class KodawariMenuApiController : Controller
    {
        private readonly KodawariMenuService KodawariMenuMan;

        public KodawariMenuApiController(KodawariMenuService kodawariMenuService)
        {
            this.KodawariMenuMan = kodawariMenuService;
        }
        [HttpGet("get-all-kodawari-menus")]
        public async Task<ActionResult<KodawariMenuPaginate>> GetAllKodawariMenuAsync([FromQuery] KodawariMenusGridFilter filter)
        {
            var data = await this.KodawariMenuMan.GetAllKodawariMenu(filter);
            return Ok(data);
        }
        [HttpPost("insert-kodawari-menu", Name = "InsertKodawariMenuAsync")]
        public async Task<ActionResult<BaseResponse>> InsertKodawariMenuAsync([FromBody] KodawariMenuCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KodawariMenuMan.InsertKodawariMenuAsync(model);

            return success;
        }
        [HttpGet("get-kodawari-menu-by-id", Name = "GetKodawariMenuByIdAsync")]
        public async Task<ActionResult<KodawariMenuModel>> GetKodawariMenuByIdAsync([FromQuery] Guid kodawariMenuId)
        {
            var data = await this.KodawariMenuMan.GetKodawariMenuById(kodawariMenuId);

            return Ok(data);
        }
        [HttpPost("update-kodawari-menu-by-id", Name = "UpdateKodawariMenuAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateKodawariMenuAsync([FromBody] KodawariMenuUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KodawariMenuMan.UpdateKodawariMenuAsync(model);

            return success;
        }
        [HttpDelete("delete-kodawari-menus", Name = "DeleteKodawariMenuAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteKodawariMenuAsync([FromBody] DeleteKodawariMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.KodawariMenuMan.DeleteKodawariMenuAsync(model);

            return getData;
        }
    }
}
