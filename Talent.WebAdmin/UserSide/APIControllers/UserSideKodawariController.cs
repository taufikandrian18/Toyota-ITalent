using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-kodawaris")]
    public class UserSideKodawariController : Controller
    {
        private readonly UserSideKodawariService KodawariMan;
        private readonly UserSideAuthService UserSideMan;

        public UserSideKodawariController(UserSideKodawariService service, UserSideAuthService userSide)
        {
            this.KodawariMan = service;
            this.UserSideMan = userSide;
        }
        [HttpGet("get-all-kodawari-banner")]
        public async Task<ActionResult<List<UserSideKodawariBannerModel>>> GetUserSideAllKodawariBannerAsync()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.KodawariMan.GetAllUserSideKodawariBanner();

            return Ok(data);
        }
        [HttpGet("get-all-kodawari-filtered")]
        public async Task<ActionResult<List<UserSideKodawariPaginateModel>>> GetUserSideAllKodawariFilteredAsync([FromQuery] Guid cms_MenuId, Guid kodawariMenuAwalId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.KodawariMan.GetKodawariPaginate(cms_MenuId, kodawariMenuAwalId);

            return Ok(data);
        }
        [HttpGet("get-kodawari-detail")]
        public async Task<ActionResult<UserSideKodawariModel>> GetUserSideKodawariDetailAsync(Guid kodawariId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.KodawariMan.GetUserSideKodawariDetail(kodawariId);

            return Ok(data);
        }
        [HttpGet("get-all-kodawari-menu-awal")]
        public async Task<ActionResult<List<UserSideKodawariMenuAwalModel>>> GetUserSideAllKodawariMenuAwalAsync()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.KodawariMan.GetKodawariMenuAwalPaginate();

            return Ok(data);
        }
        [HttpGet("get-all-kodawari-menu-category")]
        public async Task<ActionResult<UserSideKodawariCategoryMenuPaginateModel>> GetUserSideAllKodawariMenuCategoryAsync([FromQuery] Guid kodawariMenuId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.KodawariMan.GetKodawariMenuCategoryPaginate(kodawariMenuId);

            return Ok(data);
        }
        [HttpGet("get-all-kodawari-download-detail")]
        public async Task<ActionResult<UserSideKodawariDownloadModel>> GetUserSideKodawariDownloadDetailAsync([FromQuery] Guid kodawariMenuId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.KodawariMan.GetKodawariDownloadDetail(kodawariMenuId);

            return Ok(data);
        }
    }
}
