using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-upload-certificate")]
    [ApiController]
    public class UserSideUploadCertificateController : ControllerBase
    {
        private readonly UserSideCertificateService UserSideCertificateService;
        private readonly UserSideAuthService AuthService;

        public UserSideUploadCertificateController(UserSideCertificateService userSideCertificateService, UserSideAuthService authService)
        {
            UserSideCertificateService = userSideCertificateService;
            this.AuthService = authService;
        }

        [HttpPost(Name = "UploadCertificate")]
        public async Task<IActionResult> Create ([FromForm] UserSideCertificateFormModel model, string employeeId)
        {
            await this.UserSideCertificateService.CreateUploadCertificate(model, employeeId);
            return Ok();
        }

        [HttpPost("UploadCertificateByToken", Name = "UploadCertificateByToken")]
        public async Task<IActionResult> CreateByToken([FromForm] UserSideCertificateFormModel model)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response =  this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            await this.UserSideCertificateService.CreateUploadCertificate(model, response.EmployeeId);
            return Ok();
        }
    }
}