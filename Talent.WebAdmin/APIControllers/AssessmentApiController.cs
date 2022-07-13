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
    [Route("api/v1/assessment")]
    public class AssessmentApiController : Controller
    {
        private readonly AssessmentService assMan;

        public AssessmentApiController(AssessmentService service)
        {
            this.assMan = service;
        }

        [HttpGet("get-all-assessment")]
        public async Task<ActionResult<AssessmentGridModel>> GetAllAssessmentAsync([FromQuery] AssessmentFilterModel filter)
        {
            var data = await this.assMan.GetAllAssessmentAsync(filter);

            return Ok(data);
        }

        [HttpPost("insert-assessment")]
        public async Task<ActionResult> InsertAssessmentAsync([FromBody] AssessmentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await this.assMan.InsertAssessmentAsync(model);

            return Ok();
        }

        [HttpGet("get-assessment-by-id")]
        public async Task<ActionResult<AssessmentViewDetailModel>> GetAssessmentByIdAsync([FromQuery] int assessmentId)
        {
            var data = await this.assMan.GetAssessmentByIdAsync(assessmentId);

            return Ok(data);
        }

        [HttpPost("update-assessment")]
        public async Task<ActionResult> UpdateAssessmentAsync([FromBody] AssessmentUpdateModel model)
        {
            await this.assMan.UpdateAssessmentAsync(model);

            return Ok();
        }

        [HttpPost("delete-assessment")]
        public async Task<ActionResult> DeleteAssessmentAsync([FromQuery] int assessmentId)
        {
            await this.assMan.DeleteAssessmentAsync(assessmentId);

            return Ok();
        }
    }
}
