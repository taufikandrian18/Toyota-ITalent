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
    [Route("api/skill-check")]
    public class ScoreConfigApiController : Controller
    {
        private readonly ScoreConfigService ScoreConfigService;

        public ScoreConfigApiController(ScoreConfigService service)
        {
            this.ScoreConfigService = service;
        }

     
        [HttpGet("get")]
        public async Task<ActionResult<BaseResponse>> Get(ParamScoreConfigModel request)
        {
            var data = await this.ScoreConfigService.GetDetail(request);
            return data;
        }

        [HttpGet("get-list")]
        public async Task<ActionResult<BaseResponse>> GetList(ParamScoreConfigModel request)
        {
            var data = await this.ScoreConfigService.GetDetail(request);
            return data;
        }


       
        [HttpPut("update")]
        public async Task<ActionResult<BaseResponse>> Update([FromBody]RequestScoreConfigModel  model)
        {
            var data = await this.ScoreConfigService.Update(model);
            return data;
        }

        [HttpPut("create")]
        public async Task<ActionResult<BaseResponse>> Create([FromBody]RequestScoreConfigModel  model)
        {
            var data = await this.ScoreConfigService.Create(model);
            return data;
        }

      

        [HttpDelete("delete")]
        public async Task<ActionResult<BaseResponse>> Delete(string request)
        {
            var data = await this.ScoreConfigService.Delete(request);
            return data;
        }

    }
}
