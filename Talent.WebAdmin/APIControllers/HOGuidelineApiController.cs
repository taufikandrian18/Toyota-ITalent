using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
  [Route("api/v1/HOGuidelines")]
  public class HOGuidelineApiController : Controller
  {
    private readonly HOGuidelineUploadService HOGuidelineUploadMan;

    public HOGuidelineApiController(HOGuidelineUploadService hoGuidelineUploadService)
    {
      this.HOGuidelineUploadMan = hoGuidelineUploadService;
    }
    [HttpGet("get-all-ho-guideline-uploads")]
    public async Task<ActionResult<HOGuidelineUploadPaginate>> GetAllHOGuidelineUploadAsync([FromQuery] HOGuidelineUploadGridFilter filter)
    {
      var data = await this.HOGuidelineUploadMan.GetAllHOGuidelineUpload(filter);
      return Ok(data);
    }
    [HttpGet("get-all-ho-guidelines")]
    public async Task<ActionResult<HOGuidelinePaginate>> GetAllHOGuidelineAsync([FromQuery] HOGuidelineGridFilter filter)
    {
      var data = await this.HOGuidelineUploadMan.GetAllHOGuideline(filter);
      return Ok(data);
    }
    [HttpPost("insert-ho-guideline-upload", Name = "InsertHOGuidelineUploadAsync")]
    public async Task<ActionResult<BaseResponse>> InsertHOGuidelineUploadAsync([FromBody] HOGuidelineUploadCreateModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var success = await this.HOGuidelineUploadMan.InsertHOGuidelineUploadAsync(model);

      return success;
    }
    [HttpGet("get-ho-guideline-by-id", Name = "GetHOGuidelineByIdAsync")]
    public async Task<ActionResult<HOGuidelineModel>> GetHOGuidelineByIdAsync([FromQuery] Guid hoGuidelineId)
    {
      var data = await this.HOGuidelineUploadMan.GetHOGuidelineById(hoGuidelineId);

      return Ok(data);
    }
    [HttpPost("update-ho-guideline-comment-by-id", Name = "UpdateHOGuidelineCommentAsync")]
    public async Task<ActionResult<BaseResponse>> UpdateHOGuidelineCommentAsync([FromBody] HOGuidelineCommentUpdateModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var success = await this.HOGuidelineUploadMan.UpdateHOGuidelineCommentAsync(model);

      return success;
    }
    [HttpPost("update-ho-guideline-approve-by-id", Name = "UpdateHOGuidelineApproveAsync")]
    public async Task<ActionResult<BaseResponse>> UpdateHOGuidelineApproveAsync([FromBody] HOGuidelineStatusApprovedUpdateModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var success = await this.HOGuidelineUploadMan.UpdateHOGuidelineApproveAsync(model);

      return success;
    }

    [HttpPost("update-ho-guideline-by-id", Name = "UpdateHOGuidelineAsync")]
    public async Task<ActionResult<BaseResponse>> UpdateHOGuidelineAsync([FromBody] HOGuidelineUploadUpdateModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var success = await this.HOGuidelineUploadMan.UpdateHOGuidelineAsync(model);

      return success;
    }

    [HttpDelete("delete-ho-guidelines", Name = "DeleteHOGuidelineAsync")]
    public async Task<ActionResult<BaseResponse>> DeleteHOGuidelineAsync([FromBody] DeleteHOGuidelineModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      var getData = await this.HOGuidelineUploadMan.DeleteHOGuidelineAsync(model);

      return getData;
    }
  }
}
