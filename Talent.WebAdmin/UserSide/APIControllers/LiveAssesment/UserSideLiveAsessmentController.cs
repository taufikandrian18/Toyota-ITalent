using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?Linkrequest=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/live-assesment/")]
    public class UserSideLiveAsessmentController : Controller
    {
        private readonly UserSideLiveAssesmentSkillCheckService AssesmentService;
        private UserSideAuthService UserSideMan;

        public UserSideLiveAsessmentController(UserSideLiveAssesmentSkillCheckService service)
        {
            this.AssesmentService = service;
        }

        [HttpGet("get-allow-nps")]
        public async Task<ActionResult<BaseResponse>> GetAllowNPS(ParamIsAllowNPSModel request)
        {
            var data = await this.AssesmentService.IsAllowNPS(request);
            return data;
        }


     
        [HttpGet("get")]
        public async Task<ActionResult<BaseResponse>> Get(ParamLiveAssesmentSkillCheckModel request)
        {
            var data = await this.AssesmentService.GetDetail(request);
            return data;
        }

        [HttpGet("get-list")]
        public async Task<ActionResult<BaseResponse>> GetList(ParamLiveAssesmentSkillCheckModel request)
        {
            var data = await this.AssesmentService.GetList(request);
            return data;
        }

        [HttpGet("get-list-assesment-skillcheck")]
        public async Task<ActionResult<BaseResponse>> GetListAssesmentSkillCheck(ParamLiveAssesmentSkillCheckModel request)
        {
            var data = await this.AssesmentService.GetListAssesmentSkillCheck(request);
            return data;
        }

        [HttpGet("get-list-assesment")]
        public async Task<ActionResult<BaseResponse>> GetListAssesment(ParamLiveAssesmentSkillCheckModel request)
        {
            var data = await this.AssesmentService.GetListAssesment(request);
            return data;
        }


        [HttpGet("get-detail-assesment")]
        public async Task<ActionResult<BaseResponse>> GetAssesmentSkillCheckDetail(ParamLiveAssesmentSkillCheckModel request)
        {
            var data = await this.AssesmentService.GetAssesmentSkillCheckDetail(request);
            return data;
        }

        [HttpGet("get-skillcheck-user")]
        public async Task<ActionResult<BaseResponse>> GetSkillCheckListByUser(ParamLiveAssesmentSkillCheckModel request)
        {
            var data = await this.AssesmentService.GetSkillCheckListByUser(request);
            return data;
        }

        [HttpGet("get-skillcheck-superior")]
        public async Task<ActionResult<BaseResponse>> GetSkillCheckListBySuperior(ParamLiveAssesmentSkillCheckModel request)
        {
            var data = await this.AssesmentService.GetSkillCheckListBySuperior(request);
            return data;
        }


        [HttpGet("get-list-team-member")]
        public async Task<ActionResult<BaseResponse>> GetListTeamMember(ParamLiveAssesmentSkillCheckModel request)
        {
            var data = await this.AssesmentService.GetListTeamMemberSkillCheck(request);
            return data;
        }


        [HttpPut("update")]
        public async Task<ActionResult<BaseResponse>> Update([FromBody]RequestLiveAssesmentSkillCheckModel  model)
        {
        
            var data = await this.AssesmentService.Update(model);
            return data;
        }

        [HttpPost("create")]
        public async Task<ActionResult<BaseResponse>> Create([FromBody]RequestLiveAssesmentSkillCheckModel  model)
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

        [HttpDelete("cleansing-data")]
        public async Task<ActionResult<BaseResponse>> CleansingData(int trainingId)
        {
            var data = await this.AssesmentService.CleansingData(trainingId);
            return data;
        }

        [HttpDelete("cleansing-data-module")]
        public async Task<ActionResult<BaseResponse>> CleansingDataModule()
        {
            var data = await this.AssesmentService.CleansingDataModule();
            return data;
        }

    }
}
