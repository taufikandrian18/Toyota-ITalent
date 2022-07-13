using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class PersonalMappingModel
    {
        public string PrefixCode { get; set; }
        public string CompetencyName { get; set; }
        public string CompetencyDescription { get; set; }
        public int CompetencyTypeId { get; set; }
        public string CompetencyTypeName { get; set; }
        public int ProficiencyLevel { get; set; }
        public string KeyActionCode { get; set; }
        public string KeyActionName { get; set; }
        public string KeyActionDescription { get; set; }
        public string EvaluationTypeName { get; set; }
        public int EvaluationTypeId { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
    }
}
