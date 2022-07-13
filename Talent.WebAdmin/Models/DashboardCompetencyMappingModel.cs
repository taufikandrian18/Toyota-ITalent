using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class DashboardCompetencyMappingModel
    {
        public List<CompetencyMappingModel> HardCompetency { get; set; }
        public List<CompetencyMappingModel> SoftCompetency { get; set; }
    }

    public class PositionNameModel
    {
        public string PositionName { get; set; }
    }

    public class CompetencyMappingModel
    {
        public string PrefixCode { get; set; }
        public int ProficiencyLevel { get; set; }
        public double ScoreTotal { get; set; }
        public double MaxScoreTotal { get; set; }
    }
}
