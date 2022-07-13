using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class CompetencyEvaluationMappings
    {
        public int CompetencyId { get; set; }
        public int EvaluationTypeId { get; set; }
        [Column("BonusScoreLT50")]
        public int BonusScoreLt50 { get; set; }
        [Column("BonusScoreMTE50")]
        public int BonusScoreMte50 { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }

        [ForeignKey("CompetencyId")]
        [InverseProperty("CompetencyEvaluationMappings")]
        public virtual Competencies Competency { get; set; }
        [ForeignKey("EvaluationTypeId")]
        [InverseProperty("CompetencyEvaluationMappings")]
        public virtual EvaluationTypes EvaluationType { get; set; }
    }
}
