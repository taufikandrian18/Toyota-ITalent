using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/CmsSubContents")]
    public class CmsSubContentApiController : Controller
    {
        private readonly CmsSubContentService CmsSubContentMan;

        public CmsSubContentApiController(CmsSubContentService cmsSubContentService)
        {
            this.CmsSubContentMan = cmsSubContentService;
        }

        [HttpGet("get-all-cms-subcontents")]
        public async Task<ActionResult<CmsSubContentPaginate>> GetAllCmsSubContentAsync([FromQuery] CmsSubContentGridFilter filter)
        {
            var data = await this.CmsSubContentMan.GetAllCmsSubContent(filter);
            return Ok(data);
        }
        [HttpPost("insert-cms-subcontents", Name = "InsertCmsSubContentAsync")]
        public async Task<ActionResult<bool>> InsertCmsSubContentAsync([FromBody] CmsSubContentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.CmsSubContentMan.InsertCmsSubContentAsync(model);

            return Ok(success);
        }
        [HttpGet("get-cms-subcontent-by-id", Name = "GetCmsSubContentByIdAsync")]
        public async Task<ActionResult<CmsSubContentModel>> GetCmsSubContentByIdAsync([FromQuery] Guid Cms_SubContentId)
        {
            var data = await this.CmsSubContentMan.GetCmsSubContentById(Cms_SubContentId);

            return Ok(data);
        }
        [HttpPost("update-cms-subcontent-by-id", Name = "UpdateCmsSubContentAsync")]
        public async Task<ActionResult<bool>> UpdateCmsSubContentAsync([FromBody] CmsSubContentUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.CmsSubContentMan.UpdateCmsSubContentAsync(model);

            return Ok(success);
        }
        [HttpDelete("delete-cms-subcontent", Name = "DeleteCmsSubContentAsync")]
        public async Task<ActionResult> DeleteCmsSubContentAsync([FromBody] DeleteCmsSubContentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await this.CmsSubContentMan.DeleteCmsSubContentAsync(model);

            return Ok();
        }
    }
}
