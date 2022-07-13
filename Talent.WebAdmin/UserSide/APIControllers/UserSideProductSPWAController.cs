using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-product-spwas")]
    public class UserSideProductSPWAController : Controller
    {
        private readonly UserSideProductSPWAService ProductSPWAMan;
        private readonly UserSideAuthService UserSideMan;
        private readonly UserActivityService UserActivityMan;

        public UserSideProductSPWAController(UserSideProductSPWAService service, UserSideAuthService userSide, UserActivityService userActivityLogService)
        {
            this.ProductSPWAMan = service;
            this.UserSideMan = userSide;
            this.UserActivityMan = userActivityLogService;
        }

        [HttpGet("get-all-product-spwa-filtered")]
        public async Task<ActionResult<List<UserSideProductSPWAPaginateModel>>> GetAllProductSPWAFilteredAsync([FromQuery] Guid productId)
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

            var data = await this.ProductSPWAMan.GetProductSPWAPaginate(productId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "SPWA & Test Drive";
            log.Content = "Product SPWA " + productId;

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }
        [HttpGet("get-all-product-spwa-content-list-view")]
        public async Task<ActionResult<UserSideProductSPWAContentViewModel>> GetAllProductSPWAContentListViewAsync([FromQuery] Guid productSPWAMenuId,Guid productSPWACategoryId,Guid productId)
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

            var data = await this.ProductSPWAMan.GetProductSPWAContentListView(productSPWAMenuId, productSPWACategoryId,productId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "SPWA & Test Drive";
            log.Content = "Product SPWA " + productId;

            return Ok(data);
        }
        [HttpGet("get-all-product-spwa-test-drive-content-list-view")]
        public async Task<ActionResult<UserSideProductSPWATestDriveContentViewModel>> GetAllProductSPWATestDriveContentListViewAsync([FromQuery] Guid productSPWAMenuId, Guid productSPWACategoryId, Guid productId)
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

            var data = await this.ProductSPWAMan.GetProductSPWATestDriveContentListView(productSPWAMenuId, productSPWACategoryId, productId);


            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "SPWA & Test Drive";
            log.Content = "Product SPWA " + productId;

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }
        [HttpGet("get-product-spwa-mapping-detail")]
        public async Task<ActionResult<UserSideProductSPWAModel>> GetProductSPWAMappingDetailAsync(Guid ProductSPWAMappingId, Guid productId)
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

            var data = await this.ProductSPWAMan.GetUserSideProductSPWADetail(ProductSPWAMappingId, productId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "SPWA & Test Drive";
            log.Content = "Product SPWA " + productId;

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }
    }
}
