using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TrainingTypes
    {
        public TrainingTypes()
        {
            CourseTrainingTypeMappings = new HashSet<CourseTrainingTypeMappings>();
            SetupModules = new HashSet<SetupModules>();
        }

        [Key]
        public int TrainingTypeId { get; set; }
        [Required]
        [StringLength(64)]
        public string TrainingTypeName { get; set; }

        [InverseProperty("TrainingType")]
        public virtual ICollection<CourseTrainingTypeMappings> CourseTrainingTypeMappings { get; set; }
        [InverseProperty("TrainingType")]
        public virtual ICollection<SetupModules> SetupModules { get; set; }
    }
}
