using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

using TAM.Talent.Commons.Core.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/announcement")]
    public class UserSideAnnouncementController : Controller
    {
        private readonly UserSideAnnouncementService AnnouncementServices;


        public UserSideAnnouncementController(UserSideAnnouncementService service)
        {
           AnnouncementServices = service;
        }

      
        [HttpPost("data")]
        public async Task<ActionResult<BaseResponse>> InsertData([FromBody]Announcements model)
        {
            var data = await AnnouncementServices.Insert(model);
            return data;
        }


        [HttpDelete("data")]
        public async Task<ActionResult<BaseResponse>> DeleteData([FromQuery]string id)
        {
            var data = await AnnouncementServices.Delete(id);
            return data;
        }


        [HttpPut("data")]
        public async Task<ActionResult<BaseResponse>> UpdateData([FromBody]Announcements model)
        {
            var data = await AnnouncementServices.Update(model);
            return data;
        }



        [HttpGet("data")]
        public async Task<ActionResult<BaseResponse>> GetData(string id, bool? status)
        {
            var data = await AnnouncementServices.Get(id,status);
            return data;
        }
    }
}
