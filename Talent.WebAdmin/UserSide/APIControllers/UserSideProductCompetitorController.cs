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
    [Route("api/v1/userside-product-competitor")]
    public class UserSideProductCompetitorController : Controller
    {
        private readonly UserSideProductComparisonService ProductCompMan;
        private readonly UserSideAuthService UserSideMan;
        private readonly UserActivityService UserActivityMan;


        public UserSideProductCompetitorController(UserSideProductComparisonService service, UserSideAuthService userSide, UserActivityService userActivityLogService)
        {
            this.ProductCompMan = service;
            this.UserSideMan = userSide;
            this.UserActivityMan = userActivityLogService;
        }
        [HttpGet("get-base-product-comparison")]
        public async Task<ActionResult<UserSideBaseProductComparisonModel>> GetBaseProductComparisonAsync(Guid productId, Guid productTypeId)
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

            var data = await this.ProductCompMan.GetUserSideBaseProduct(productId, productTypeId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Spec & Comparison";
            log.Content = "Comparison " + productId + "&" + productTypeId;

            await this.UserActivityMan.InsertUserActivityLog(log);


            return Ok(data);
        }
        [HttpGet("get-competitor-product-comparison")]
        public async Task<ActionResult<UserSideProductComparisonModel>> GetCompetitorProductComparisonAsync(Guid productId, Guid productTypeId)
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

            var data = await this.ProductCompMan.GetUserSideProductComparison(productId, productTypeId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Spec & Comparison";
            log.Content = "Comparison " + productId + "&" + productTypeId;

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }
        [HttpGet("get-all-base-product-change-type")]
        public async Task<ActionResult<List<UserSideBaseProductComparisonModel>>> GetAllUserSideBaseProductChangeTypeAsync(int pageSize, int pageIndex, Guid productId)
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

            var data = await this.ProductCompMan.GetAllUserSideBaseChangeType(pageSize, pageIndex, productId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Spec & Comparison";
            log.Content = "Comparison " + productId;

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }
        [HttpGet("get-all-competitor-same-brand-product-change-type")]
        public async Task<ActionResult<List<UserSideBaseProductComparisonModel>>> GetAllUserSideCompetitorSameBrandProductChangeTypeAsync(int pageSize, int pageIndex, Guid productTypeId)
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

            var data = await this.ProductCompMan.GetAllUserSideCompetitorSameBrandProducts(pageSize, pageIndex, productTypeId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Spec & Comparison";
            log.Content = "Comparison Product Type Id = " + productTypeId;

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }
        [HttpGet("get-all-competitor-product-change-type")]
        public async Task<ActionResult<List<UserSideProductComparisonModel>>> GetAllUserSideCompetitorProductChangeTypeAsync(int pageSize, int pageIndex)
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

            var data = await this.ProductCompMan.GetAllUserSideCompetitorBrandProducts(pageSize, pageIndex);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Spec & Comparison";
            log.Content = "Comparison all competitor without product ID";

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }
        [HttpGet("get-all-feature-detail-comparison")]
        public async Task<ActionResult<List<UserSideProductFeatureNameModel>>> GetAllUserSideFeatureProductMappingAsync(Guid productId, Guid productTypeId, Guid productCompetitorId, Guid productCompetitorTypeId)
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

            var data = await this.ProductCompMan.GetAllUserSideFeatureMapping(productId, productTypeId,productCompetitorId,productCompetitorTypeId);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Spec & Comparison";
            log.Content = "Comparison " + productId;

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }
        [HttpPost("contribute-photo-feature")]
        public async Task<ActionResult> ContributeNewPhotoFeatureAysnc([FromBody] UserSideProductFeatureComparisonContributeModel model)
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

            await this.ProductCompMan.ContributeUpdatePhotoFeatureAsync(model, response.EmployeeId);

            return Ok();
        }
    }
}
