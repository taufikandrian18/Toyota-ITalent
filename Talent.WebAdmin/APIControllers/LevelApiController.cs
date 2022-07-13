using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/level")]
    public class LevelApiController : Controller
    {
        private readonly LevelService LevelMan;

        public LevelApiController(LevelService levelService)
        {
            this.LevelMan = levelService;
        }

        [HttpGet("get-all-levels", Name = "get-all-levels")]
        public async Task<ActionResult<LevelViewModel>> GetAllLevels()
        {
            var result = await this.LevelMan.GetAllLevels();
            return Ok(result);
        }

        [HttpGet("get-level-by-id/{id}", Name = "get-level-by-id")]
        public async Task<ActionResult<LevelModel>> GetLevelById(int id)
        {
            var result = await LevelMan.GetLevelById(id);
            return Ok(result);
        }
    }
}
