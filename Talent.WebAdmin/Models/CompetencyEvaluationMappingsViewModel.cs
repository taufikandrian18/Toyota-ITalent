using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class CompetencyEvaluationMappingsViewModel
    {
        public int CompetencyId { get; set; }
        public int EvaluationTypeId { get; set; }
        public int BonusScoreLt50 { get; set; }
        public int BonusScoreMte50 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CompetencyMapping
    {
        public int CompetencyId { get; set; }
        public int EvaluationTypeId { get; set; }
        public string CompetencyMappingCode { get; set; }
        public string CompetencyName { get; set; }
        public string TypeofEvaluation { get; set; }
        public int BonusScoreLt50 { get; set; }
        public int BonusScoreMte50 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CompetencyMappingGridFilter : GridFilter
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string CompetencyName { get; set; }
        public string CompetencyMappingCode { get; set; }
        public string TypeofEvaluation { get; set; }
        
    }

    public class CompetencyMappingGetDataGridFilter
    {
        [Required]
        public int CompetencyId { get; set; }
        [Required]
        public int EvaluationTypeId { get; set; }

    }

    public class CompetencyMappingPaginate
    {
        public List<CompetencyMapping> Data { get; set; }
        public int TotalData { get; set; }
    }

    public class CompetencyMappingInsertModel
    {
        public int CompetencyId { get; set; }
        public int EvaluationTypeId { get; set; }
        public int BonusScoreLt50 { get; set; }
        public int BonusScoreMte50 { get; set; }
    }

    public class CompetencyMappingJoinModel
    {
        public string CompetencyNameMapping { get; set; }
        public int CompetencyId { get; set; }
        public int EvaluationTypeId { get; set; }
        public int? CompetencyTypeId { get; set; }
        public string PrefixCode { get; set; }
        public string CompetencyTypeName { get; set; }
        public string EvaluationTypeName { get; set; }

    }
}
