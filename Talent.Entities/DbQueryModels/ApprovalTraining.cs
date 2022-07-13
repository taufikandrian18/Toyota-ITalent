using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class ApprovalTraining
    {
        public int TrainingId { set; get; }
        public string CourseName { set; get; }
        public string ProgramType { set; get; }
        public int Batch { set; get; }
        public int? Enrolment { set; get; }
        public int? Quota { set; get; }
        public DateTime? TrainingStartDate { set; get; }
        public DateTime? TrainingEndDate { set; get; }
        public DateTime? TrainingUpdatedAt { set; get; }
    }
}
