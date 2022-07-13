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
    [Route("api/v1/setup-time-point")]
    public class SetupTimePointApiController : Controller
    {
        private readonly SetupTimePointService _SetupTimePointMan;

        public SetupTimePointApiController(SetupTimePointService setupTimePointService)
        {
            this._SetupTimePointMan = setupTimePointService;
        }


        // GET: api/<controller>
        [HttpGet("get-all-setup-time-point-paginate-async")]
        public async Task<ActionResult<SetupTimePointPaginate>> GetAllSetupTimePointPaginateAsync(SetupTimePointGridFilter filter)
        {
            var data = await this._SetupTimePointMan.GetAllSetupTimePointPaginateAsync(filter);
            return Ok(data);
        }

        [HttpGet("get-setup-time-point-get-id-async")]
        public async Task<ActionResult<SetupTimePoint>> GetSetupTimePointGetIdAsync(int timePointId)
        {
            var data = await this._SetupTimePointMan.GetSetupTimePointGetIdAsync(timePointId);
            return Ok(data);
        }
        [HttpGet("get-setup-time-point-by-score-async")]
        public async Task<ActionResult<TimePointTaskModel>> GetSetupTimePointByScoreAsync(int score)
        {
            var data = await this._SetupTimePointMan.GetSetupTimePointByScoreAsync(score);
            return Ok(data);
        }

        [HttpPost("insert-time-point-async")]
        public async Task<ActionResult<bool>> InsertTimePointAsync([FromBody] SetupTimePointInsertModel insert)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            var data = await this._SetupTimePointMan.InsertTimePointAsync(insert);

            return Ok(data);
        }

        [HttpPost("update-time-point-async")]
        public async Task<ActionResult<bool>> UpdateTimePointAsync([FromQuery] int id, [FromBody] SetupTimePointInsertModel update)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            var data = await this._SetupTimePointMan.UpdateTimePointAsync(id, update);

            if(data == false)
            {
                return BadRequest();
            }

            return Ok(data);
        }

        [HttpDelete("delete-time-point-async")]
        public async Task<ActionResult<bool>> DeleteTimePointAsync([FromQuery] int id)
        {
            var data = await this._SetupTimePointMan.DeleteTimePointAsync(id);

            if(data == false)
            {
                return BadRequest();
            }

            return Ok(data);
        }
    }
}
