using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-download")]
    public class UserSideUserDownloadController : Controller
    {
        private readonly UserSideUserDownloadService Services;
        private readonly UserSideAuthService AuthService;


        public UserSideUserDownloadController(UserSideUserDownloadService service, UserSideAuthService auth)
        {
            this.Services = service;
            this.AuthService = auth;
        }


        [HttpPost("create")]
        public async Task<ActionResult<BaseResponse>> Create([FromBody] VMEmployeeDownload model)
        {
            var data = await this.Services.Create(model);
            return data;
        }

        [HttpGet("get")]
        public async Task<ActionResult<BaseResponse>> GetData(VMEmployeeDownloadParameter model)
        {

            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            model.EmployeeID = response.EmployeeId;

            var data = await this.Services.GetData(model);
            return data;
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<BaseResponse>> Delete(string  model)
        {
            var data = await this.Services.Delete(model);
            return data;
        }


    }
}
