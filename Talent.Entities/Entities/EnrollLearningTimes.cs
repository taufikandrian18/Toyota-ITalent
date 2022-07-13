using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EnrollLearningTimes
    {
        [Key]
        public int EnrollLearningTimeId { get; set; }
        public int? EnrollLearningId { get; set; }
        public int SetupModuleId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        [ForeignKey("EnrollLearningId")]
        [InverseProperty("EnrollLearningTimes")]
        public virtual EnrollLearnings EnrollLearning { get; set; }
        [ForeignKey("SetupModuleId")]
        [InverseProperty("EnrollLearningTimes")]
        public virtual SetupModules SetupModule { get; set; }
    }
}
