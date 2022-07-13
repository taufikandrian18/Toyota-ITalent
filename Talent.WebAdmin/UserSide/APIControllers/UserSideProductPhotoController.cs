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
    [Route("api/v1/userside-product-photo")]
    public class UserSideProductPhotoController : Controller
    {
        private readonly UserSideProductPhotoService ProductPhotoMan;
        private readonly UserSideAuthService UserSideMan;

        public UserSideProductPhotoController(UserSideProductPhotoService service, UserSideAuthService userSide)
        {
            this.ProductPhotoMan = service;
            this.UserSideMan = userSide;
        }
        [HttpGet("get-all-product-photo-by-product-id")]
        public async Task<ActionResult<List<UserSideProductPhotoModel>>> GetAllProductPhotoListAsync([FromQuery] Guid ProductId, bool is360Photos)
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

            var data = await this.ProductPhotoMan.GetAllUserSideProductPhoto(ProductId, is360Photos);

            return data;
        }
    }
}
