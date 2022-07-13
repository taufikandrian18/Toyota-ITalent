using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
  [Route("api/v1/CmsSettings")]
  public class CmsSettingApiController : Controller
  {
    private readonly CmsSettingService CmsSettingMan;

    public CmsSettingApiController(CmsSettingService cmsSettingService)
    {
      this.CmsSettingMan = cmsSettingService;
    }
    [HttpGet("get-all-cms-settings")]
    public async Task<ActionResult<CmsSettingPaginate>> GetAllCmsSettingAsync([FromQuery] CmsSettingGridFilter filter)
    {
      var data = await this.CmsSettingMan.GetAllCmsSetting(filter);
      return Ok(data);
    }
    [HttpPost("insert-cms-settings", Name = "InsertCmsSettingAsync")]
    public async Task<ActionResult<Guid>> InsertCmsSettingAsync([FromBody] CmsSettingCreateModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var success = await this.CmsSettingMan.InsertCmsSettingAsync(model);

      return Ok(success);
    }
    [HttpGet("get-cms-setting-by-id", Name = "GetCmsSettingByIdAsync")]
    public async Task<ActionResult<CmsSettingModel>> GetCmsSettingByIdAsync([FromQuery] Guid Cms_SettingId)
    {
      var data = await this.CmsSettingMan.GetCmsSettingById(Cms_SettingId);

      return Ok(data);
    }
    [HttpPost("update-cms-setting-by-id", Name = "UpdateCmsSettingAsync")]
    public async Task<ActionResult<bool>> UpdateCmsSettingAsync([FromBody] CmsSettingUpdateModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var success = await this.CmsSettingMan.UpdateCmsSettingAsync(model);

      return Ok(success);
    }
    [HttpDelete("delete-cms-settings", Name = "DeleteCmsSettingAsync")]
    public async Task<ActionResult> DeleteCmsSettingAsync([FromBody] DeleteCmsSettingModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      await this.CmsSettingMan.DeleteCmsSettingAsync(model);

      return Ok();
    }
  }
}
