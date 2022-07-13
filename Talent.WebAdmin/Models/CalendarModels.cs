using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class CalendarScheduleCourseModel
    {
        public string Title { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string ClassName { get; set; }
    }
}
