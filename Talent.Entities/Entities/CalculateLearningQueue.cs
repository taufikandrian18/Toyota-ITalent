using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class CalculateLearningQueue
    {
        public Guid CalculateLearningQueueId { get; set; }
        public int EnrollLearningId { get; set; }
        public DateTime FinishedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string EnrollType { get; set; }
        public int? SetupModuleId { get; set; }

        [ForeignKey("EnrollLearningId")]
        [InverseProperty("CalculateLearningQueue")]
        public virtual EnrollLearnings EnrollLearning { get; set; }
        [ForeignKey("SetupModuleId")]
        [InverseProperty("CalculateLearningQueue")]
        public virtual SetupModules SetupModule { get; set; }
    }
}
