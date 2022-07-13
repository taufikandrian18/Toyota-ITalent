using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/event")]
    public class EventApiController : Controller
    {
        private readonly EventService EventMan;
        private readonly EventJoinService EventJoinMan;
        
        public EventApiController(EventService eventService, EventJoinService eventJoinService)
        {
            this.EventMan = eventService;
            this.EventJoinMan = eventJoinService;
        }

        [HttpGet("get-all-join-events", Name = "get-all-join-events")]
        public async Task<ActionResult<EventJoinViewModel>> GetAllJoinEvents([FromQuery] EventFilter filter)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }
            
            var result = await this.EventJoinMan.GetAllEvents(filter);
            return Ok(result);
        }

        [HttpGet("get-join-event-by-id/{id}", Name = "get-join-event-by-id")]
        public async Task<ActionResult<EventJoinModel>> GetJoinEventById(int id)
        {
            var result = await EventJoinMan.GetEventJoinById(id);
            return Ok(result);
        }

        [HttpPost("create", Name = "create-event")]
        public async Task<ActionResult<int>> CreateEvent([FromBody] EventFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var result = await EventMan.CreateEvent(model);
            return Ok(result);
        }

        [HttpPost("update", Name = "update-event")]
        public async Task<ActionResult<bool>> UpdateEvent([FromBody] EventJoinModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await EventMan.UpdateEvent(model);
            if (result == false)
            {
                return BadRequest("Data not available");
            }
            return Ok(result);
        }

        [HttpDelete("delete", Name = "delete-event")]
        public async Task<ActionResult<bool>> DeleteEvent([FromBody] int id)
        {
            var result = await EventMan.DeleteEvent(id);
            if (result == false)
            {
                return BadRequest("Data not available");
            }
            return Ok(result);
        }
    }
}
