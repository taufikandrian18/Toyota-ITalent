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
    [Route("api/assesment/question-type/")]
    public class AssesmentQuestionTypeApiController : Controller
    {
        private readonly AssesmentQuestionTypeService AssesmentQuestionTypeService;

        public AssesmentQuestionTypeApiController(AssesmentQuestionTypeService service)
        {
            this.AssesmentQuestionTypeService = service;
        }

     
        [HttpGet("get")]
        public async Task<ActionResult<BaseResponse>> Get(ParamAssesmentQuestionTypeModel request)
        {
            var data = await this.AssesmentQuestionTypeService.GetDetail(request);
            return data;
        }

        [HttpGet("get-list")]
        public async Task<ActionResult<BaseResponse>> GetList(ParamAssesmentQuestionTypeModel request)
        {
            var data = await this.AssesmentQuestionTypeService.GetDetail(request);
            return data;
        }
       
        [HttpPut("update")]
        public async Task<ActionResult<BaseResponse>> Update([FromBody]RequestAssesmentQuestionTypeModel  model)
        {
            var data = await this.AssesmentQuestionTypeService.Update(model);
            return data;
        }

        [HttpPut("create")]
        public async Task<ActionResult<BaseResponse>> Create([FromBody]RequestAssesmentQuestionTypeModel  model)
        {
            var data = await this.AssesmentQuestionTypeService.Create(model);
            return data;
        }   

        [HttpDelete("delete")]
        public async Task<ActionResult<BaseResponse>> Delete(string request)
        {
            var data = await this.AssesmentQuestionTypeService.Delete(request);
            return data;
        }

    }
}
