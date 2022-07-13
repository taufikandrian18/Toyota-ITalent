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
    [Route("api/v1/point-type")]
    public class PointTypeApiController : Controller
    {
        private readonly PointTypeService _PointTypeMan;


        public PointTypeApiController(PointTypeService pointTypeService)
        {
            this._PointTypeMan = pointTypeService;
        }
        // GET: api/<controller>
        [HttpGet("get-all-point-type-async")]
        public async Task<ActionResult<List<PointTypeViewModel>>> GetAllPointTypeAsync()
        {
            var data = await this._PointTypeMan.GetAllPointType();
            return Ok(data);
        }

        [HttpGet("get-point-type-by-id")]
        public async Task<ActionResult<string>> GetPointTypeById(int id)
        {
            var data = await this._PointTypeMan.GetPointTypeById(id);
            return Ok(data);
        }
    }
}
