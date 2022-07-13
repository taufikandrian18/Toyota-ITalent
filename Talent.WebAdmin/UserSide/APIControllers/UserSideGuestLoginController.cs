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
    [Route("api/v1/userside-guest-auth")]
    public class UserSideGuestLoginController : Controller
    {
        private readonly UserSideGuestLoginService LoginMan;
        private readonly UserSideAuthService AuthService;
        private readonly InboxService InboxMan;


        public UserSideGuestLoginController(UserSideGuestLoginService service, UserSideAuthService auth, InboxService inbox)
        {
            this.LoginMan = service;
            this.AuthService = auth;
            this.InboxMan = inbox;
        }


        [HttpPost("login")]
        public async Task<ActionResult<BaseResponse>> Login([FromBody] GuestLoginModel model)
        {
            var data = await this.LoginMan.Login(model);
            return data;
        }



        [HttpPost("logout")]
        public async Task<ActionResult<BaseResponse>> Logout([FromHeader] string authorization)
        {


            if (String.IsNullOrWhiteSpace(authorization) == true)
            {
                var message = new Message();

                message.En = "Token not found!";
                message.Id = "Token tidak ditemukan";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var responseToken = this.AuthService.VerifyMobileToken(token);


            var data = await this.LoginMan.Logout(responseToken.EmployeeId);
            return StatusCode(200, BaseResponse.ResponseOk());
        }


        [HttpPost("register")]
        public async Task<ActionResult<BaseResponse>> Register([FromBody] GuestRegisterModel model)
        {

            var data = await this.LoginMan.Register(model);
            return data;
        }


        [HttpPost("change-password")]
        public async Task<ActionResult<BaseResponse>> ChangePassword([FromBody] GuestChangePasswordModel model)
        {
            var data = await this.LoginMan.UpdatePassword(model);
            return data;
        }

        [HttpPost("guest-forget-password")]
        public async Task<ActionResult<BaseResponse>> GuestForgotPassword(string email)
        {
            var data = await this.LoginMan.GuestForgetPasssword(email);
            return data;
        }


        [HttpPost("request-forget-password")]
        public async Task<ActionResult<BaseResponse>> RequestGuestForgotPassword(string email)
        {
            var data = await this.LoginMan.RequestForgetPassword(email);
            return data;
        }


        [HttpPost("user-forget-password")]
        public async Task<ActionResult<BaseResponse>> UserForgotPassword([FromBody] UserForgetPassword model)
        {
            var data = await this.LoginMan.UserForgetPasssword(model);
            return data;
        }


        [HttpPost("user-contact-us")]
        public async Task<ActionResult<BaseResponse>> UserContactUs([FromBody] UserForgetPassword model)
        {
            var data = await this.LoginMan.ContactUs(model);
            return data;
        }

        [HttpPost("user-upgrade-account")]
        public async Task<ActionResult<BaseResponse>> UserUpgradeAccount([FromBody] GuestUpgradeAccountModel model)
        {
            var data = await this.LoginMan.UserUpgradeAccount(model);
            return data;
        }

        [HttpGet("user-guest-inbox")]
        public async Task<ActionResult<BaseResponse>> GetInboxGuestAccount([FromQuery] InboxFilterModel filter, [FromHeader] string authorization)
        {
            if (String.IsNullOrWhiteSpace(authorization) == true)
            {
                var message = new Message();

                message.En = "Token not found!";
                message.Id = "Token tidak ditemukan";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var responseToken = this.AuthService.VerifyMobileToken(token);

            var data = await this.InboxMan.GetInbox(filter, responseToken.EmployeeId);
            return Ok(data);
        }

        [HttpGet("get-profile")]
        public async Task<ActionResult<BaseResponse>> GetProfile()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var message = new Message();

                message.En = "Token not found!";
                message.Id = "Token tidak ditemukan";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var responseToken = this.AuthService.VerifyMobileToken(token);

            if (responseToken == null)
            {
                var message = new Message();

                message.En = "Invalid Token!";
                message.Id = "Token salah";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            var data = await this.LoginMan.GetProfile(responseToken.Email);
            return data;
        }

        [HttpGet("get-announcement")]
        public async Task<ActionResult<BaseResponse>> GetAnnouncement()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var message = new Message();

                message.En = "Token not found!";
                message.Id = "Token tidak ditemukan";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var responseToken = this.AuthService.VerifyMobileToken(token);

            if (responseToken == null)
            {
                var message = new Message();

                message.En = "Invalid Token!";
                message.Id = "Token salah";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            var data = await this.LoginMan.GetAnnouncement(responseToken.EmployeeId);
            return data;
        }

        [HttpGet("user-guest-check-validation")]
        public async Task<ActionResult<BaseResponse>> CheckValidation([FromQuery] InboxFilterModel filter, [FromHeader] string authorization)
        {
            if (String.IsNullOrWhiteSpace(authorization) == true)
            {
                var message = new Message();

                message.En = "Token not found!";
                message.Id = "Token tidak ditemukan";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var responseToken = this.AuthService.VerifyMobileToken(token);

            var data = await this.LoginMan.CheckValidation(responseToken.EmployeeId);
            return data;
        }

    }
}
