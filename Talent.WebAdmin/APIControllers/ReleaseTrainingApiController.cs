using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/release-training")]
    public class ReleaseTrainingApiController : Controller
    {
        private readonly ReleaseTrainingService ReleaseTrainingMan;

        public ReleaseTrainingApiController(ReleaseTrainingService releaseTrainingService)
        {
            this.ReleaseTrainingMan = releaseTrainingService;
        }

        [HttpGet("get-course-setup-learning", Name = "get-course-setup-learning")]
        public async Task<ActionResult<List<CourseReleaseTrainingModel>>> GetCourseSetupLearning([FromQuery]string courseName)
        {
            var result = await this.ReleaseTrainingMan.GetCourseReleaseTraining(courseName);

            return Ok(result);
        }

        [HttpGet("get-all-course-setup-learning", Name = "get-all-course-setup-learning")]
        public async Task<ActionResult<List<CourseReleaseTrainingModel>>> GetAllCourseSetupLearning(string courseName)
        {
            var result = await this.ReleaseTrainingMan.GetAllCourseReleaseTraining(courseName);

            return Ok(result);
        }

        [HttpGet("get-approval-status", Name = "get-approval-status")]
        public async Task<ActionResult<List<ApprovalStatusViewModels>>> GetApprovalStatus()
        {
            var result = await this.ReleaseTrainingMan.GetAllApprovalStatus();

            return Ok(result);
        }

        [HttpGet("get-setup-module-relase/{courseId}", Name = "get-setup-module-release")]
        public async Task<ActionResult<List<ReleaseTrainingSetupModuleModel>>> GetSetupModuleRelease(int courseId)
        {
            var result = await this.ReleaseTrainingMan.GetAllSetupModuleReleaseResult(courseId);

            return Ok(result);
        }


        [HttpGet("get-batch/{courseId}", Name = "get-batch")]
        public async Task<ActionResult<int>> GetBatchReleaseTraining(int courseId)
        {
            var result = await this.ReleaseTrainingMan.GetBatchReleaseTraining(courseId);

            return Ok(result);
        }

        [HttpPost("insert-relase-training", Name = "insert-release-training")]
        public async Task<ActionResult<bool>> InsertReleaseTraining([FromBody] ReleaseTrainingFormModel model)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest("Model State is not valid");
            }

            var result = await this.ReleaseTrainingMan.SetupRelaseTraining(model);

            if (result == false)
            {
                return BadRequest("Failed to Insert!");
            }

            return Ok(result);
        }

        [HttpGet("get-coach-for-release-training/{coachName}", Name = "get-coach-for-release-training")]
        public async Task<ActionResult<List<CoachForReleaseTraining>>> GetCoachForReleaseTraining(string coachName)
        {
            var result = await this.ReleaseTrainingMan.GetCoachForReleaseTraining(coachName);

            return Ok(result);
        }

        [HttpGet("get-teaching-time-points", Name = "get-teaching-time-points")]
        public async Task<ActionResult<List<TeachingTimepPointsModel>>> GetTeachingTimePoints()
        {
            var result = await this.ReleaseTrainingMan.GetAllTeachingTimePoints();

            return Ok(result);
        }

        [HttpGet("get-all-position-by-outlet", Name = "get-all-position-by-outlet")]
        public async Task<ActionResult<List<PositionViewModel>>> GetAllPosition([FromHeader] string outletId)
        {
            var result = await this.ReleaseTrainingMan.GetAllPositionByOutletIds(outletId);

            return Ok(result);
        }

        [HttpGet("get-all-position", Name = "get-all-position")]
        public async Task<ActionResult<List<PositionViewModel>>> GetAllPosition()
        {
            var result = await this.ReleaseTrainingMan.GetAllPosition();

            return Ok(result);
        }

        [HttpGet("get-all-area", Name = "get-all-area")]
        public async Task<ActionResult<List<AreaViewModel>>> GetAllArea()
        {
            var result = await this.ReleaseTrainingMan.GetAllArea();

            return Ok(result);
        }

        [HttpGet("get-all-dealer", Name = "get-all-dealer")]
        public async Task<ActionResult<List<DealerViewModel>>> GetAllDealer()
        {
            var result = await this.ReleaseTrainingMan.GetAllDealer();
            return Ok(result);
        }

        [HttpGet("get-all-province", Name = "get-all-province")]
        public async Task<ActionResult<List<ProvinceViewModel>>> GetAllProvince()
        {
            var result = await this.ReleaseTrainingMan.GetAllProvince();
            return Ok(result);
        }

        [HttpGet("get-all-city", Name = "get-all-city")]
        public async Task<ActionResult<List<CityViewModel>>> GetAllCity()
        {
            var result = await this.ReleaseTrainingMan.GetAllCity();
            return Ok(result);
        }

        [HttpGet("get-all-outlet", Name = "get-all-outlet")]
        public async Task<ActionResult<List<OutletCompleteViewModel>>> GetAllOutlet()
        {
            var result = await this.ReleaseTrainingMan.GetAllOutlet();
            return Ok(result);
        }

        [HttpGet("get-outlet-filtered", Name = "get-outlet-filtered")]
        public async Task<ActionResult<List<OutletViewModel>>> GetOutletFiltered([FromHeader] string filterJson)
        {
            var result = await this.ReleaseTrainingMan.GetOutletFiltered(filterJson);
            return Ok(result);
        }

        [HttpGet("get-outlet-filtered-id", Name = "get-outlet-filtered-id")]
        public async Task<ActionResult<List<OutletViewModel>>> GetOutletFilteredId([FromHeader] string filterJson)
        {
            var result = await this.ReleaseTrainingMan.GetOutletFilteredIds(filterJson);
            return Ok(result);
        }

        [HttpGet("get-release-training", Name = "get-release-training")]
        public async Task<ActionResult<ReleaseTrainingViewModel>> GetReleaseTrainingViewModel([FromQuery] ReleaseTrainingFilter filter)
        {
            var result = await this.ReleaseTrainingMan.GetReleaseTrainingFiltered(filter);
            return Ok(result);
        }

        //this is new line for tweaking get release training by dealer id
        [HttpGet("get-release-training-by-dealer", Name = "get-release-training-by-dealer")]
        public async Task<ActionResult<ReleaseTrainingViewModel>> GetReleaseTrainingViewByDealerModel([FromQuery] ReleaseTrainingByDealerFilter filter)
        {
            var result = await this.ReleaseTrainingMan.GetReleaseTrainingByDealerFiltered(filter);
            return Ok(result);
        }


        [HttpGet("get-related-release-training", Name = "get-related-release-training")]
        public async Task<ActionResult<ReleaseTrainingViewModel>> GetRelatedReleaseTrainingViewModel([FromQuery] ReleaseTrainingFilter filter)
        {
            var result = await this.ReleaseTrainingMan.GetRelatedReleaseTrainingFiltered(filter);
            return Ok(result);
        }

        [HttpGet("get-release-training-detail/{trainingId}", Name = "get-release-training-detail")]
        public async Task<ActionResult<RelaseTrainingDetailModel>> GetReleaseTrainingById(int trainingId)
        {
            var result = await this.ReleaseTrainingMan.GetTrainingDetailById(trainingId);
            return Ok(result);
        }

        [HttpPost("update-release-training", Name = "update-release-training")]
        public async Task<ActionResult<bool>> UpdateReleaseTraining([FromBody] ReleaseTrainingFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model State is not valid");
            }

            var result = await this.ReleaseTrainingMan.UpdateReleaseTraining(model);

            if (result == false)
            {
                return BadRequest("Failed to insert");
            }

            return Ok(result);
        }

        [HttpDelete("delete-release-training/{trainingId}", Name = "delete-release-training")]
        public async Task<ActionResult<bool>> DeleteReleaseTraining(int trainingId)
        {
            var result = await this.ReleaseTrainingMan.RemoveReleaseTraining(trainingId);
            if (result == false)
            {
                return BadRequest("Failed to remove");
            }

            return Ok(result);
        }

        [HttpGet("get-total-detail-course/{courseId}", Name = "get-total-detail-course")]
        public async Task<ActionResult<TotalCourseDetail>> GetTotalDetailCourse(int courseId)
        {
            var result = await this.ReleaseTrainingMan.GetTotalCourseDetail(courseId);

            return Ok(result);
        }

    }
}
