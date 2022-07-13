using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/course")]
    public class CourseApiController : Controller
    {
        private readonly CourseService CourseMan;
        private readonly CourseJoinService CourseJoinMan;

        public CourseApiController(CourseService courseService, CourseJoinService courseJoinService)
        {
            this.CourseMan = courseService;
            this.CourseJoinMan = courseJoinService;
        }

        [HttpGet("get-all-join-courses",Name = "get-all-join-courses")]
        public async Task<ActionResult<CourseJoinViewModel>> GetAllJoinCourses([FromQuery] CourseFilter filter)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await this.CourseJoinMan.GetAllCourses(filter);
            return Ok(result);
        }

        [HttpGet("get-join-course-by-id/{id}", Name = "get-join-course-by-id")]
        public async Task<ActionResult<CourseJoinModel>> GetJoinCourseById(int id)
        {
            var result = await CourseJoinMan.GetCourseJoinById(id);
            return Ok(result);
        }

        [HttpPost("create", Name = "create-course")]
        public async Task<ActionResult<int>> CreateCourse([FromBody] CourseFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await CourseMan.CreateCourse(model);
            return Ok(result);
        }

        [HttpPost("update", Name = "update-course")]
        public async Task<ActionResult<bool>> UpdateCourse([FromBody] CourseFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await CourseMan.UpdateCourse(model);
            if (result == false)
            {
                return BadRequest("Data not available");
            }
            return Ok(result);
        }

        [HttpDelete("delete", Name = "delete-course")]
        public async Task<ActionResult<bool>> DeleteCourse([FromBody] int id)
        {
            var result = await CourseMan.DeleteCourse(id);
            if (result == false)
            {
                return BadRequest("Data not available");
            }
            return Ok(result);
        }
    }
}
