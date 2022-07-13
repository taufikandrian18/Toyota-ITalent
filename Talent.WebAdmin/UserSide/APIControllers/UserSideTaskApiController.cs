using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.UserSide.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.ApiControllers
{
    [Route("api/v1/usersidetask")]
    public class UserSideTaskApiController : Controller
    {
        private readonly UserSideTaskService TaskMan;

        public UserSideTaskApiController(UserSideTaskService userSideTaskService)
        {
            TaskMan = userSideTaskService;
        }

        [HttpGet("user-side-get-task-by-id")]
        public async Task<ActionResult<object>> UserSideGetTaskById(int id)
        {
            var data = await this.TaskMan.UserSideGetTaskById(id);
            return Ok(data);
        }

        [HttpGet("user-side-get-task-ids-from-setup-module")]
        public async Task<ActionResult<List<int>>> GetTaskIdsFromSetupModule(int setupModuleId)
        {
            var data = await this.TaskMan.GetTaskIdsFromSetupModule(setupModuleId);
            return Ok(data);
        }

        [HttpGet("user-side-get-task-ids-from-pop-quiz")]
        public async Task<ActionResult<List<int>>> GetTaskIdsFromPopQuiz(int popQuizId)
        {
            var data = await this.TaskMan.GetSetupTaskIdsFromPopQuiz(popQuizId);
            return Ok(data);
        }
    }
}
