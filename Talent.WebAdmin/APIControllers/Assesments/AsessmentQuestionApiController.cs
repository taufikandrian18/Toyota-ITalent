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
    [Route("api/assesment/questions")]
    public class AsessmentQuestionApiController : Controller
    {
        private readonly AssesmentQuestionService AssesmentQuestionService;

        public AsessmentQuestionApiController(AssesmentQuestionService service)
        {
            this.AssesmentQuestionService = service;
        }

     
        [HttpGet("get")]
        public async Task<ActionResult<BaseResponse>> Get(ParamAssesmentQuestionModel request)
        {
            var data = await this.AssesmentQuestionService.GetDetail(request);
            return data;
        }

        [HttpGet("get-list")]
        public async Task<ActionResult<BaseResponse>> GetList(ParamAssesmentQuestionModel request)
        {
            var data = await this.AssesmentQuestionService.GetList(request);
            return data;
        }


       
        [HttpPut("update")]
        public async Task<ActionResult<BaseResponse>> Update([FromBody]RequestAssesmentQuestionModel  model)
        {
            var data = await this.AssesmentQuestionService.Update(model);
            return data;
        }

        [HttpPut("create")]
        public async Task<ActionResult<BaseResponse>> Create([FromBody]RequestAssesmentQuestionModel  model)
        {
            var data = await this.AssesmentQuestionService.Create(model);
            return data;
        }

      

        [HttpDelete("delete")]
        public async Task<ActionResult<BaseResponse>> Delete(string request)
        {
            var data = await this.AssesmentQuestionService.Delete(request);
            return data;
        }

    }
}
