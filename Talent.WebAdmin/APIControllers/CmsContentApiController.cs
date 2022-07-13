using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
  [Route("api/v1/CmsContents")]
  public class CmsContentApiController : Controller
  {
    private readonly CmsContentService CmsContentMan;

    public CmsContentApiController(CmsContentService cmsContentService)
    {
      this.CmsContentMan = cmsContentService;
    }

    [HttpGet("get-all-cms-contents")]
    public async Task<ActionResult<CmsContentPaginate>> GetAllCmsContentAsync([FromQuery] CmsContentGridFilter filter)
    {
      var data = await this.CmsContentMan.GetAllCmsContent(filter);
      return Ok(data);
    }
    [HttpPost("insert-cms-content", Name = "InsertCmsContentAsync")]
    public async Task<ActionResult<Guid>> InsertCmsContentAsync([FromBody] CmsContentCreateModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var success = await this.CmsContentMan.InsertCmsContentAsync(model);

      return Ok(success);
    }
    [HttpGet("get-cms-content-by-id", Name = "GetCmsContentByIdAsync")]
    public async Task<ActionResult<CmsContentModel>> GetCmsContentByIdAsync([FromQuery] Guid Cms_ContentId)
    {
      var data = await this.CmsContentMan.GetCmsContentById(Cms_ContentId);

      return Ok(data);
    }
    [HttpPost("update-cms-content-by-id", Name = "UpdateCmsContentAsync")]
    public async Task<ActionResult<bool>> UpdateCmsContentAsync([FromBody] CmsContentUpdateModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var success = await this.CmsContentMan.UpdateCmsContentAsync(model);

      return Ok(success);
    }
    [HttpDelete("delete-cms-content", Name = "DeleteCmsContentAsync")]
    public async Task<ActionResult> DeleteCmsContentAsync([FromBody] DeleteCmsContentModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      await this.CmsContentMan.DeleteCmsContentAsync(model);

      return Ok();
    }
  }
}
