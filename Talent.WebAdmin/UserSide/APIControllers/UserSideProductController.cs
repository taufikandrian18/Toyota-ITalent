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
using static Talent.WebAdmin.UserSide.Models.UserSideProductModel;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-product")]
    public class UserSideProductController : Controller
    {
        private readonly UserSideProductService ProductMan;
        private readonly UserSideAuthService UserSideMan;
        private readonly UserActivityService UserActivityMan;


        public UserSideProductController(UserSideProductService service, UserSideAuthService userSide, UserActivityService userActivityLogService)
        {
            this.ProductMan = service;
            this.UserSideMan = userSide;
            this.UserActivityMan = userActivityLogService;
        }
        [HttpGet("get-all-products-filtered/{productCategory}")]
        public async Task<ActionResult<List<UserSideProductInformationModel>>> GetAllProductFilteredAsync(int pageSize, int pageIndex,string productCategory)
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

            var data = await this.ProductMan.GetAllUserSideProductFiltered(pageSize, pageIndex, productCategory);

            var log = new UserLogActivities();
            log.EmployeeId = response.EmployeeId;
            log.Type = "Spec & Comparison";
            log.Content = productCategory;

            await this.UserActivityMan.InsertUserActivityLog(log);

            return Ok(data);
        }
        [HttpGet("get-all-product-category-list")]
        public async Task<ActionResult<List<UserSideProductCatListModel>>> GetAllProductCategoriesAsync()
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

            var data = await this.ProductMan.GetAllUserSideProductCategoryList();
            return data;
        }
    }
}
