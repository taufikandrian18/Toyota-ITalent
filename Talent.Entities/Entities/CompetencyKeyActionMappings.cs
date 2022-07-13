using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class CompetencyKeyActionMappings
    {
        public int CompetencyId { get; set; }
        public int KeyActionId { get; set; }

        [ForeignKey("CompetencyId")]
        [InverseProperty("CompetencyKeyActionMappings")]
        public virtual Competencies Competency { get; set; }
        [ForeignKey("KeyActionId")]
        [InverseProperty("CompetencyKeyActionMappings")]
        public virtual KeyActions KeyAction { get; set; }
    }
}
