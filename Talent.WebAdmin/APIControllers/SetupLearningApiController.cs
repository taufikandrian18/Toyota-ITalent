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
    [Route("api/v1/setup-learning")]
    public class SetupLearningApiController : Controller
    {
        private readonly SetupLearningService _SetupLearningMan;

        public SetupLearningApiController(SetupLearningService setupLearningService)
        {
            this._SetupLearningMan = setupLearningService;
        }
        [HttpGet("get-all-program-type")]
        public async Task<ActionResult<List<string>>> GetAllProgramTypeAsync()
        {
            var data = await this._SetupLearningMan.GetAllProgramType();
            return Ok(data);
        }
        [HttpGet("get-all-approval-status")]
        public async Task<ActionResult<List<string>>> GetAllSetupLearningAsync()
        {
            var data = await this._SetupLearningMan.GetAllApprovalStatus();
            return Ok(data);
        }
        [HttpGet("get-all-setup-learning")]
        public async Task<ActionResult<SetupLearningPaginate>> GetAllSetupLearningAsync(SetupLearningGridFilter filter)
        {
            var data = await this._SetupLearningMan.GetAllSetupLearning(filter);
            return Ok(data);
        }
        [HttpGet("get-all-course-lock-unlock/{id}")]
        public async Task<ActionResult<SetupLearningCourseLockUnlock>> GetAllCourseLockUnlock(int id)
        {
            var data = await this._SetupLearningMan.GetAllCourseLockUnlock(id);
            return Ok(data);
        }
        [HttpPost("course-lock-unlock-module")]
        public async Task<ActionResult<bool>> CourseLockUnlockModule([FromBody]SetupLearningCourseLockUnlock course)
        {
            var data = await this._SetupLearningMan.CourseLockUnlockModule(course);
            return Ok(data);
        }
        [HttpDelete("remove-course")]
        public async Task<ActionResult<bool>> RemoveCourse(int id)
        {
            var result = await this._SetupLearningMan.RemoveCourse(id);

            if (result == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
