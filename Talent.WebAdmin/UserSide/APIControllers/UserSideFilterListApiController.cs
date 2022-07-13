using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/userside-filterlist")]
    public class UserSideFilterListApiController : Controller
    {
        private readonly UserSideFilterListService filterMan;
        private readonly UserSideAuthService UserSideMan;

        public UserSideFilterListApiController(UserSideFilterListService service, UserSideAuthService authService)
        {
            this.filterMan = service;
            this.UserSideMan = authService;
        }

        [HttpGet("get-my-team-filter")]
        public async Task<ActionResult<UserSideMyTeamFilterListModel>> GetMyTeamFilterAsync([FromQuery] int? outletPageSize, string outletKeyword, int? positionPageSize, string positionKeyword)
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
                var retdata = new UserSideMyTeamFilterListModel();
                retdata.Outlets = new List<UserSideOutletFilterListModel>();
                retdata.Positions = new List<UserSidePositionFilterListModel>();
                return Ok(retdata);
                //return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            UserSideMyTeamFilterListModel data = new UserSideMyTeamFilterListModel();
            try
            {
                data.Outlets = await this.filterMan.GetUserSideOutletFilterByDealerListAsync(outletPageSize, outletKeyword, response.EmployeeId);

                data.Positions = await this.filterMan.GetUserSidePositionFilterListAsync(positionPageSize, positionKeyword);
            }
            catch (Exception)
            {
                data.Outlets = new List<UserSideOutletFilterListModel>();
                data.Positions = new List<UserSidePositionFilterListModel>();
            }
            

            return data;
        }

        [HttpGet("get-my-rank-filter")]
        public async Task<ActionResult<UserSideMyRankFilterListModel>> GetMyRankFilterAsync([FromQuery] UserSideMyRankFilterModel filter)
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
                var retdata = new UserSideMyRankFilterListModel();
                retdata.Area = new List<UserSideAreaFilterListModel>();
                retdata.Position = new List<UserSidePositionFilterListModel>();
                retdata.Dealer = new List<UserSideDealerFilterListModel>();
                return Ok(retdata);
                //return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            UserSideMyRankFilterListModel data = new UserSideMyRankFilterListModel();

            data.Area = await this.filterMan.GetUserSideAreaFilterListAsync(filter.AreaPageSize, filter.AreaKeyword);

            data.Position = await this.filterMan.GetUserSidePositionFilterListAsync(filter.PositionPageSize, filter.PostionKeyword);

            data.Dealer = await this.filterMan.GetUserSideDealerFilterListAsync(filter.DealerPageSize, filter.DealerKeyword);

            return data;
        }

        [HttpGet("get-general-filter")]
        public async Task<ActionResult<UserSideGeneralFilterListModel>> GetGeneralFilterAsync([FromQuery] UserSideGeneralFilterPageModel filter)
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

            UserSideGeneralFilterListModel data = new UserSideGeneralFilterListModel();
            try
            {
                data.Outlets = await this.filterMan.GetUserSideOutletFilterListAsync(filter.OutletPageSize, null);

                data.Areas = await this.filterMan.GetUserSideAreaFilterListAsync(filter.AreaPageSize, null);

                data.Dealers = await this.filterMan.GetUserSideDealerFilterListAsync(filter.DealerPageSize, null);

                data.Provinces = await this.filterMan.GetUserSideProvinceFilterListAsync(filter.ProvincePageSize, null);

                data.Cities = await this.filterMan.GetUserSideCityFilterListAsync(filter.CityPageSize, null);

                data.Position = await this.filterMan.GetUserSidePositionFilterListAsync(filter.PositionPageSize, null);
            }
            catch (Exception)
            {
                data.Outlets = new List<UserSideOutletFilterListModel>();
                data.Areas = new List<UserSideAreaFilterListModel>();
                data.Dealers = new List<UserSideDealerFilterListModel>();
                data.Provinces = new List<UserSideProvinceFilterListModel>();
                data.Cities = new List<UserSideCityFilterListModel>();
                data.Position = new List<UserSidePositionFilterListModel>();
            }
            return data;
        }

        [HttpGet("get-my-learning-filter")]
        public async Task<ActionResult<UserSideMyLearningFilterListModel>> GetMyLearningFilterAsync()
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

            UserSideMyLearningFilterListModel data = new UserSideMyLearningFilterListModel();

            data.ProgramTypes = await this.filterMan.GetUserSideProgramTypesFilterListAsync(null, null);

            data.LearningTypes = await this.filterMan.GetUserSideLearningTypeFilterListAsync(null, null);

            data.MaterialTypes = await this.filterMan.GetUserSideMaterialTypeFilterListAsync(null, null);

            data.Pricing = this.filterMan.GetUserSidePriceFilterList();

            return data;
        }

        [HttpGet("get-my-news-filter")]
        public async Task<ActionResult<UserSideMyNewsFilterListModel>> GetMyNewsFilterAsync()
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

            UserSideMyNewsFilterListModel data = new UserSideMyNewsFilterListModel();


            data.InternalExternal = this.filterMan.GetUserSideNewsInternalExternalList();

            data.NewsCategories = await this.filterMan.GetUserSideNewsCategoriesFilterListAsync();

            return data;
        }

        [HttpGet("get-my-event-filter")]
        public async Task<ActionResult<UserSideMyEventFilterListModel>> GetMyEventFilterAsync()
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

            UserSideMyEventFilterListModel data = new UserSideMyEventFilterListModel();

            data.EventCategory = await this.filterMan.GetUserSideEventCategoriesList();

            return data;
        }

        [HttpGet("get-my-coach-filter")]
        public async Task<ActionResult<UserSideMyCoachFilterListModel>> GetMyCoachFilterAsync([FromQuery] UserSideMyCoachFilterPageModel model)
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

            var data = new UserSideMyCoachFilterListModel();

            data.Ebadges = await this.filterMan.GetUserSideEBadgesFilterList();
            data.Topics = await this.filterMan.GetUserSideTopicFilterListModels(model.TopicPageSize, model.TopicPageIndex);

            return data;
        }
    }
}
