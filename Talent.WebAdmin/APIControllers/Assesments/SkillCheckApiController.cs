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
    [Route("api/assesment/skill-check")]
    public class SkillCheckApiController : Controller
    {
        private readonly SkillCheckService SkillCheckService;

        public SkillCheckApiController(SkillCheckService service)
        {
            this.SkillCheckService = service;
        }

     
        [HttpGet("get")]
        public async Task<ActionResult<BaseResponse>> Get(ParamSkillCheckModel request)
        {
            var data = await this.SkillCheckService.GetDetail(request);
            return data;
        }

        [HttpGet("get-list")]
        public async Task<ActionResult<BaseResponse>> GetList(ParamSkillCheckModel request)
        {
            var data = await this.SkillCheckService.GetList(request);
            return data;
        }


       
        [HttpPut("update")]
        public async Task<ActionResult<BaseResponse>> Update([FromBody]RequestSkillCheckModel  model)
        {
            var data = await this.SkillCheckService.Update(model);
            return data;
        }

        [HttpPost("create")]
        public async Task<ActionResult<BaseResponse>> Create([FromBody]RequestSkillCheckModel  model)
        {
            var data = await this.SkillCheckService.Create(model);
            return data;
        }

      

        [HttpDelete("delete")]
        public async Task<ActionResult<BaseResponse>> Delete(string request)
        {
            var data = await this.SkillCheckService.Delete(request);
            return data;
        }

    }
}
