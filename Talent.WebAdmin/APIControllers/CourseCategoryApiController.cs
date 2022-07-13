using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/course-category")]
    public class CourseCategoryApiController : Controller
    {
        private readonly CourseCategoryService CourseCategoryMan;

        public CourseCategoryApiController(CourseCategoryService courseCategoryService)
        {
            this.CourseCategoryMan = courseCategoryService;
        }

        [HttpGet("get-all-course-categories", Name = "get-all-course-categories")]
        public async Task<ActionResult<CourseCategoryViewModel>> GetAllCourseCategories()
        {
            var result = await this.CourseCategoryMan.GetAllCourseCategories();
            return Ok(result);
        }

        [HttpGet("get-course-category-by-id/{id}", Name = "get-course-category-by-id")]
        public async Task<ActionResult<CourseCategoryModel>> GetCourseCategoryById(int id)
        {
            var result = await CourseCategoryMan.GetCourseCategoryById(id);
            return Ok(result);
        }
    }
}
