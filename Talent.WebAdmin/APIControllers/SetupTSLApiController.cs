using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/setup-tsl")]
    public class SetupTSLApiController : Controller
    {
        private readonly SetupTSLService SetupTSLServiceMan;
        
        public SetupTSLApiController(SetupTSLService setupTSLService)
        {
            this.SetupTSLServiceMan = setupTSLService;
        }

        [HttpGet("get-all-training-service-level-data")]
        public async Task<ActionResult<SetupTSLViewModel>> getAllTSLData()
        {
            var result = await this.SetupTSLServiceMan.GetAllTSLData();

            return Ok(result);
        }

        /// <summary>
        /// Update training service level for dashboard
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("update-training-service-level")]
        public async Task<ActionResult<bool>> UpdateTrainingServiceLevel([FromBody]SetupTSLViewModel model)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest("Model State is not valid");
            }

            var result = await this.SetupTSLServiceMan.UpdateTrainingServiceLevel(model);

            return Ok(result);
        }

    }
}
