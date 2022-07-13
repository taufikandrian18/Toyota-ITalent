using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class SurveyReportViewModel
    {
        public List<SurveyReportModel> SurveyReportList { get; set; }
        public int TotalData { get; set; }
    }

    public class SurveyReportModel
    {
        public int SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public int Respondent{ get; set; }
        public decimal RespondentRate { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UrlDetail { get; set; }
    }

    public class SurveyReportFilter : PageAbstract
    {
        public string SurveyTitle { get; set; }
        public int? Respondent { get; set; }
        public decimal? RespondentRate { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
