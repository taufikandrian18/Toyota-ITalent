using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Competencies
    {
        public Competencies()
        {
            CompetencyEvaluationMappings = new HashSet<CompetencyEvaluationMappings>();
            CompetencyKeyActionMappings = new HashSet<CompetencyKeyActionMappings>();
            PositionCompentencyMappings = new HashSet<PositionCompentencyMappings>();
            SetupTasks = new HashSet<SetupTasks>();
            Tasks = new HashSet<Tasks>();
        }

        [Key]
        public int CompetencyId { get; set; }
        public int CompetencyTypeId { get; set; }
        [Required]
        [StringLength(16)]
        public string PrefixCode { get; set; }
        [Required]
        [StringLength(255)]
        public string CompetencyName { get; set; }
        public string CompetencyDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("CompetencyTypeId")]
        [InverseProperty("Competencies")]
        public virtual CompetencyTypes CompetencyType { get; set; }
        [InverseProperty("Competency")]
        public virtual ICollection<CompetencyEvaluationMappings> CompetencyEvaluationMappings { get; set; }
        [InverseProperty("Competency")]
        public virtual ICollection<CompetencyKeyActionMappings> CompetencyKeyActionMappings { get; set; }
        [InverseProperty("Competency")]
        public virtual ICollection<PositionCompentencyMappings> PositionCompentencyMappings { get; set; }
        [InverseProperty("Competency")]
        public virtual ICollection<SetupTasks> SetupTasks { get; set; }
        [InverseProperty("Competency")]
        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
