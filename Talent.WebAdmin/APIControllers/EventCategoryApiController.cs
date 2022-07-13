using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/eventCategory")]
    public class EventCategoryApiController : Controller
    {
        private readonly EventCategoryService EventCategoryMan;

        public EventCategoryApiController(EventCategoryService eventCategoryService)
        {
            this.EventCategoryMan = eventCategoryService;
        }

        [HttpGet("get-all-event-categories", Name = "get-all-event-categories")]
        public async Task<ActionResult<EventCategoryViewModel>> GetAllEventCategories([FromQuery] EventCategoryFilter filter)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await this.EventCategoryMan.GetAllEventCategories(filter);
            return Ok(result);
        }

        [HttpGet("get-all-event-categories-no-filter", Name = "get-all-event-categories-no-filter")]
        public async Task<ActionResult<List<EventCategoryModel>>> GetAllEventCategoriesNoFilter()
        {
            var result = await this.EventCategoryMan.GetAllEventCategoriesNoFilter();
            return Ok(result);
        }

        [HttpGet("get-event-category-by-id/{id}", Name = "get-event-category-by-id")]
        public async Task<ActionResult<EventCategoryModel>> GetEventCategoryById(int id)
        {
            var result = await EventCategoryMan.GetEventCategoryById(id);
            return Ok(result);
        }

        [HttpPost("create", Name = "create-event-category")]
        public async Task<ActionResult<int>> CreateEventCategory([FromBody] EventCategoryFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await EventCategoryMan.CreateEventCategory(model);

            if(result == null)
            {
                return BadRequest("Event Category Name already exist");
            }

            return Ok(result);
        }

        [HttpPost("update", Name = "update-event-category")]
        public async Task<ActionResult<bool>> UpdateEventCategory([FromBody] EventCategoryFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var result = await EventCategoryMan.UpdateEventCategory(model);
            if (result == false)
            {
                return BadRequest("Data not available");
            }
            return Ok(result);
        }

        [HttpDelete("delete", Name = "delete-event-category")]
        public async Task<ActionResult<bool>> DeleteEventCategory([FromBody] int id)
        {
            var result = await EventCategoryMan.DeleteEventCategory(id);
            return Ok(result);
        }
    }
}
