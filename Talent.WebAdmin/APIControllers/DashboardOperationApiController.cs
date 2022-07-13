using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/dashboard-operation")]
    public class DashboardOperationApiController : Controller
    {
        private readonly DashboardOperationService DashboardOperationServiceMan;

        public DashboardOperationApiController(DashboardOperationService dashboardOperationService)
        {
            this.DashboardOperationServiceMan = dashboardOperationService;
        }

        [HttpGet("get-all-total-users")]
        public async Task<ActionResult<int>> GetTotalUsers()
        {
            var result = await this.DashboardOperationServiceMan.GetTotalUsers();

            return Ok(result);
        }

        [HttpGet("get-all-total-active-users")]
        public async Task<ActionResult<int>> GetTotalActiveUsers()
        {
            var result = await this.DashboardOperationServiceMan.GetTotalActiveUsers();

            return Ok(result);
        }

        [HttpGet("get-all-total-access-rate")]
        public async Task<ActionResult<int>> GetTotalAccessRate()
        {
            var result = await this.DashboardOperationServiceMan.GetTotalAccessRate();

            return Ok(result);
        }

        [HttpGet("get-average-access-time")]
        public async Task<ActionResult<int>> GetAverageAccessTime()
        {
            var result = await this.DashboardOperationServiceMan.GetAverageAccessTime();

            return Ok(result);
        }

        [HttpGet("get-total-learning-enrollment")]
        public async Task<ActionResult<int>> GetTotalLearningEnrollment()
        {
            var result = await this.DashboardOperationServiceMan.GetTotalLearningEnrollment();

            return Ok(result);
        }

        [HttpGet("get-total-certificate-rate")]
        public async Task<ActionResult<int>> GetTotalCertificationRate()
        {
            var result = await this.DashboardOperationServiceMan.GetTotalCertificationRate();

            return Ok(result);
        }

        [HttpGet("get-all-approval-content")]
        public async Task<ActionResult<DashboardApprovalListViewModel>> GetAllApproval()
        {
            var result = await this.DashboardOperationServiceMan.GetApprovalData();

            return Ok(result);
        }

        [HttpGet("get-all-total-users-this-year")]
        public async Task<ActionResult<DashboardUsersThisYearListViewModel>> GetTotalUsersThisYear()
        {
            var result = await this.DashboardOperationServiceMan.GetTotalUsersThisYear();

            return Ok(result);
        }

        [HttpGet("get-two-weeks-class-schedule")]
        public async Task<ActionResult<DashboardClassListViewModel>> GetClassScheduleList()
        {
            var result = await this.DashboardOperationServiceMan.GetClassScheduleList();

            return Ok(result);
        }

        [HttpGet("get-all-tsl-report")]
        public async Task<ActionResult<SetupTSLViewModel>> GetAllTSLData()
        {
            var result = await this.DashboardOperationServiceMan.GetAllTSLReportData();

            return Ok(result);
        }

        [HttpGet("get-all-position-name")]
        public async Task<ActionResult<List<PositionNameModel>>> GetAllPositionName()
        {
            var result = await this.DashboardOperationServiceMan.GetAllPositionName();

            return Ok(result);
        }

        [HttpGet("get-position-competency-mapping")]
        public async Task<ActionResult<DashboardCompetencyMappingModel>> GetPositionCompetencyMapping(string positionName)
        {
            var result = await this.DashboardOperationServiceMan.GetPositionCompetencyMapping(positionName);

            return Ok(result);
        }

        [HttpGet("get-top-5-topics")]
        public ActionResult<DashboardTop5TopicViewModel> GetTop5Topic()
        {
            var result = this.DashboardOperationServiceMan.GetTop5Topic();

            return Ok(result);
        }

        [HttpGet("get-top-5-learnings")]
        public ActionResult<DashboardTop5LearningViewModel> GetTop5Learning()
        {
            var result = this.DashboardOperationServiceMan.GetTop5Learning();

            return Ok(result);
        }

        [HttpGet("get-total-learning-library")]
        public async Task<ActionResult<DashboardTotalLearningLibraryViewModel>> GetTotalLearningLibrary()
        {
            var result = await this.DashboardOperationServiceMan.GetTotalLearningLibrary();

            return Ok(result);
        }

        [HttpGet("get-top-5-news")]
        public async Task<ActionResult<DashboardTop5NewsViewModel>> GetTop5News()
        {
            var result = await this.DashboardOperationServiceMan.GetTop5News();

            return Ok(result);
        }

        [HttpGet("get-top-5-events")]
        public async Task<ActionResult<DashboardTop5EventsViewModel>> GetTop5Events()
        {
            var result = await this.DashboardOperationServiceMan.GetTop5Events();

            return Ok(result);
        }

        [HttpGet("get-top-5-coach")]
        public ActionResult<DashboardTop5CoachViewModel> GetTop5Coach()
        {
            var result = this.DashboardOperationServiceMan.GetTop5Coach();

            return Ok(result);
        }

        [HttpGet("get-top-5-reward-type")]
        public ActionResult<DashboardTop5RewardTypeViewModel> GetTop5RewardType()
        {
            var result = this.DashboardOperationServiceMan.GetTop5RewardType();

            return Ok(result);
        }

        [HttpGet("get-nps-report")]
        public ActionResult<DashboardNPSReportViewModel> GetNPSReport()
        {
            var result = this.DashboardOperationServiceMan.GetNPSReport();

            return Ok(result);
        }

        [HttpGet("get-my-insight")]
        public async Task<ActionResult<DashboardMyInsightViewModel>> GetMyInsight()
        {
            var result = await this.DashboardOperationServiceMan.GetMyInsight();

            return Ok(result);
        }
    }
}
