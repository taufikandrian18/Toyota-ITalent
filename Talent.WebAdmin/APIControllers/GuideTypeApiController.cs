using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/guide-type")]
    public class GuideTypeApiController : Controller
    {
        private readonly GuideTypeService GuideTypeMan;

        public GuideTypeApiController(GuideTypeService guideTypeService)
        {
            this.GuideTypeMan = guideTypeService;
        }

        [HttpGet("get-all-guide-types", Name = "get-all-guide-types")]
        public async Task<ActionResult<GuideTypeViewModel>> GetAllGuideTypes()
        {
            var result = await this.GuideTypeMan.GetAllGuideTypes();
            return Ok(result);
        }

        [HttpGet("get-guide-type-by-id/{id}", Name = "get-guide-type-by-id")]
        public async Task<ActionResult<GuideTypeModel>> GetGuideTypeById(int id)
        {
            var result = await GuideTypeMan.GetGuideTypeById(id);
            return Ok(result);
        }
    }
}
