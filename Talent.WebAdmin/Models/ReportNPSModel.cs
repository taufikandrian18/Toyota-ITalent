using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ReportNPSViewModel
    {
        public List<ReportNPSItemModel> ReportNPSItems { get; set; }
        public int TotalData { get; set; }
    }

    public class ReportNPSFilterModel : PageAbstract
    {
        public int PageSize { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CourseName { get; set; }
        public int? ProgramTypeId { get; set; }
        public string Status { get; set; }
        public int? Batch { get; set; }
    }
    public class ReportNPSItemModel
    {
        public int TrainingId { get; set; }
        public int ProgramTypeId { get; set; }
        public string ProgramTypeName { get; set; }
        public string CourseName { get; set; }
        public int CourseId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Batch { get; set; }
        public string Status { get; set; }
        public string ViewCourseUrl { get; set; }
        public string ViewCoachUrl { get; set; }

    }
}
