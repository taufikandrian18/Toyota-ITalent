using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-mylearning")]
    public class UserSideMyLearningController : Controller
    {
        private readonly UserSideMyLearningService LearningMan;
        private readonly UserSideAuthService UserSideMan;
        private readonly UserActivityService UserActivityMan;

        public UserSideMyLearningController(UserSideMyLearningService service, UserSideAuthService userSide, UserActivityService userActivityLogService)
        {
            this.LearningMan = service;
            this.UserSideMan = userSide;
            this.UserActivityMan = userActivityLogService;
        }


        [HttpGet("get-training-by-id")]
        public async Task<ActionResult<UserSideCourseViewModel>> GetTrainingByIdAsync([FromQuery] int trainingId, int courseId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                var retdata = new UserSideCourseViewModel
                {
                    TopicName = new List<string>()
                };
                return Ok(retdata);
                //return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetUserSideCourseAsync(response.EmployeeId, trainingId, courseId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Training";
            log.Content = data.TrainingName;
            log.ContentId = data.TrainingId;

            //await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }

        [HttpGet("get-module-by-id")]
        public async Task<ActionResult<UserSideModuleViewModel>> GetModuleByIdAsync([FromQuery] int setupModuleId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetUserSideModuleAsync(response.EmployeeId, setupModuleId);
            if (data == null)
            {
                data = new UserSideModuleViewModel();
                data.TopicNames = new List<string>();
            }

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "My Learning";
            log.Content = "Module " + data.ModuleName;

            //await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }

        /// <summary>
        /// to get all courses from database (SetupModules table)
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-trainings")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageCourseModel>>> GetAllTrainingAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllUserSideTrainings(pageSize, pageIndex);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "My Learning";
            log.Content = "All Training";

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }

        [HttpGet("get-all-popular-trainings")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageCourseModel>>> GetAllPopularTrainingAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetPaginatedResultUserSidePopularTrainings(pageSize, pageIndex, response.EmployeeId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "My Learning";
            log.Content = "Popular Training";

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }


        [HttpGet("get-all-latest-trainings")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageCourseModel>>> GetAllLatestTrainingAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetPaginatedResultUserSideLatestTrainings(pageSize, pageIndex, response.EmployeeId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "My Learning";
            log.Content = "Latest Training";

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }

        [HttpGet("get-all-pop-quiz-by-setup-learning-id")]
        public async Task<ActionResult<List<UserSidePopQuizModel>>> GetAllPopQuizFromSetupLearning(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllPopQuizFromSetupLearning(pageSize, pageIndex);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "My Learning";
            log.Content = "Pop Quiz";

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }

        /// <summary>
        /// insert form task answer untuk pop quiz
        /// </summary>
        /// <param name="insert"></param>
        /// <returns></returns>
        [HttpPost("insert-task-answer")]
        public async Task<ActionResult<bool>> InsertTaskAnswer([FromBody] TaskAnswerInsertModel insert)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }
            var employeeId = response.EmployeeId;
            var data = await this.LearningMan.InsertTaskAnswer(insert, employeeId);
            return Ok(data);
        }

        [HttpGet("get-all-recommended-trainings")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageCourseModel>>> GetAllRecommendedTrainingAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllUserSideRecommendedTrainings(pageSize, pageIndex, response.EmployeeId);
            if (data == null)
            {
                data = new List<UserSideMyLearningHomepageCourseModel>();
            }

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "My Learning";
            log.Content = "Recommended Training";

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }

        [HttpGet("get-all-queued-trainings")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageCourseModel>>> GetAllQueuedTrainingAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllUserSideQueuedTrainings(response.EmployeeId, pageSize, pageIndex);

            return Ok(data);
        }

        [HttpGet("get-all-continued-trainings")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageCourseModel>>> GetAllContinuedTrainingAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllUserSideContinueTrainings(response.EmployeeId, pageSize, pageIndex);

            return Ok(data);
        }

        [HttpGet("get-all-completed-trainings")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageCourseModel>>> GetAllCompletedTrainingAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllUserSideCompleteTrainings(response.EmployeeId, pageSize, pageIndex);

            return Ok(data);
        }

        /// <summary>
        /// untuk mengambil module yang popular
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("get-all-popular-modules")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageModuleModel>>> GetAllPopularModulesAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllUserSidePopularModules(pageSize, pageIndex);

            return Ok(data);
        }

        /// <summary>
        /// untuk mengambil module yang direkomendasi
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("get-all-recommended-modules")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageModuleModel>>> GetAllRecommendedModulesAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllUserSideRecommendedModules(pageSize, pageIndex);

            return Ok(data);
        }

        /// <summary>
        /// untuk mengambil module yang user telah di antri
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("get-all-queued-modules")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageModuleModel>>> GetAllQueuedModulesAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllUserSideQueuedModules(response.EmployeeId, pageSize, pageIndex);

            return Ok(data);
        }

        /// <summary>
        /// untuk mengambil module yang user sedang pelajari
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("get-all-continued-modules")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageModuleModel>>> GetAllContinuedModulesAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllUserSideContinueModules(response.EmployeeId, pageSize, pageIndex);

            return Ok(data);
        }

        /// <summary>
        /// untuk mengambil module yang telah diselesaikan oleh user
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("get-all-completed-modules")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageModuleModel>>> GetAllCompletedModulesAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllUserSideCompleteModules(response.EmployeeId, pageSize, pageIndex);

            return Ok(data);
        }

        [HttpGet("get-all-queued-badges")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageCourseBadgeModelNew>>> GetAllQueuedBadgesAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllUserSideQueuedBadges(response.EmployeeId, pageSize, pageIndex);

            return Ok(data);
        }

        [HttpGet("get-all-continued-badges")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageCourseBadgeModelNew>>> GetAllContinuedBadgesAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllUserSideContinueBadges(response.EmployeeId, pageSize, pageIndex);

            return Ok(data);
        }

        [HttpGet("get-all-completed-badges")]
        public async Task<ActionResult<List<UserSideMyLearningHomepageCourseBadgeModelNew>>> GetAllCompletedBadgesAsync(int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetAllUserSideCompleteBadges(response.EmployeeId, pageSize, pageIndex);

            return Ok(data);
        }

        [HttpGet("get-course-overview")]
        public async Task<ActionResult<UserSideCourseOverviewModel>> GetCourseOverviewAsync([FromQuery] int courseId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                var retdata = new UserSideCourseOverviewModel
                {
                    LearningObjs = new List<string>(),
                    Prerequisites = new List<UserSidePrerequisiteModel>()
                };

                return Ok(retdata);
                //return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetCourseOverview(courseId, response.EmployeeId);
            if (data == null)
            {
                data = new UserSideCourseOverviewModel
                {
                    LearningObjs = new List<string>(),
                    Prerequisites = new List<UserSidePrerequisiteModel>()
                };
            }

            return Ok(data);
        }

        [HttpGet("get-course-training-scheme")]
        public async Task<ActionResult<UserSideCourseTrainingViewModel>> GetCourseTrainingSchemeAsync([FromQuery] int courseId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetTrainingScheme(courseId);
            if (data == null)
            {
                data = new List<UserSideCourseTrainingViewModel>();
            }

            return Ok(data);
        }

        [HttpGet("get-course-coach")]
        public async Task<ActionResult<UserSideCourseCoachViewModel>> GetCourseCoachAsync([FromQuery] int trainingId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetCoach(trainingId);
            if (data == null)
            {
                data = new List<UserSideCourseCoachViewModel>();
            }

            return Ok(data);
        }

        [HttpGet("get-people-who-like-the-course-list")]
        public async Task<ActionResult<UserSideCourseLikePeopleListModel>> GetPeopleWhoLikeTheCourse([FromQuery] int trainingId, int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                var retdata = new List<UserSideCourseLikePeopleListModel>();
                return Ok(retdata);

                //return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetPeopleWhoLikeTheCourseList(trainingId, pageSize, pageIndex);

            return Ok(data);
        }

        [HttpGet("get-people-who-like-the-course-sample-image")]
        public async Task<ActionResult<UserSideCourseLikePeopleSampleImage>> GetPeopleWhoLikeTheCourseSampleImage([FromQuery] int trainingId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetPeopleWhoLikeTheCourseSampleImage(trainingId);

            return Ok(data);
        }

        [HttpGet("get-people-who-like-the-course-count")]
        public async Task<ActionResult<int>> GetPeopleWhoLikeCourseCount([FromQuery] int trainingId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetLike(trainingId);

            return Ok(data);
        }

        [HttpGet("get-people-who-took-the-course-sample-image")]
        public async Task<ActionResult<UserSideWhoTookTheCourseSampleImage>> GetPeopleWhoTookTheCourseSampleImage([FromQuery] int trainingId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetPeopleWHoTookTheCourseSampleImage(trainingId);

            return Ok(data);
        }

        [HttpGet("get-people-who-took-the-course-list")]
        public async Task<ActionResult<UserSidePeopleWhoTookTheCourseListModel>> GetPeopleWhoTookTheCourseList([FromQuery] int trainingId, int pageSize, int pageIndex)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                var retdata = new List<UserSidePeopleWhoTookTheCourseListModel>();

                return Ok(retdata);
                //return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetPeopleWhoTookTheCourseList(trainingId, pageSize, pageIndex);

            return Ok(data);
        }

        [HttpGet("get-peole-who-took-the-course-count")]
        public async Task<ActionResult<int>> GetPeopleWhoTookTheCourseCount([FromQuery] int trainingId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetTotalAssignee(trainingId);

            return Ok(data);
        }

        [HttpGet("get-module-content")]
        public async Task<ActionResult<ModuleContentViewModel>> GetModuleContentAsync([FromQuery] int? trainingId, int? setupModuleId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetCourseContent(response.EmployeeId, trainingId, null, setupModuleId);

            if (data.IsEnrolled == false)
            {
                return Ok(data.Item1);
            }

            return Ok(data.Item1);

        }

        [HttpGet("get-module-isRemed")]
        public async Task<ActionResult<BaseResponse>> GetModuleContentIsRemedAsync([FromQuery] int trainingId, int setupModuleId, string SkillCheckGuid)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetModuleIsRemed(trainingId, setupModuleId, response.EmployeeId, SkillCheckGuid);

            return data;

        }

        [HttpGet("get-module-content-pagination")]
        public async Task<ActionResult<ModuleContentViewPaginationModel>> GetModuleContentPaginationAsync([FromQuery] int? trainingId, int? setupModuleId, int? index)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetCourseContentPagination(response.EmployeeId, trainingId, null, setupModuleId, index);

            if (data.IsEnrolled == false)
            {
                return Ok(null);
            }

            return Ok(data.Item1);
        }

        [HttpGet("get-module-review")]
        public async Task<ActionResult<CourseReviewViewModel>> GetModuleReviewAsync([FromQuery] int trainingId, int limit, int page)
        {
            if (limit == 0)
            {
                limit = 10;
            }

            if (page == 0)
            {
                page = 1;
            }

            page = (page - 1) * limit;

            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetCourseReview(trainingId, limit, page);
            if (data == null)
            {
                data = new CourseReviewSummaryViewModel
                {
                    Reviews = new List<CourseReviewViewModel>()
                };
            }

            return Ok(data);
        }

        //[HttpGet("get-member-data")]
        //public async Task<ActionResult<TeamMemberDetail>> GetTeamMemberAsync([FromQuery] int trainingId)
        //{
        //    var data = await this.LearningMan.GetTeamMember(trainingId);

        //    return Ok(data);
        //}


        [HttpPost("post-member-assign-training")]
        public async Task<ActionResult> PostAssignTrainingMemberAsync([FromBody] TeamMemberAssignModel model)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            await this.LearningMan.AssignTrainingMember(response.EmployeeId, model);

            return Ok();
        }

        [HttpPost("post-member-assign-module")]
        public async Task<ActionResult> PostAssignModuleMemberAsync([FromBody] TeamMemberAssignModel model)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            await this.LearningMan.AssignModuleMember(response.EmployeeId, model);

            return Ok();
        }

        [HttpGet("get-member-filter")]
        public async Task<ActionResult<TeamMemberDetail>> GetAssignMemberAsync([FromQuery] MemberGridFilter filter)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.SearchMember(filter, response.EmployeeId);

            return Ok(data);
        }

        [HttpGet("get-training-module")]
        public async Task<ActionResult<GetModule>> GetTrainingModule([FromQuery] int moduleId, int setupModuleId, int trainingId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }
            var data = await this.LearningMan.GetModuleContent(response.EmployeeId, moduleId, setupModuleId, trainingId);
            if (data == null)
            {
                data = new GetModule();
            }

            return Ok(data);
        }

        //[HttpGet("get-file/{blobId}")]
        //public async Task<IActionResult> GetFileFromMinio(string blobId)
        //{
        //    var getFile = await this.LearningMan.DownloadFile(blobId);

        //    return File(getFile.Item1,getFile.Item3);
        //}

        [HttpPost("post-enroll-training")]
        public async Task<ActionResult> PostEnrollTrainingModule([FromQuery] EnrollQueueModel model)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var isEligibleResponse = await this.LearningMan.IsEligible(response.EmployeeId, model.TrainingId);

            if (isEligibleResponse == false)
            {
                return BadRequest(ResponseMessageEnum.FailedBaseString + "tidak bisa enroll di karenakan tidak sesuai dengan posisi release course");
            }

            var enrollResponse = await this.LearningMan.EnrollTraining(response.EmployeeId, model);

            if (enrollResponse == false)
            {
                return BadRequest(ResponseMessageEnum.FailedBaseString + "Sudah Pernah di Enrolled atau Prerequisite Belum Terpenuhi atau Quota Training Full");
            }

            return Ok();
        }

        [HttpPost("post-cancel-enroll-training")]
        public async Task<ActionResult> PostCancelEnrollTrainingModule([FromQuery] EnrollQueueModel model)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            //var enrollResponse = await this.LearningMan.CancelEnrollTraining(response.EmployeeId, model);

            //if (enrollResponse == false)
            //{
            //    return BadRequest(ResponseMessageEnum.FailedBaseString);
            //}

            var enrollResponse = false;

            return Ok(enrollResponse);
        }

        [HttpPost("post-enroll-module")]
        public async Task<ActionResult> PostEnrollModule([FromQuery] int setupModuleId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var enrollResponse = await this.LearningMan.EnrollModule(response.EmployeeId, setupModuleId);

            //var isEligibleResponse = await this.LearningMan.IsEligibleBySetupModule(response.EmployeeId, setupModuleId);

            if (enrollResponse == false)
            {
                return BadRequest(ResponseMessageEnum.FailedBaseString + "Sudah Pernah di Enrolled");
            }

            //if (isEligibleResponse == false)
            //{
            //    return BadRequest(ResponseMessageEnum.FailedBaseString + "tidak bisa enroll di karenakan tidak sesuai dengan posisi release course");
            //}

            return Ok();
        }


        [HttpPost("post-cancel-enroll-module")]
        public async Task<ActionResult> PostCancelEnrollModule([FromQuery] int setupModuleId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var enrollResponse = await this.LearningMan.CancelEnrollModule(response.EmployeeId, setupModuleId);

            if (enrollResponse == false)
            {
                return BadRequest(ResponseMessageEnum.FailedBaseString);
            }

            return Ok(enrollResponse);
        }

        [HttpPost("post-add-to-queue-training")]
        public async Task<ActionResult> PostQueueModule([FromQuery] EnrollQueueModel model)
        {

            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            await this.LearningMan.AddToQeueu(response.EmployeeId, model);

            return Ok();
        }

        [HttpPost("post-unqueue-training")]
        public async Task<ActionResult> PostUnqueueModule([FromQuery] EnrollQueueModel model)
        {

            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            await this.LearningMan.UnqueueTraining(response.EmployeeId, model);

            return Ok();
        }

        [HttpPost("update-queue-to-enroll-training")]
        public async Task<ActionResult> UpdateQueueToEnrollModule([FromQuery] EnrollQueueModel model)
        {

            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            await this.LearningMan.UpdateQueueToEnroll(response.EmployeeId, model);

            return Ok();
        }

        [HttpPost("start-training-module")]
        public async Task<ActionResult> StartTrainingModule([FromBody] CheckModuleStartModel model)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            return Ok(await this.LearningMan.StartModule(response.EmployeeId, model));
        }

        [HttpGet("get-ispass-by-id")]
        public async Task<ActionResult<UserSideIsLearningPassModel>> GetUserIsPassByIdAsync([FromQuery] GetIsPassModel model)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            UserSideIsLearningPassModel responseReturn = new UserSideIsLearningPassModel();

            if (model.TrainingId > 0)
            {
                responseReturn = await LearningMan.GetUserIsPassByTrainingIdAsync(response.EmployeeId, model.TrainingId);
            }
            else
            if (model.SetupModuleId > 0)
            {
                responseReturn = await LearningMan.GetUserIsPassBySetupModulIdAsync(response.EmployeeId, model.SetupModuleId);
            }
            if (responseReturn == null) return NotFound();
            return Ok(responseReturn);
        }

        [HttpGet("get-score-Module-list-by-id")]
        public async Task<ActionResult<List<ModuleScoreModel>>> GetScoreModuleByTrainingIdAsync([FromQuery] int trainingId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var responseModel = await LearningMan.GetListScoreModuleByTrainingIdAsync(response.EmployeeId, trainingId);

            return responseModel;
        }

        [HttpGet("get-count-training")]
        public async Task<ActionResult<BaseResponse>> GetCountTraining()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.LearningMan.GetCountTraining(response.EmployeeId);

            return Ok(data);
        }

    }
}
