using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/learning-type")]
    public class LearningTypeApiController : Controller
    {
        private readonly LearningTypeService LearningTypeMan;

        public LearningTypeApiController(LearningTypeService learningTypesService)
        {
            this.LearningTypeMan = learningTypesService;
        }

        [HttpGet("get-all-learning-types",Name = "get-all-learning-types")]
        public async Task<ActionResult<LearningTypeViewModel>> GetAllLearningTypes()
        {
            var result = await this.LearningTypeMan.GetAllLearningTypes();
            return Ok(result);
        }

        [HttpGet("get-learning-type-by-id/{id}", Name = "get-learning-type-by-id")]
        public async Task<ActionResult<LearningTypeModel>> GetLearningTypeById(int id)
        {
            var result = await LearningTypeMan.GetLearningTypeById(id);
            return Ok(result);
        }
    }
}
