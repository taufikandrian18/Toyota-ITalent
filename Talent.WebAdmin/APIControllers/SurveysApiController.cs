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
    [Route("api/v1/surveys")]
    public class SurveysApiController : Controller
    {
        private readonly SurveysService _SurveyMan;

        public SurveysApiController(SurveysService surveysService)
        {
            this._SurveyMan = surveysService;
        }
        [HttpGet("get-all-survey-paginate-async")]
        public async Task<ActionResult<SurveysPaginate>> GetAllSurveyPaginateAsync(SurveysGridFilter filter)
        {
            var data = await this._SurveyMan.GetAllSurveyPaginateAsync(filter);
            return Ok(data);

        }
        [HttpGet("get-all-approval-status")]
        public async Task<ActionResult<List<ApprovalStatusModel>>> GetAllApprovalStatus()
        {
            var data = await this._SurveyMan.GetAllApprovalStatus();
            return Ok(data);
        }
        [HttpGet("get-all-survey-question-type")]
        public async Task<ActionResult<List<SurveyQuestionType>>> GetAllSurveyQuestionType()
        {
            var data = await this._SurveyMan.GetAllSurveyQuestionType();
            return Ok(data);
        }
        [HttpGet("get-survey-by-id/{id}")]
        public async Task<ActionResult<SurveyInitialize>> GetSurveyById(int id)
        {
            var data = await this._SurveyMan.GetSurveyById(id);
            return Ok(data);
        }

        [HttpPost("submit-survey")]
        public async Task<ActionResult<bool>> SubmitSurvey([FromBody]SurveyInsert insert)
        {
            var data = await this._SurveyMan.SubmitSurvey(insert);
            return Ok(data);
        }

        [HttpPost("save-survey")]
        public async Task<ActionResult<bool>> SaveSurvey([FromBody]SurveyInsert insert)
        {
            var data = await this._SurveyMan.SaveSurvey(insert);
            return Ok(data);
        }

        [HttpPost("update-submit-survey/{surveyId}")]
        public async Task<ActionResult<bool>> UpdateSubmitSurvey([FromBody]SurveyInsert insert, int surveyId)
        {
            var data = await this._SurveyMan.SubmitEditSurvey(surveyId, insert);
            return Ok(data);
        }

        [HttpPost("update-save-survey/{surveyId}")]
        public async Task<ActionResult<bool>> UpdateSaveSurvey([FromBody]SurveyInsert insert, int surveyId)
        {
            var data = await this._SurveyMan.SaveEditSurvey(insert, surveyId);
            return Ok(data);
        }

        [HttpPost("delete-survey/{surveyId}")]
        public async Task<ActionResult<bool>> DeleteSurvey(int surveyId)
        {
            var data = await this._SurveyMan.DeleteSurveyById(surveyId);
            return Ok(data);
        }

        [HttpGet("get-area-by-id/{id}")]
        public async Task<ActionResult<List<AreaViewModel>>> GetAreaById(int id)
        {
            var data = await this._SurveyMan.GetAreaById(id);
            return data;
        }

        [HttpGet("get-region-by-id/{id}")]
        public async Task<ActionResult<List<RegionViewModel>>> GetRegionById(int id)
        {
            var data = await this._SurveyMan.GetRegionById(id);
            return data;
        }

        [HttpGet("get-dealer-by-id/{id}")]
        public async Task<ActionResult<List<DealerViewModel>>> GetDealerById(int id)
        {
            var data = await this._SurveyMan.GetDealerById(id);
            return data;
        }

        [HttpGet("get-province-by-id/{id}")]
        public async Task<ActionResult<List<ProvinceViewModel>>> GetProvinceById(int id)
        {
            var data = await this._SurveyMan.GetProvinceById(id);
            return data;
        }

        [HttpGet("get-city-by-id/{id}")]
        public async Task<ActionResult<List<CityViewModel>>> GetCityById(int id)
        {
            var data = await this._SurveyMan.GetCityById(id);
            return data;
        }
    }
}
