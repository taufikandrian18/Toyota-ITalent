using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?Linkrequest=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/assesment/")]
    public class AsessmentApiController : Controller
    {
        private readonly AssesmentService AssesmentService;

        public AsessmentApiController(AssesmentService service)
        {
            this.AssesmentService = service;
        }

     
        [HttpGet("get")]
        public async Task<ActionResult<BaseResponse>> Get(ParamAssesmentModel request)
        {
            var data = await this.AssesmentService.GetDetail(request);
            return data;
        }

        [HttpGet("get-list")]
        public async Task<ActionResult<BaseResponse>> GetList(ParamAssesmentModel request)
        {
            var data = await this.AssesmentService.GetList(request);
            return data;
        }


       
        [HttpPut("update")]
        public async Task<ActionResult<BaseResponse>> Update([FromBody]RequestAssesmentModel  model)
        {
            var data = await this.AssesmentService.Update(model);
            return data;
        }

        [HttpPost("create")]
        public async Task<ActionResult<BaseResponse>> Create([FromBody]RequestAssesmentModel  model)
        {
            var data = await this.AssesmentService.Create(model);
            return data;
        }

      

        [HttpDelete("delete")]
        public async Task<ActionResult<BaseResponse>> Delete(string request)
        {
            var data = await this.AssesmentService.Delete(request);
            return data;
        }

        [HttpGet("tracking-progress-report")]
        public async Task<ActionResult<BaseResponse>> GetTrackingProgressReport([FromQuery]RequestTrackingProgressModel request)
        {
            var data = await this.AssesmentService.GetTrackingProgress(request);
            return data;
        }

       /* [HttpGet("export-tracking-progress-report")]
        public async Task<ActionResult<BaseResponse>> ExportTrackingProgressReport([FromQuery]RequestTableOfContent request)
        {
            var data = await this.AssesmentService.ExportTrackingProgressUser(request);
            return data;
        }*/

        [HttpGet("tracking-progress-report-user")]
        public async Task<ActionResult<BaseResponse>> GetTrackingProgressReportUser([FromQuery] RequestTableOfContent request)
        {
            var data = await this.AssesmentService.GetTrackingProgressUser(request);
            return data;
        }
    }
}
