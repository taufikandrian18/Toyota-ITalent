using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-rank")]
    public class UserSideRankController : ControllerBase
    {
        private readonly UserSideRankService RankMan;
        private readonly UserSideAuthService AuthService;

        public UserSideRankController(UserSideRankService service, UserSideAuthService authService)
        {
            this.RankMan = service;
            this.AuthService = authService;
        }

        [HttpGet("get-rank-coach")]
        public async Task<ActionResult<UserSideRankModel>> GetRankCoachAsync()
        {
            var data = await this.RankMan.GetRankCoach();

            return Ok(data);
        }

        [HttpGet("positions")]
        public async Task<ActionResult<UserSidePositionModel>> GetPositionsAsync(int itemPerPage, int pageIndex)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            var data = await this.RankMan.GetPositions(itemPerPage, pageIndex);

            return Ok(data);
        }

        [HttpGet("areas")]
        public async Task<ActionResult<UserSideAreaModel>> GetAreasAsync(int itemPerPage, int pageIndex)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            var data = await this.RankMan.GetAreas(itemPerPage, pageIndex);

            return Ok(data);
        }

        [HttpGet("dealers")]
        public async Task<ActionResult<UserSideDealerModel>> GetDealersAsync(int itemPerPage, int pageIndex)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            var data = await this.RankMan.GetDealers(itemPerPage, pageIndex);

            return Ok(data);
        }

        [HttpGet("get-rank-all-user-by-token")]
        public async Task<ActionResult<UserSideRankModel>> GetRankAllUserByTokenAsync([FromQuery]UserSideRankFilterModel filter)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            //var dataFilter = JsonConvert.DeserializeObject<UserSideRankFilterModel>(filter);
            
            // Check validation IsAllTime. If IsAllTime is True, this API use method GetRankAllUserAllTime 
            if(filter.IsAllTime == true)
            {
                var data = await this.RankMan.GetRankAllUserAllTime(filter, response.EmployeeId);
                return Ok(data);
            }

            // If IsAllTime is False, this API use method GetRankAllUserCurrentYear.
            if (filter.IsAllTime == false)
            {
                var data = await this.RankMan.GetRankAllUserCurrentYear(filter, response.EmployeeId);
                return Ok(data);
            }
            return BadRequest("Please check your data. IsAllTime null");
        }

        [HttpGet("get-rank-user-by-token")]
        public async Task<ActionResult<UserSideRankModel>> GetRankUserByTokenAsync()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.RankMan.GetRankUser(response.EmployeeId);

            return Ok(data);
        }
    }
}