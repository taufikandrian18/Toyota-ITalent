using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/KodawariDownloads")]
    public class KodawariDownloadApiController : Controller
    {
        private readonly KodawariDownloadService KodawariDownloadMan;

        public KodawariDownloadApiController(KodawariDownloadService kodawariDownloadService)
        {
            this.KodawariDownloadMan = kodawariDownloadService;
        }
        [HttpGet("get-all-kodawari-downloads")]
        public async Task<ActionResult<KodawariDownloadPaginate>> GetAllKodawariDownloadAsync([FromQuery] KodawariDownloadGridFilter filter)
        {
            var data = await this.KodawariDownloadMan.GetAllKodawariDownload(filter);
            return Ok(data);
        }
        [HttpPost("insert-kodawari-download", Name = "InsertKodawariDownloadAsync")]
        public async Task<ActionResult<BaseResponse>> InsertKodawariDownloadAsync([FromBody] KodawariDownloadCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KodawariDownloadMan.InsertKodawariDownloadAsync(model);

            return success;
        }
        [HttpGet("get-kodawari-download-by-id", Name = "GetKodawariDownloadByIdAsync")]
        public async Task<ActionResult<KodawariDownloadModel>> GetKodawariDownloadByIdAsync([FromQuery] Guid kodawariDownloadId)
        {
            var data = await this.KodawariDownloadMan.GetKodawariDownloadById(kodawariDownloadId);

            return Ok(data);
        }
        [HttpPost("update-kodawari-download-by-id", Name = "UpdateKodawariDownloadAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateKodawariDownloadAsync([FromBody] KodawariDownloadUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KodawariDownloadMan.UpdateKodawariDownloadAsync(model);

            return success;
        }
        [HttpDelete("delete-kodawari-downloads", Name = "DeleteKodawariDownloadAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteKodawariDownloadAsync([FromBody] DeleteKodawariDownloadModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.KodawariDownloadMan.DeleteKodawariDownloadAsync(model);

            return getData;
        }
        [HttpPost("update-kodawari-download-by-downloadable", Name = "UpdateKodawariDownloadByDownloadableAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateKodawariDownloadByDownloadableAsync([FromBody] UpdateKodawariDownloadStatusDownloadModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.KodawariDownloadMan.UpdateKodawariDownloadByIsDownloadableAsync(model);

            return getData;
        }
    }
}
