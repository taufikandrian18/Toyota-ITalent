using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class GetTrackingParameter
    {
        public string TrainingID { get; set; }
        public string DealerID { get; set; }
        public string OutletID { get; set; }
        public string AreaID { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Positions { get; set; }
        public string Status { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }

    public class ReportTrackingCompetencyPoints
    {
        public string SetupModuleID { get; set; }
        public int TrainingTypeID { get; set; }
        public string ModuleName { get; set; }
        public string Point { get; set; }
    }

    public class GetKPIParameter
    {
        public string TrainingID { get; set; }
        public string DealerID { get; set; }
        public string AreaID { get; set; }
    }

    public class ReportKPITrackingPoints
    {
        public string ModuleName { get; set; }
        public string Point { get; set; }
        public string Percentage { get; set; }
    }

    public class ReportKPITrackingPositions
    {
        public string Position { get; set; }
        public string Results { get; set; }
    }

    public class ReportKPITrackingPositionsResults
    {
        public string ModuleName { get; set; }
        public string ModuleType { get; set; }
        public string IsPercent { get; set; }
        public int TrainingTypeID { get; set; }
        public string ModuleValue { get; set; }
    }
    
}
