using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class ReportNPSModel
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
    }
}
