using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/CmsFmbs")]
    public class CmsFmbApiController : Controller
    {
        private readonly CmsFmbService CmsFmbMan;

        public CmsFmbApiController(CmsFmbService cmsFmbService)
        {
            this.CmsFmbMan = cmsFmbService;
        }

        [HttpGet("get-all-cms-fmbs")]
        public async Task<ActionResult<CmsFmbPaginate>> GetAllCmsFmbAsync([FromQuery] CmsFmbGridFilter filter)
        {
            var data = await this.CmsFmbMan.GetAllCmsFmb(filter);
            return Ok(data);
        }
        [HttpPost("insert-cms-fmbs", Name = "InsertCmsFmbAsync")]
        public async Task<ActionResult<Guid>> InsertCmsFmbAsync([FromBody] CmsFmbCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.CmsFmbMan.InsertCmsFmbAsync(model);

            return Ok(success);
        }
        [HttpGet("get-cms-fmb-by-id", Name = "GetCmsFmbByIdAsync")]
        public async Task<ActionResult<CmsFmbModel>> GetCmsFmbByIdAsync([FromQuery] Guid Cms_FmbId)
        {
            var data = await this.CmsFmbMan.GetCmsFmbById(Cms_FmbId);

            return Ok(data);
        }
        [HttpPost("update-cms-fmb-by-id", Name = "UpdateCmsFmbAsync")]
        public async Task<ActionResult<bool>> UpdateCmsFmbAsync([FromBody] CmsFmbUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.CmsFmbMan.UpdateCmsFmbAsync(model);

            return Ok(success);
        }
        [HttpDelete("delete-cms-fmb", Name = "DeleteCmsFmbAsync")]
        public async Task<ActionResult> DeleteCmsFmbAsync([FromBody] DeleteCmsFmbModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await this.CmsFmbMan.DeleteCmsFmbAsync(model);

            return Ok();
        }
    }
}
