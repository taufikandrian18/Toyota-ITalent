using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EvaluationTypes
    {
        public EvaluationTypes()
        {
            CompetencyEvaluationMappings = new HashSet<CompetencyEvaluationMappings>();
            Tasks = new HashSet<Tasks>();
        }

        [Key]
        public int EvaluationTypeId { get; set; }
        [Required]
        [StringLength(64)]
        public string EvaluationTypeName { get; set; }

        [InverseProperty("EvaluationType")]
        public virtual ICollection<CompetencyEvaluationMappings> CompetencyEvaluationMappings { get; set; }
        [InverseProperty("EvaluationType")]
        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
