using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ReportTrainingScoreModel
    {
        public int TrainingId { get; set; }
        public string CourseName { get; set; }
        public string ProgramType { get; set; }
        public int Batch { get; set; }
        public int Participant { get; set; }
        public Decimal? ParticipantRate { get; set; }
        public string Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string UrlDetail { get; set; }
    }

    public class ReportTrainingScoreFilterModel : PageAbstract
    {
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? ProgramTypeId { get; set; }
        public int? Participant { get; set; }
        public string Status { get; set; }
        public string CourseName { get; set; }
        public int? Batch { get; set; }
        public Decimal? ParticipantRate { get; set; }
    }

    public class ReportTrainingScoreViewModel
    {
        public List<ReportTrainingScoreModel> ListReportTrainingScore { get; set; }
        public int TotalData { get; set; }
    }


}
