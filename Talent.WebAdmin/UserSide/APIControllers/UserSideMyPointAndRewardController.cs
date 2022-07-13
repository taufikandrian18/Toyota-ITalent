using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    /// <summary>
    /// User Side My Point and Reward Web API controller for providing Web APIs such as retrieve Point and Reward data.
    /// </summary>
    [Route("api/v1/userside-point-reward")]
    public class UserSideMyPointAndRewardController : Controller
    {
        private readonly UserSideMyPointAndRewardService Service;
        private readonly UserSideAuthService AuthService;

        public UserSideMyPointAndRewardController(UserSideMyPointAndRewardService userSideMyPointAndRewardService, UserSideAuthService authService)
        {
            this.Service = userSideMyPointAndRewardService;
            this.AuthService = authService;
        }

        /// <summary>
        /// Get point for spesific employee.
        /// </summary>
        /// <returns></returns>
        [HttpGet("point")]
        public async Task<ActionResult<UserSideMyPointModel>> GetPoint()
        {
            // Get employeeId from token
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response =  this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                var retdata = new UserSideMyPointModel();
                return Ok(retdata);
                //return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.Service.GetPointByEmployeeId(response.EmployeeId);

            return Ok(data);
        }

        /// <summary>
        /// Get all history point.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("history-points")]
        public async Task<ActionResult<UserSideHistoryPointModel>> GetHistoryPoints()
        {
            // Get employeeId from token
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

            var data = await this.Service.GetHistoryPoint(response.EmployeeId);

            return Ok(data);

        }

        /// <summary>
        /// Get all rewards.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("rewards")]
        public async Task<ActionResult<List<UserSideRewardModel>>> GetRewards([FromQuery]UserSideRewardFilterModel filter)
        {
            if(filter.PageIndex < 1)
            {
                filter.PageIndex = 1;
            }

            var data = await this.Service.GetRewards(filter);

            return Ok(data);
        }

        /// <summary>
        /// Detail reward.
        /// </summary>
        /// <param name="rewardId"></param>
        /// <returns></returns>
        [HttpGet("detail-reward/{rewardId}")]
        public async Task<ActionResult<UserSideRewardModel>> GetDetailReward(int rewardId)
        {
            var data = await this.Service.GetDetailReward(rewardId);

            return Ok(data);
        }

        /// <summary>
        /// redeem point reward for spesific employee and spesific reward.
        /// </summary>
        /// <param name="rewardId"></param>
        /// <returns></returns>
        [HttpPost("redeem-reward/{rewardId}")]
        public async Task<ActionResult<UserSideMyPointModel>> GetPoint(int rewardId)
        {
            // Get employeeId from token
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

            var data = await this.Service.RedeemReward(response.EmployeeId, rewardId);

            if (!string.IsNullOrEmpty(data))
            {
                return BadRequest(data);
            }
            return Ok();
        }
    }
}