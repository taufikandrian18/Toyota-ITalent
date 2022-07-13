using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Services;
using static Talent.WebAdmin.UserSide.Models.UserSideProductGalleryModel;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-product-gallery")]
    public class UserSideProductGalleryController : Controller
    {
        private readonly UserSideProductGalleryService ProductGalleryMan;
        private readonly UserSideAuthService UserSideMan;
        private readonly UserActivityService UserActivityMan;

        public UserSideProductGalleryController(UserSideProductGalleryService service, UserSideAuthService userSide, UserActivityService userActivityLogService)
        {
            this.ProductGalleryMan = service;
            this.UserSideMan = userSide;
            this.UserActivityMan = userActivityLogService;
        }
        [HttpGet("get-all-product-galleries-filtered/{productGalleryColorCode}")]
        public async Task<ActionResult<UserSideProductGalleryPaginate>> GetAllProductGalleryFilteredAsync(int pageSize, int pageIndex, string productGalleryColorCode, string productTypeName, Guid productId)
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

            var data = await this.ProductGalleryMan.GetAllUserSideProductGalleryFiltered(pageSize, pageIndex, productGalleryColorCode, productTypeName, productId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Spec & Comparison";
            log.Content = "Gallery";

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }
        [HttpGet("get-all-product-gallery-color-list")]
        public async Task<ActionResult<List<UserSideProductGalleryColorListView>>> GetAllProductGalleryCodeListAsync([FromQuery] Guid productId)
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

            var data = await this.ProductGalleryMan.GetAllUserSideProductGalleryColorCode(productId);

            return data;
        }
        [HttpPost("contribute-photo-gallery")]
        public async Task<ActionResult<BaseResponse>> ContributeNewPhotoGalleryAysnc([FromBody] UserSideProductGalleryContributeModel model)
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

            var getData = await this.ProductGalleryMan.ContributeNewPhotoGalleryAsync(model, response.EmployeeId);

            return getData;
        }
    }
}
