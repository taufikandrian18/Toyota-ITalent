using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TrainingModuleMappings
    {
        [Key]
        public int TrainingModuleMappingId { get; set; }
        public int TrainingId { get; set; }
        public int SetupModuleId { get; set; }
        public DateTime? TrainingStart { get; set; }
        public DateTime? TrainingEnd { get; set; }
        public int? TimePointId { get; set; }
        public int? CoachId { get; set; }

        [ForeignKey("CoachId")]
        [InverseProperty("TrainingModuleMappings")]
        public virtual Coaches Coach { get; set; }
        [ForeignKey("SetupModuleId")]
        [InverseProperty("TrainingModuleMappings")]
        public virtual SetupModules SetupModule { get; set; }
        [ForeignKey("TimePointId")]
        [InverseProperty("TrainingModuleMappings")]
        public virtual TimePoints TimePoint { get; set; }
        [ForeignKey("TrainingId")]
        [InverseProperty("TrainingModuleMappings")]
        public virtual Trainings Training { get; set; }
    }
}
