using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
  [Route("api/v1/CmsWorkPrincipals")]
  public class CmsWorkPrincipalApiController : Controller
  {
    private readonly CmsWorkPrincipalService CmsWorkPrincipalMan;

    public CmsWorkPrincipalApiController(CmsWorkPrincipalService cmsWorkPrincipalService)
    {
      this.CmsWorkPrincipalMan = cmsWorkPrincipalService;
    }

    [HttpGet("get-all-cms-work-principals")]
    public async Task<ActionResult<CmsWorkPrincipalPaginate>> GetAllCmsWorkPrincipalAsync([FromQuery] CmsWorkPrincipalGridFilter filter)
    {
      var data = await this.CmsWorkPrincipalMan.GetAllCmsWorkPrincipal(filter);
      return Ok(data);
    }
    [HttpPost("insert-cms-work-principals", Name = "InsertCmsWorkPrincipalAsync")]
    public async Task<ActionResult<Guid>> InsertCmsWorkPrincipalAsync([FromBody] CmsWorkPrincipalCreateModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var success = await this.CmsWorkPrincipalMan.InsertCmsWorkPrincipalAsync(model);

      return Ok(success);
    }
    [HttpGet("get-cms-work-principal-by-id", Name = "GetCmsWorkPrincipalByIdAsync")]
    public async Task<ActionResult<CmsWorkPrincipalModel>> GetCmsWorkPrincipalByIdAsync([FromQuery] Guid Cms_WorkPrincipalId)
    {
      var data = await this.CmsWorkPrincipalMan.GetCmsWorkPrincipalById(Cms_WorkPrincipalId);

      return Ok(data);
    }
    [HttpPost("update-cms-work-principal-by-id", Name = "UpdateCmsWorkPrincipalAsync")]
    public async Task<ActionResult<bool>> UpdateCmsWorkPrincipalAsync([FromBody] CmsWorkPrincipalUpdateModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var success = await this.CmsWorkPrincipalMan.UpdateCmsWorkPrincipalAsync(model);

      return Ok(success);
    }
    [HttpDelete("delete-cms-work-principal", Name = "DeleteCmsWorkPrincipalAsync")]
    public async Task<ActionResult> DeleteCmsWorkPrincipalAsync([FromBody] DeleteCmsWorkPrincipalModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      await this.CmsWorkPrincipalMan.DeleteCmsWorkPrincipalAsync(model);

      return Ok();
    }
  }
}
