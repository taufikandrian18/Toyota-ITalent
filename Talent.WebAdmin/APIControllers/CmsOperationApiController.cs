using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
  [Route("api/v1/CmsOperations")]
  public class CmsOperationApiController : Controller
  {
    private readonly CmsOperationService CmsOperationMan;

    public CmsOperationApiController(CmsOperationService cmsOperationService)
    {
      this.CmsOperationMan = cmsOperationService;
    }
    [HttpGet("get-all-cms-operations")]
    public async Task<ActionResult<CmsOperationPaginate>> GetAllCmsOperationAsync([FromQuery] CmsOperationGridFilter filter)
    {
      var data = await this.CmsOperationMan.GetAllCmsOperation(filter);
      return Ok(data);
    }
    [HttpPost("insert-cms-operations", Name = "InsertCmsOperationAsync")]
    public async Task<ActionResult<Guid>> InsertCmsOperationAsync([FromBody] CmsOperationCreateModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var success = await this.CmsOperationMan.InsertCmsOperationAsync(model);

      return Ok(success);
    }
    [HttpGet("get-cms-operation-by-id", Name = "GetCmsOperationByIdAsync")]
    public async Task<ActionResult<CmsOperationModel>> GetCmsOperationByIdAsync([FromQuery] Guid Cms_OperationId)
    {
      var data = await this.CmsOperationMan.GetCmsOperationById(Cms_OperationId);

      return Ok(data);
    }
    [HttpPost("update-cms-operation-by-id", Name = "UpdateCmsOperationAsync")]
    public async Task<ActionResult<bool>> UpdateCmsOperationAsync([FromBody] CmsOperationUpdateModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var success = await this.CmsOperationMan.UpdateCmsOperationAsync(model);

      return Ok(success);
    }
    [HttpDelete("delete-cms-operation", Name = "DeleteCmsOperationAsync")]
    public async Task<ActionResult> DeleteCmsOperationAsync([FromBody] DeleteCmsOperationModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      await this.CmsOperationMan.DeleteCmsOperationAsync(model);

      return Ok();
    }
  }
}
