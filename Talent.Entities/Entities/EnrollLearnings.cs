using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EnrollLearnings
    {
        public EnrollLearnings()
        {
            CalculateLearningQueue = new HashSet<CalculateLearningQueue>();
            EnrollLearningTimes = new HashSet<EnrollLearningTimes>();
        }

        public int EnrollLearningId { get; set; }
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int? TrainingId { get; set; }
        public bool IsQueued { get; set; }
        public bool IsEnrolled { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RemedialLevel { get; set; }
        public bool? IsPassed { get; set; }
        public bool IsRejected { get; set; }
        public bool IsJoined { get; set; }
        public int? SetupModuleId { get; set; }
        public bool IsDrafted { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("EnrollLearnings")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("SetupModuleId")]
        [InverseProperty("EnrollLearnings")]
        public virtual SetupModules SetupModule { get; set; }
        [ForeignKey("TrainingId")]
        [InverseProperty("EnrollLearnings")]
        public virtual Trainings Training { get; set; }
        [InverseProperty("EnrollLearning")]
        public virtual ICollection<CalculateLearningQueue> CalculateLearningQueue { get; set; }
        [InverseProperty("EnrollLearning")]
        public virtual ICollection<EnrollLearningTimes> EnrollLearningTimes { get; set; }
    }
}
