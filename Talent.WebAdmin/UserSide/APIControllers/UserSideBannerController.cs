using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    /// <summary>
    /// User Side Banner Web API controller for providing Web APIs such as retrieve banner data.
    /// </summary>
    [Route("api/v1/userside-banners")]
    public class UserSideBannerController : Controller
    {
        private readonly UserSideBannerService Service;

        public UserSideBannerController(UserSideBannerService userSideBannerService)
        {
            this.Service = userSideBannerService;
        }

        /// <summary>
        ///  Get all banner.
        /// </summary>
        /// <param name="itemPerPage"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<UserSideBannerModel>>> GetBanners(int itemPerPage, int pageIndex)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            var data = await this.Service.GetBanners(itemPerPage, pageIndex);
            if (data == null)
            {
                data = new List<UserSideBannerModel>();
            }

            return Ok(data);
        }
    }
}