using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class SetupModuleCalculate
    {
        public string CourseName { get; set; }
        public int SetupModuleId { get; set; }
        public int MinimumScore { get; set; }
        public int EmployeeScore { get; set; }
        public int EmployeePoint { get; set; }
        public bool IsForRemedial { get; set; } 


    }

    public class WeightedModel
    {
        public decimal Weighted { get; set; }
    }
}
