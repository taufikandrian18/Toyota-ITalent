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
    [Route("api/v1/hobby")]
    public class HobbyApiController : Controller
    {
        private readonly HobbyService hobbyMan;

        public HobbyApiController(HobbyService service)
        {
            this.hobbyMan = service;
        }

        [HttpGet("get-all-hobbies")]
        public async Task<ActionResult<HobbyGridModel>> GetAllHobbyAsync([FromQuery] HobbyFilterModel filter)
        {
            var data = await this.hobbyMan.GetAllHobbyAsync(filter);

            return Ok(data);
        }

        [HttpPost("insert-hobby")]
        public async Task<ActionResult<bool>> InsertHobbyAsync([FromBody] HobbyCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(false);
            }

            var isSuccess = await this.hobbyMan.InsertHobby(model);

            if (!isSuccess)
            {
                return Ok(false);
            }

            return Ok(true);
        }

        [HttpGet("get-hobby-by-id")]
        public async Task<ActionResult<HobbyViewModel>> GetHobbyByIdAsync([FromQuery] int hobbyId)
        {
            var data = await this.hobbyMan.GetHobbyByIdAsync(hobbyId);

            return Ok(data);
        }

        [HttpPost("update-hobby")]
        public async Task<ActionResult<bool>> UpdateHobbyByIdAsync([FromBody] HobbyUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(false);
            }

            var success = await this.hobbyMan.UpdateHobbyAsync(model);

            if (!success)
            {
                return Ok(false);
            }

            return Ok(true);
        }

        [HttpDelete("delete-hobby")]
        public async Task<ActionResult> DeleteHobbyByIdAsync([FromQuery] int hobbyId)
        {
            await this.hobbyMan.DeleteHobbyAsync(hobbyId);

            return Ok();
        }
    }
}
