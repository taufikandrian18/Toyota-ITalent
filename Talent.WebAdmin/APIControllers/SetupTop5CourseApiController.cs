using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/setup-top-5-course")]
    public class SetupTop5CourseApiController : Controller
    {
        private readonly SetupTop5CourseService SetupTop5CourseServiceMan;
        public SetupTop5CourseApiController(SetupTop5CourseService setupTop5CourseService)
        {
            this.SetupTop5CourseServiceMan = setupTop5CourseService;
        }

        /// <summary>
        /// Get All Setup Top 5 Course
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet("get-all-setup-top-5-course")]
        public async Task<ActionResult<SetupTop5CourseViewModel>> GetAllSetupTop5Course([FromQuery] SetupTop5CourseSearch search)
        {
            var result = await this.SetupTop5CourseServiceMan.GetAllSetupTop5CoursePaginate(search);

            return Ok(result);
        }

        /// <summary>
        /// Get dropdown data for add new course
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-course-list-dropdown")]
        public async Task<ActionResult<SetupTop5CourseDropdownList>> GetCourseListDropdown()
        {
            var result = await this.SetupTop5CourseServiceMan.GetCourseNameDropdown();

            return Ok(result);
        }

        /// <summary>
        /// Insert new Course to Top5Course list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("insert-into-top-5-course")]
        public async Task<ActionResult<bool>> InsertIntoTop5Course([FromBody]SetupTop5CourseDropdownData model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model State is not valid");
            }

            var result = await this.SetupTop5CourseServiceMan.InsertIntoTop5Course(model);

            return Ok(result);
        }

        /// <summary>
        /// Rearrange top5Course list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("rearrange-top-5-course")]
        public async Task<ActionResult<bool>> RearrangeTop5Course([FromBody]SetupTop5CourseViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model State is not valid");
            }

            var result = await this.SetupTop5CourseServiceMan.RearrangeTop5Course(model);

            return Ok(result);
        }
    }
}
