using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/calendar")]
    public class CalendarApiController : Controller
    {
        private readonly CalendarService calMan;

        public CalendarApiController(CalendarService service)
        {
            this.calMan = service;
        }

        [HttpGet("get-all-course-schedule")]
        public async Task<ActionResult<List<CalendarScheduleCourseModel>>> GetAllCourseScheduleAsync()
        {
            var data = await this.calMan.GetAllCourseScheduleAsync();

            return data;
        }
    }
}
