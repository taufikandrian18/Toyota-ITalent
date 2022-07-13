using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-useraccesstime")]
    public class UserSideUserAccessTimeController : Controller
    {
        private readonly UserSideUserAccessTimesService _AccessMan;
        private readonly UserSideAuthService _AuthMan;

        public UserSideUserAccessTimeController(UserSideUserAccessTimesService userSideUserAccessTimesService, UserSideAuthService userSideAuthService)
        {
            this._AccessMan = userSideUserAccessTimesService;
            this._AuthMan = userSideAuthService ;
        }
        
        [HttpPost("employee-start-access-time")]
        public async Task<ActionResult<string>> EmployeeStartAccessTime()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this._AuthMan.VerifyMobileToken(token);

            if (response == null)
            {
               return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }
            var employeeId = response.EmployeeId;

            var accesTimeId = await this._AccessMan.EmployeeStartDate(employeeId);
            return Ok(accesTimeId);
        }

        [HttpPost("employee-end-access-time")]
        public async Task<ActionResult<string>> EmployeeEndAccessTime(int employeeAccesTimeId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this._AuthMan.VerifyMobileToken(token);

            if (response == null)
            {
               return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }
            var employeeId = response.EmployeeId;

            var accesTimeId = await this._AccessMan.EmployeeEndDate(employeeAccesTimeId, employeeId);
            if(accesTimeId == null)
            {
                return BadRequest(accesTimeId);
            }
            return Ok(accesTimeId);

        }


    }
}
