using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/topic")]
    public class TopicApiController : Controller
    {
        private readonly TopicService TopicMan;

        public TopicApiController(TopicService topicService)
        {
            this.TopicMan = topicService;
        }

        [HttpGet("get-all-topic", Name = "GetTopicTableAsync")]
        public async Task<ActionResult<GridTopicModel>> GetAsync([FromQuery] TopicGridFilter filter)
        {
            var data = await this.TopicMan.GetAllTopicsAsync(filter);

            return Ok(data);
        }

        [HttpGet("validate-topic-name", Name = "ValidateTopicByNameAsync")]
        public async Task<ActionResult<bool>> GetAsync([FromQuery] string name)
        {
            var isExist = await this.TopicMan.ValidateTopicByNameAsync(name);

            return Ok(isExist);
        }

        [HttpPost("create-new-topic", Name = "InsertTopicAsync")]
        public async Task<ActionResult> PostAsync([FromBody] TopicCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var isSuccess = await this.TopicMan.InsertTopicAsync(createModel);

            if (isSuccess == 0)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("get-topic-by-id", Name = "GetTopicByIdAsync")]
        public async Task<ActionResult<TopicViewDetailModel>> GetIdAsync([FromQuery] int topicId)
        {
            var data = await this.TopicMan.GetTopicByIdAsync(topicId);

            return Ok(data);
        }

        [HttpGet("validate-update-topic-name", Name = "ValidateUpdateTopicNameAsync")]
        public async Task<ActionResult<bool>> GetAsync([FromQuery] string topicName, int topicId)
        {
            var isExistAndChanged = await this.TopicMan.ValidateUpdateTopicNameAsync(topicName, topicId);

            return Ok(isExistAndChanged);
        }

        [HttpPut("update-topic", Name = "UpdateTopicAsync")]
        public async Task<ActionResult> UpdateAsync([FromBody] TopicUpdateModel updateModel, int topicId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var isSuccess = await this.TopicMan.UpdateTopicAsync(updateModel, topicId);

            if (isSuccess == false)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("delete-topic", Name = "DeleteTopicByIdAsync")]
        public async Task<ActionResult<bool>> DeleteAsync([FromQuery] int topicId)
        {
            var isSuccess = await this.TopicMan.DeleteTopicByIdAsync(topicId);

            return Ok(isSuccess);
        }

        [HttpGet("get-option", Name = "GetAllOptionTopicAsync")]
        public async Task<ActionResult<List<TopicCoachOptionModel>>> GetOptionAsync()
        {
            var data = await this.TopicMan.GetAllTopicOption();

            return Ok(data);
        }

        [HttpGet("get-ebadge-option", Name = "GetTopicEbadgeOptionAsync")]
        public async Task<ActionResult<List<TopicEbadgeOptionModel>>> GetEbadgeOption()
        {
            var data = await this.TopicMan.GetEbadgeOption();

            return Ok(data);
        }
    }
}
