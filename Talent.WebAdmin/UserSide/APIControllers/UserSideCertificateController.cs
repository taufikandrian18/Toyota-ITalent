using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    /// <summary>
    /// Certificate Web API controller for providing Web APIs such as retrieve certificates data.
    /// </summary>
    [Route("api/v1/userside-certificate")]
    public class UserSideCertificateController : ControllerBase
    {
        private readonly UserSideCertificateService CertificateMan;
        private readonly UserSideAuthService AuthService;

        public UserSideCertificateController(UserSideCertificateService service, UserSideAuthService authService)
        {
            this.CertificateMan = service;
            this.AuthService = authService;
        }

        /// <summary>
        /// Get all certificate data by employee id.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>List of certificate data in <seealso cref="List{UserSideCertificateViewModel}"/> format.</returns>
        [HttpGet("get-certificate-by-id")]
        public async Task<ActionResult<UserSideCertificateModel>> GetCertificateByIdAsync([FromQuery] string employeeId)
        {
            var data = await this.CertificateMan.GetCertificateByEmployeeId(employeeId);

            return Ok(data);
        }

        /// <summary>
        /// Get all certificate data by token.
        /// </summary>
        /// <returns>List of certificate data in <seealso cref="List{UserSideCertificateViewModel}"/> format.</returns>
        [HttpGet("get-certificate-by-token")]
        public async Task<ActionResult<UserSideCertificateModel>> GetCertificateByTokenAsync()
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

            var data = await this.CertificateMan.GetCertificateByEmployeeId(response.EmployeeId);

            return Ok(data);
        }

        /// <summary>
        /// Get single certificate data by jwt session token and certificate id
        /// </summary>
        /// <param name="certificateId"></param>
        /// <returns>Single </returns>
        [HttpGet("get-single-certificate/{certificateId}")]
        public async Task<ActionResult<string>> GetSingleCertificate(int certificateId)
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

            var data = await this.CertificateMan.GetSingleCertificate(response.EmployeeId, certificateId);
            if (data == null) return BadRequest("Certificate not found");
            return Ok(data);
        }
    }
}