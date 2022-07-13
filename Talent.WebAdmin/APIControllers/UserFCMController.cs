using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/user-fcm")]
    public class UserFCMController : Controller
    {
        private readonly UserFCMService ServiceMan;

        public UserFCMController(UserFCMService service)
        {
            this.ServiceMan = service;
        }

     
        [HttpPost("create")]
        public async Task<ActionResult<BaseResponse>> Create([FromBody] VMUserFCM model)
        {
            var getData = await ServiceMan.Create(model);
            return getData;
        }

        [HttpGet("get")]
        public async Task<ActionResult<BaseResponse>> Get(string id)
        {
            var getData = await ServiceMan.Get(id);
            return getData;
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<BaseResponse>> Delete(string id)
        {
            var getData = await ServiceMan.Delete(id);
            return getData;
        }

        [HttpPut("update")]
        public async Task<ActionResult<BaseResponse>> Update([FromBody] UserFcmTokens model)
        {
            var getData = await ServiceMan.Update(model);
            return getData;
        }

    }
}
