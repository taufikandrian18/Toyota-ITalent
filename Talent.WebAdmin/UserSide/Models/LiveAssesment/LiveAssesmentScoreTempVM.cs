using System;
namespace Talent.WebAdmin.UserSide.Models.LiveAssesment
{
    public class LiveAssesmentScoreTempVM
    {
        public string SkillcheckGUID { get; set; }
        public double AvgScore { get; set; }
        public double HighestScore { get; set; }
        public double LatestScore { get; set; }
    }
    public class LiveAssesmentScoreTempEmpVM
    {
        public string EmployeeGUID { get; set; }
        public string SkillcheckGUID { get; set; }
        public string AssesmentId { get; set; }
        public double Score { get; set; }
    }
}
