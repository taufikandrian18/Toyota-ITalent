using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/KodawariBanners")]
    public class KodawariBannerApiController : Controller
    {
        private readonly KodawariBannerService KodawariBannerMan;

        public KodawariBannerApiController(KodawariBannerService kodawariBannerService)
        {
            this.KodawariBannerMan = kodawariBannerService;
        }
        [HttpGet("get-all-kodawari-banners")]
        public async Task<ActionResult<KodawariBannerPaginate>> GetAllKodawariBannerAsync([FromQuery] KodawariBannersGridFilter filter)
        {
            var data = await this.KodawariBannerMan.GetAllKodawariBanner(filter);
            return Ok(data);
        }
        [HttpPost("insert-kodawari-banner", Name = "InsertKodawariBannerAsync")]
        public async Task<ActionResult<BaseResponse>> InsertKodawariBannerAsync([FromBody] KodawariBannerCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KodawariBannerMan.InsertKodawariBannerAsync(model);

            return success;
        }
        [HttpGet("get-kodawari-banner-by-id", Name = "GetKodawariBannerByIdAsync")]
        public async Task<ActionResult<KodawariBannerModel>> GetKodawariBannerByIdAsync([FromQuery] Guid kodawariBannerId)
        {
            var data = await this.KodawariBannerMan.GetKodawariBannerById(kodawariBannerId);

            return Ok(data);
        }
        [HttpPost("update-kodawari-banner-by-id", Name = "UpdateKodawariBannerAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateKodawariBannerAsync([FromBody] KodawariBannerUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KodawariBannerMan.UpdateKodawariBannerAsync(model);

            return success;
        }
        [HttpDelete("delete-kodawari-banners", Name = "DeleteKodawariBannerAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteKodawariBannerAsync([FromBody] DeleteKodawariBannerModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.KodawariBannerMan.DeleteKodawariBannerAsync(model);

            return getData;
        }
    }
}
