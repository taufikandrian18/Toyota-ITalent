using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class CourseLearningObjectives
    {
        public int LearningObjectiveId { get; set; }
        public int CourseId { get; set; }
        [Required]
        [StringLength(1024)]
        public string LearningObjectiveName { get; set; }

        [ForeignKey("CourseId")]
        [InverseProperty("CourseLearningObjectives")]
        public virtual Courses Course { get; set; }
    }
}
