using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class CoursePrerequisiteMappings
    {
        [Key]
        public int CoursePrerequisiteMappingId { get; set; }
        public int PrevCourseId { get; set; }
        public int? NextCourseId { get; set; }
        public int? NextSetupModuleId { get; set; }

        [ForeignKey("NextCourseId")]
        [InverseProperty("CoursePrerequisiteMappingsNextCourse")]
        public virtual Courses NextCourse { get; set; }
        [ForeignKey("NextSetupModuleId")]
        [InverseProperty("CoursePrerequisiteMappings")]
        public virtual SetupModules NextSetupModule { get; set; }
        [ForeignKey("PrevCourseId")]
        [InverseProperty("CoursePrerequisiteMappingsPrevCourse")]
        public virtual Courses PrevCourse { get; set; }
    }
}
