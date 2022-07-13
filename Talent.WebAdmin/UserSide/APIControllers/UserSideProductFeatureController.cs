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
    [Route("api/v1/userside-product-feature")]
    public class UserSideProductFeatureController : Controller
    {
        private readonly UserSideProductFeatureService ProductFeatureMan;
        private readonly UserSideAuthService UserSideMan;
        private readonly UserActivityService UserActivityMan;

        public UserSideProductFeatureController(UserSideProductFeatureService service, UserSideAuthService userSide, UserActivityService userActivityLogService)
        {
            this.ProductFeatureMan = service;
            this.UserSideMan = userSide;
            this.UserActivityMan = userActivityLogService;
        }
        [HttpGet("get-base-product-comparison")]
        public async Task<ActionResult<UserSideProductFeatureViewModel>> GetProductFeatureFilterAsync(Guid productFeatureCategoryId, Guid productId, Guid productTypeId)
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

            UserSideProductFeatureViewModel data = new UserSideProductFeatureViewModel();

            data.FeatureCategoryList = await this.ProductFeatureMan.GetUserSideFeatureCategoryFilterList();
            data.FeatureList = await this.ProductFeatureMan.GetUserSideFeatureFilterList(productFeatureCategoryId, productId, productTypeId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Spec & Comparison";
            log.Content = "Feature";

            await this.UserActivityMan.InsertUserActivityLog(log);


            return Ok(data);
        }
        [HttpGet("get-product-feature-mapping-detail")]
        public async Task<ActionResult<UserSideProductFeatureListModel>> GetProductFeatureDetailAsync(Guid productFeatureCategoryId, Guid productId, Guid productTypeId, Guid productFeatureId)
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

            var data = await this.ProductFeatureMan.GetUserSideFeatureMappingDetail(productFeatureCategoryId, productId, productTypeId, productFeatureId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Spec & Comparison";
            log.Content = "Feature";

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }
    }
}
