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
    [Route("api/v1/userside-key-piece-of-minds")]
    public class UserSideKeyPieceOfMindController : Controller
    {
        private readonly UserSideKeyPieceOfMindService KeyPieceOfMindMan;
        private readonly UserSideAuthService UserSideMan;

        public UserSideKeyPieceOfMindController(UserSideKeyPieceOfMindService service, UserSideAuthService userSide)
        {
            this.KeyPieceOfMindMan = service;
            this.UserSideMan = userSide;
        }

        [HttpGet("get-all-key-piece-of-mind-filtered")]
        public async Task<ActionResult<List<UserSideKeyPieceOfMindPaginateModel>>> GetAllKeyPieceOfMindFilteredAsync([FromQuery] Guid keyPieceOfMindMenuId)
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

            var data = await this.KeyPieceOfMindMan.GetKeyPieceOfMindPaginate(keyPieceOfMindMenuId);

            return Ok(data);
        }
        [HttpGet("get-key-piece-of-mind-detail")]
        public async Task<ActionResult<UserSideKeyPieceOfMindModel>> GetKeyPieceOfMindDetailAsync(Guid keyPieceOfMindId)
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

            var data = await this.KeyPieceOfMindMan.GetUserSideKeyPieceOfMindDetail(keyPieceOfMindId);

            return Ok(data);
        }
        [HttpGet("get-all-key-piece-of-mind-menu-awal")]
        public async Task<ActionResult<List<UserSideKeyPieceOfMindMenuAwalModel>>> GetAllKeyPieceOfMindMenuAwalAsync()
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

            var data = await this.KeyPieceOfMindMan.GetUserSideKeyPieceOfMindMenuAwalPaginate();

            return Ok(data);
        }
    }
}
