using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class PositionCompentencyMappings
    {
        [Key]
        public int PositionCompentencyMappingId { get; set; }
        public int PositionId { get; set; }
        public int CompetencyId { get; set; }
        [Required]
        [StringLength(64)]
        public string Priority { get; set; }
        public int ProficiencyLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }

        [ForeignKey("CompetencyId")]
        [InverseProperty("PositionCompentencyMappings")]
        public virtual Competencies Competency { get; set; }
        [ForeignKey("PositionId")]
        [InverseProperty("PositionCompentencyMappings")]
        public virtual Positions Position { get; set; }
    }
}
