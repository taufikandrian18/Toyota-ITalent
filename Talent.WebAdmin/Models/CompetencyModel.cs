using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class CompetencyModel
    {
        public int CompetencyId { get; set; }
        public int CompetencyTypeId { get; set; }
        public string PrefixCode { get; set; }
        public string CompetencyName { get; set; }
        public string CompetencyDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CompetencyViewModel
    {
        public List<CompetencyModel> ListCompetencyModel { get; set; }
        public int TotalItem { get; set; }
    }

    public class CompetencyFormModel
    {
        public int? CompetencyId { get; set; }
        public int CompetencyTypeId { get; set; }
        [Required]
        [StringLength(16)]
        public string PrefixCode { get; set; }
        [Required]
        [StringLength(255)]
        public string CompetencyName { get; set; }
        [StringLength(1024)]
        public string CompetencyDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<CompetencyKeyActionMappingModel> CompetencyKeyActionMappings { get; set; }
    }

    public class CompetencyFilter : PageAbstract
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string CompetencyName { get; set; }
        public string PrefixCode { get; set; }
        public string KeyActionCode { get; set; }
    }

    public class CompetencyJoinModel
    {
        public int CompetencyId { get; set; }
        public int CompetencyTypeId { get; set; }
        public string CompetencyTypeName { get; set; }
        public string KeyActionCode { get; set; }
        public string PrefixCode { get; set; }
        public string CompetencyName { get; set; }
        public string CompetencyDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CompetencyKeyActionMappingJoinModel> CompetencyKeyActionMappings { get; set; }
    }

    public class CompetencyJoinViewModel
    {
        public List<CompetencyJoinModel> ListCompetencyJoinModel { get; set; }
        public int TotalItem { get; set; }
    }
}
