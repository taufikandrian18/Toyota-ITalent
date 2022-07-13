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
    [Route("api/v1/userside-product-programs")]
    public class UserSideProductProgramController : Controller
    {
        private readonly UserSideProductProgramService ProductProgramMan;
        private readonly UserSideAuthService UserSideMan;

        public UserSideProductProgramController(UserSideProductProgramService service, UserSideAuthService userSide)
        {
            this.ProductProgramMan = service;
            this.UserSideMan = userSide;
        }

        [HttpGet("get-all-product-program-filtered")]
        public async Task<ActionResult<List<UserSideProductProgramPaginateModel>>> GetAllProductProgramFilteredAsync([FromQuery] Guid productId)
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

            var data = await this.ProductProgramMan.GetProductProgramPaginate(productId);

            return Ok(data);
        }
        [HttpGet("get-product-program-mapping-detail")]
        public async Task<ActionResult<UserSideProductProgramModel>> GetProductProgramMappingDetailAsync(Guid ProductProgramMappingId, Guid productId)
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

            var data = await this.ProductProgramMan.GetUserSideProductProgramDetail(ProductProgramMappingId, productId);

            return Ok(data);
        }
    }
}
