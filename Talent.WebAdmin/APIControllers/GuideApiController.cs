using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/guide")]
    public class GuideApiController : Controller
    {
        private readonly GuideService GuideMan;
        private readonly GuideJoinService GuideJoinMan;

        public GuideApiController(GuideService guideService, GuideJoinService guideJoinService)
        {
            this.GuideMan = guideService;
            this.GuideJoinMan = guideJoinService;
        }

        [HttpGet("get-all-join-guides", Name = "get-all-join-guides")]
        public async Task<ActionResult<GuideJoinViewModel>> GetAllJoinGuides([FromQuery] GuideFilter filter)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await this.GuideJoinMan.GetAllGuides(filter);
            return Ok(result);
        }

        [HttpGet("get-join-guide-by-id/{id}", Name = "get-join-guide-by-id")]
        public async Task<ActionResult<GuideJoinModel>> GetJoinGuideById(int id)
        {
            var result = await GuideJoinMan.GetGuideJoinById(id);
            return Ok(result);
        }

        [HttpPost("create", Name = "create-guide")]
        public async Task<ActionResult<int>> CreateGuide([FromBody] GuideFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await GuideMan.CreateGuide(model);
            return Ok(result);
        }

        [HttpPost("update", Name = "update-guide")]
        public async Task<ActionResult<bool>> UpdateGuide([FromBody] GuideFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var result = await GuideMan.UpdateGuide(model);
            if (result == false)
            {
                return BadRequest("Data not available");
            }
            return Ok(result);
        }

        [HttpDelete("delete", Name = "delete-guide")]
        public async Task<ActionResult<bool>> DeleteGuide([FromBody] int id)
        {
            var result = await GuideMan.DeleteGuide(id);
            if (result == false)
            {
                return BadRequest("Data not available");
            }
            return Ok(result);
        }
    }
}
