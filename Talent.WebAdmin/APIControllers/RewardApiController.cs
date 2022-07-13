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
    [Route("api/v1/reward")]
    public class RewardApiController : Controller
    {
        private readonly RewardService rewMan;

        public RewardApiController(RewardService service)
        {
            this.rewMan = service;
        }

        [HttpGet("get-all-reward")]
        public async Task<ActionResult<RewardGridModel>> GetAllRewardsAsync([FromQuery] RewardFilterModel filter)
        {
            var data = await this.rewMan.GetAllRewardAsync(filter);

            return Ok(data);
        }

        [HttpPost("insert-reward")]
        public async Task<ActionResult<bool>> InsertRewardAsync([FromBody] RewardCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.rewMan.InsertRewardAsync(model);

            return Ok(success);
        }

        [HttpGet("get-reward-by-id")]
        public async Task<ActionResult<RewardViewDetailModel>> GetRewardByIdAsync([FromQuery] int rewardId)
        {
            var data = await this.rewMan.GetRewardById(rewardId);

            return Ok(data);
        }

        [HttpPost("update-reward")]
        public async Task<ActionResult<bool>> UpdateRewardAsync([FromBody] RewardUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.rewMan.UpdateRewardAsync(model);

            return Ok(success);
        }

        [HttpPost("delete-reward")]
        public async Task<ActionResult> DeleteRewardAsync([FromQuery] int rewardId)
        {
            await this.rewMan.DeleteRewardAsync(rewardId);

            return Ok();
        }
    }
}
