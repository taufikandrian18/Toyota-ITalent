using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    /// <summary>
    /// User Side News Web API controller for providing Web APIs such as retrieve news data.
    /// </summary>
    [Route("api/v1/userside-news")]
    public class UserSideNewsController : Controller
    {
        private readonly UserSideNewsService Service;

        public UserSideNewsController(UserSideNewsService userSideNewsService)
        {
            this.Service = userSideNewsService;
        }

        /// <summary>
        /// Get all news categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet("news-categories")]
        public async Task<ActionResult<List<UserSideNewsCategoryModel>>> GetNewsCategories()
        {
            var data = await this.Service.GetNewsCategories();
            if (data == null)
            {
                data = new List<UserSideNewsCategoryModel>();
            }

            return Ok(data);
        }

        /// <summary>
        /// Get all popular news.
        /// </summary>
        /// <param name="itemPerPage"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("popular-news")]
        public async Task<ActionResult<List<UserSideNewsModel>>> GetPopularNews(int itemPerPage, int pageIndex)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            var data = await this.Service.GetPopularNews(itemPerPage, pageIndex);

            return Ok(data);
        }

        /// <summary>
        /// Get all news.
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        [HttpGet("news")]
        public async Task<ActionResult<List<UserSideNewsModel>>> GetNews([FromQuery] UserSideNewsFilterModel filterModel)
        {
            if (filterModel.PageIndex < 1)
            {
                filterModel.PageIndex = 1;
            }

            var data = await this.Service.GetNews(filterModel);

            return Ok(data);
        }

        /// <summary>
        /// Detail news.
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        [HttpGet("detail-news/{newsId}")]
        public async Task<ActionResult<List<UserSideNewsModel>>> GetDetailNews(int newsId, int itemPerPageRelatedNews, int pageIndexRelatedNews)
        {
            if (pageIndexRelatedNews < 1)
            {
                pageIndexRelatedNews = 1;
            }

            var data = await this.Service.GetDetailNews(newsId, itemPerPageRelatedNews, pageIndexRelatedNews);

            return Ok(data);
        }

        /// <summary>
        /// Get news by category.
        /// </summary>
        /// <param name="newsCategoryId"></param>
        /// <param name="itemPerPage"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("news-by-category/{newsCategoryId}")]
        public async Task<ActionResult<List<UserSideNewsModel>>> GetNewsByCategory(int newsCategoryId, int itemPerPage, int pageIndex)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            var data = await this.Service.GetNewsByCategory(newsCategoryId, itemPerPage, pageIndex);

            return Ok(data);
        }

        [HttpPost("add-total-view")]
        public async Task<ActionResult<bool>> AddTotalView([FromQuery] int newsId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model State is not valid!");
            }

            var result = await this.Service.AddTotalView(newsId);

            return Ok(result);
        }
    }
}