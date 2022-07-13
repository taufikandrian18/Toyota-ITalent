using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class CourseTrainingTypeMappings
    {
        public int CourseId { get; set; }
        public int TrainingTypeId { get; set; }
        public int? MinimumScore { get; set; }
        [StringLength(1024)]
        public string WorkloadRequirement { get; set; }
        public int? Percentage { get; set; }

        [ForeignKey("CourseId")]
        [InverseProperty("CourseTrainingTypeMappings")]
        public virtual Courses Course { get; set; }
        [ForeignKey("TrainingTypeId")]
        [InverseProperty("CourseTrainingTypeMappings")]
        public virtual TrainingTypes TrainingType { get; set; }
    }
}
