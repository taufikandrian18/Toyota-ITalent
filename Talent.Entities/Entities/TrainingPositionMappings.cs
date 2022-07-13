using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TrainingPositionMappings
    {
        public int TrainingId { get; set; }
        public int PositionId { get; set; }

        [ForeignKey("PositionId")]
        [InverseProperty("TrainingPositionMappings")]
        public virtual Positions Position { get; set; }
        [ForeignKey("TrainingId")]
        [InverseProperty("TrainingPositionMappings")]
        public virtual Trainings Training { get; set; }
    }
}
