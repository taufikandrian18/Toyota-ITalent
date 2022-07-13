using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/CmsMenus")]
    public class CmsMenuApiController : Controller
    {
        private readonly CmsMenuService CmsMenuMan;

        public CmsMenuApiController(CmsMenuService cmsMenuService)
        {
            this.CmsMenuMan = cmsMenuService;
        }

        [HttpGet("get-all-cms-menus")]
        public async Task<ActionResult<CmsMenuPaginate>> GetAllCmsMenuAsync([FromQuery] CmsMenuGridFilter filter)
        {
            var data = await this.CmsMenuMan.GetAllCmsMenu(filter);
            return Ok(data);
        }
        [HttpPost("insert-cms-menu", Name = "InsertCmsMenuAsync")]
        public async Task<ActionResult<BaseResponse>> InsertCmsMenuAsync([FromBody] CmsMenuCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.CmsMenuMan.InsertCmsMenuAsync(model);

            return success;
        }
        [HttpGet("get-cms-menu-by-id", Name = "GetCmsMenuByIdAsync")]
        public async Task<ActionResult<CmsMenuModel>> GetCmsMenuByIdAsync([FromQuery] Guid Cms_MenuId)
        {
            var data = await this.CmsMenuMan.GetCmsMenuById(Cms_MenuId);

            return Ok(data);
        }
        [HttpPost("update-cms-menu-by-id", Name = "UpdateCmsMenuAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateCmsMenuAsync([FromBody] CmsMenuUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.CmsMenuMan.UpdateCmsMenuAsync(model);

            return success;
        }
        [HttpDelete("delete-cms-menu", Name = "DeleteCmsMenuAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteCmsMenuAsync([FromBody] DeleteCmsMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.CmsMenuMan.DeleteCmsMenuAsync(model);

            return getData;
        }
    }
}
