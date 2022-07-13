using System;
using System.Collections.Generic;
using System.Text;

namespace TAM.Talent.Excels.Lib.Models
{
    public class TrainingScoreReportExcellModel
    {
        public DateTime Timestamp { get; set; }
        public string CourseName { get; set; }
        public int TotalScore { get; set; }
        public int TotalScoreModule { get; set; }
        public float Attempts { get; set; }
        public string ModuleName { get; set; }
        public string CompetencyCode { get; set; }
        public string TypeofQuestion { get; set; }
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
