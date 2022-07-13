using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using Talent.WebAdmin.UserSide.Models.LiveAssesment;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?Linkrequest=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/scoring/")]
    public class UserSideScoringController : Controller
    {
        private readonly UserSideScoringService ScoringService;

        public UserSideScoringController(UserSideScoringService service)
        {
            this.ScoringService = service;
        }


        [HttpGet("get-course-by-user")]
        public async Task<ActionResult<BaseResponse>> GetCourseByUser([FromQuery] CourseReportRequest request)
        {
            var data = await this.ScoringService.GetCourseByUser(request);
            return data;
        }

        [HttpGet("get-list-course-by-user")]

        public async Task<ActionResult<BaseResponse>> GetListCourseByUser([FromQuery] CourseReportRequest request)
        {
            var data = await this.ScoringService.GetCoursesListUser(request);
            return data;
        }

         [HttpGet("get-status-assesment")]

        public async Task<ActionResult<BaseResponse>> GetStatusAssesmentUser([FromQuery] int trainingId, string employeeId, bool isCoach, bool isTamPeople)
        {
            var data = await this.ScoringService.GetStatusAssesmentUser(trainingId, employeeId, isCoach);
            return data;
        }

       

        [HttpGet("remedial-report-my-team")]
        public async Task<ActionResult<BaseResponse>> RemedialReportMyTeam([FromQuery] ScoringReportRequest request)
        {
            var data = await this.ScoringService.RemedialReportMyTeam(request);
            return data;
        }

        [HttpGet("skillcheck-user")]
        public async Task<ActionResult<BaseResponse>> SkillCheckPerUser([FromQuery] ScoringReportRequest request)
        {
            var data = await this.ScoringService.SkillCheckPerUser(request);
            return data;
        }


        [HttpGet("get-scoring-summary-my-team")]
        public async Task<ActionResult<BaseResponse>> GetCourseCategorySummaryMyTeam(ScoringSummaryRequest request)
        {
            var data = await this.ScoringService.GetCourseCategorySummaryMyTeam(request);
            return data;
        }

        [HttpGet("get-scoring-summary-my-team-detail")]
        public async Task<ActionResult<BaseResponse>> GetCourseCategorySummaryMyTeamDetail([FromQuery] ScoringSummaryRequest request)
        {
            var data = await this.ScoringService.GetCourseCategorySummaryMyTeamDetail(request);
            return data;
        }

        [HttpGet("get-scoring-summary-my-coach-detail")]
        public async Task<ActionResult<BaseResponse>> GetCourseCategorySummaryMyCoachDetail([FromQuery] ScoringSummaryRequest request, string filetype)
        {
             if (filetype == ""){
                filetype = "xlsx";
            }
            var data = await this.ScoringService.GetCourseCategorySummaryMyCoachDetail(request, filetype);
            return data;
        }


        [HttpGet("get-scoring-course-summary-my-team")]
        public async Task<ActionResult<BaseResponse>> GetScoringCourseSummaryMyTeam(ScoringSummaryRequest request)
        {
            //var data = await this.ScoringService.GetScoringCourseSummaryMyTeam(request);
            var data = await this.ScoringService.GetScoringCourseSummaryMyTeamReplicate(request);
            return data;
        }

        [HttpGet("get-scoring-course-summary-my-team-detail")]
        public async Task<ActionResult<BaseResponse>> GetScoringCourseSummaryMyTeamDetail(ScoringSummaryRequest request)
        {
            var data = await this.ScoringService.GetScoringCourseSummaryMyTeamDetail(request);
            return data;
        }



        [HttpGet("get-scoring-course-summary-my-coach-detail")]
        public async Task<ActionResult<BaseResponse>> GetScoringCourseSummaryMyCoachDetail(ScoringSummaryRequest request, string filetype, string trainingId)
        {
            if (filetype == ""){
                filetype = "xlsx";
            }
            var data = await this.ScoringService.GetScoringCourseSummaryMyCoachDetail(request, filetype );
            return data;
        }


        [HttpGet("get-attempts")]
        public async Task<ActionResult<BaseResponse>> GetAttempts(RequestLiveAssesmentSkillCheckModel request)
        {
            var data = await this.ScoringService.GetAttempts(request);
            return data;
        }

        [HttpPost("get-attempt-Tasks")]
        public async Task<ActionResult<BaseResponse>> GetAttemptTask([FromBody] TaskAnswerGetAttemptModel insert)
        {
            var data = await this.ScoringService.GetAttemptTask(insert);
            return data;
        }
    }
}
