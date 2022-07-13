using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class TrainingScoreReportViewQueryModel
    {
        public int TrainingId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int ProgramTypeId { get; set; }
        public string ProgramTypeName { get; set; }
        public int Batch { get; set; }
        public int Participant { get; set; }
        public Decimal? ParticipantRate { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
