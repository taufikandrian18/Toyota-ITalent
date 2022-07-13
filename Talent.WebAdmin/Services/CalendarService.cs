using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class CalendarService
    {
        private readonly TalentContext DB;

        public CalendarService(TalentContext context)
        {
            this.DB = context;
        }

        public async Task<List<CalendarScheduleCourseModel>> GetAllCourseScheduleAsync()
        {
            var data = await (from t in DB.Trainings
                              join c in DB.Courses on t.CourseId equals c.CourseId
                              where t.StartDate != null && t.EndDate != null && t.IsDeleted == false
                              select new CalendarScheduleCourseModel
                              {
                                  Title = c.CourseName,
                                  Start = t.StartDate.GetValueOrDefault(),
                                  End = t.EndDate.EndDayTime(),
                                  ClassName = CalendarEventClassEnum.Primary
                              }).ToListAsync();

            return data;
        }
    }
}
