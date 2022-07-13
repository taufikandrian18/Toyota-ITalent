using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/banner")]
    public class BannerApiController : Controller
    {
        private readonly BannerService BannerMan;

        public BannerApiController(BannerService bannerService)
        {
            this.BannerMan = bannerService;
        }

        [HttpGet("get-banner-filtered", Name = "get-banner-filtered")]
        public async Task<ActionResult<BannerViewModel>> GetBannerFiltered([FromQuery] BannerFilterModel model)
        {
            var result = await this.BannerMan.GetBannerFiltered(model);

            return Ok(result);
        }

        [HttpPost("insert-banner", Name = "insert-banner")]
        public async Task<ActionResult<bool>> InsertBanner([FromBody] BannerFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model State is not valid");
            }

            var result = await this.BannerMan.InsertBanner(model);

            if (result == false)
            {
                return BadRequest("Failed to Insert");
            }

            return Ok(result);
        }

        [HttpDelete("delete-banner/{id}", Name = "delete-banner")]
        public async Task<ActionResult<bool>> DeleteBanner(int id)
        {
            var result = await this.BannerMan.DeleteBanner(id);

            if (result == false)
            {
                return BadRequest("Failed to Delete");
            }

            return Ok(result);
        }

        [HttpGet("get-banner-by-id/{id}", Name = "get-banner-by-id")]
        public async Task<ActionResult<BannerFormModel>> GetBannerById(int id)
        {
            var result = await this.BannerMan.GetBannerById(id);
            return Ok(result);
        }

        [HttpGet("get-banner-by-id-from-outside/{id}", Name = "get-banner-by-id-from-outside")]
        public async Task<ActionResult<BannerViewModel>> GetBannerByIdFromOutside(int id)
        {
            var result = await this.BannerMan.GetBannerByIdFromOutside(id);
            return Ok(result);
        }

    
        [HttpGet("get-all-content-type", Name = "get-all-content-type")]
        public async Task<ActionResult<List<MobileMappingPage>>> GetAllContentType()
        {
            var result = await this.BannerMan.GetAllContentType();

            return Ok(result);
        }

        [HttpGet("get-content-name", Name = "get-content-name")]
        public async Task<ActionResult<List<ContentModel>>> GetAllContentNameFiltered([FromQuery] ContentFilterModel model)
        {
            var result = await this.BannerMan.SearchContent(model);

            return Ok(result);
        }

        [HttpGet("get-content-name-by-id", Name = "get-content-name-by-id")]
        public async Task<ActionResult<ContentModel>> GetAllContentNameWithIdFiltered([FromQuery] ContentWithIdFilterModel model)
        {
            var result = await this.BannerMan.SearchContentWithId(model);

            return Ok(result);
        }

        [HttpPost("update-banner", Name = "update-banner")]
        public async Task<ActionResult<bool>> UpdateBanner([FromBody] BannerFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model state is not valid");
            }

            var result = await this.BannerMan.UpdateBanner(model);

            if (result == false)
            {
                return BadRequest("Something went wrong");
            }

            return Ok(result);
        }

        [HttpGet("get-approval-status-for-banner", Name = "get-approval-status-for-banner")]
        public async Task<ActionResult<List<ApprovalStatusForBannerModel>>> GetApprovalStatusBanner()
        {
            var result = await this.BannerMan.GetApprovalStatus();
            return Ok(result);
        }

    }
}
