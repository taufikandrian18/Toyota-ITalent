using System;
using System.Collections.Generic;
using System.Text;

namespace TAM.Talent.Excels.Lib.Models
{
    public class SurveyReportExcelModel
    {
        public DateTime TimeStamp { get; set; }
        public int SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public string TypeOfQuestion { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string EmployeeName { get; set; }
        public string Area { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Dealer { get; set; }
        public string Outlet { get; set; }
    }
}
