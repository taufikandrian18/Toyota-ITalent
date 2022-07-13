using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class SurveyReport
    {
        public int SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public int Respondent { get; set; }
        public decimal RespondentRate { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class SurveyReportDetailModel
    {
        public DateTime TimeStamp { get; set; }
        public int SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public string TypeOfQuestion { get; set; }
        public string Question { get; set; }
        public string BlobId { get; set; }
        public string Mime { get; set; }
        public int SurveyQuestionTypeId { get; set; }
        public string Answer { get; set; }
        public string EmployeeName { get; set; }
        public string Area { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Dealer { get; set; }
        public string Outlet { get; set; }
    }
}
