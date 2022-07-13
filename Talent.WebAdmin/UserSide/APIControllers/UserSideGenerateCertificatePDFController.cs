using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-generate-certificate-pdf")]
    public class UserSideGenerateCertificatePDFController : Controller
    {
        private readonly UserSideGenerateCertificatePDFService CertificateService;
        private readonly UserSideAuthService AuthService;

        public UserSideGenerateCertificatePDFController(UserSideGenerateCertificatePDFService certificateService, UserSideAuthService userSideAuthService)
        {
            this.CertificateService = certificateService;
            this.AuthService = userSideAuthService;
        }

        //[HttpPost("generate-certificate/{trainingId}")]
        //public async Task<ActionResult<bool>> GenerateCertificate(int trainingId)
        //{
        //    var response = await this.CertificateService.GenerateCertficateFromTrainingAsync(trainingId);
        //    return response;
        //}

        [HttpPost("generate-employee-certificate/{trainingId}")]
        public async Task<ActionResult<bool>> GenerateEmployeeCertificate(int trainingId)
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
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var result = await this.CertificateService.GenerateCertificateAfterTrainingAsync(trainingId, response.EmployeeId);

            return result;
        }
    }
}
