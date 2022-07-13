using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class AssignedLearnings
    {
        [Key]
        public int AssignedLearningId { get; set; }
        public int? TrainingId { get; set; }
        public int? SetupModuleId { get; set; }
        [Required]
        [StringLength(64)]
        public string AssignedTo { get; set; }
        [Required]
        [StringLength(64)]
        public string AssignedBy { get; set; }
        public DateTime AssignedAt { get; set; }

        [ForeignKey("AssignedBy")]
        [InverseProperty("AssignedLearningsAssignedByNavigation")]
        public virtual Employees AssignedByNavigation { get; set; }
        [ForeignKey("AssignedTo")]
        [InverseProperty("AssignedLearningsAssignedToNavigation")]
        public virtual Employees AssignedToNavigation { get; set; }
        [ForeignKey("SetupModuleId")]
        [InverseProperty("AssignedLearnings")]
        public virtual SetupModules SetupModule { get; set; }
        [ForeignKey("TrainingId")]
        [InverseProperty("AssignedLearnings")]
        public virtual Trainings Training { get; set; }
    }
}
