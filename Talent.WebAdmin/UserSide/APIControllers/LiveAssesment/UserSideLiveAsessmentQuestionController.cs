using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?Linkrequest=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/live-assesment/question")]
    public class UserSideLiveAsessmentQuestionController : Controller
    {
        private readonly UserSideLiveAssesmentSkillCheckQuestionService AssesmentService;

        public UserSideLiveAsessmentQuestionController(UserSideLiveAssesmentSkillCheckQuestionService service)
        {
            this.AssesmentService = service;
        }

     
        [HttpGet("get")]
        public async Task<ActionResult<BaseResponse>> Get(ParamLiveAssesmentSkillCheckQuestionModel request)
        {
            var data = await this.AssesmentService.GetDetail(request);
            return data;
        }

        [HttpGet("get-list")]
        public async Task<ActionResult<BaseResponse>> GetList(ParamLiveAssesmentSkillCheckQuestionModel request)
        {
            var data = await this.AssesmentService.GetDetail(request);
            return data;
        }
       
        [HttpPut("update")]
        public async Task<ActionResult<BaseResponse>> Update([FromBody]RequestLiveAssesmentSkillCheckQuestionModel  model)
        {
            var data = await this.AssesmentService.Update(model);
            return data;
        }

        [HttpPut("create")]
        public async Task<ActionResult<BaseResponse>> Create([FromBody]RequestLiveAssesmentSkillCheckQuestionModel  model)
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

    }
}
