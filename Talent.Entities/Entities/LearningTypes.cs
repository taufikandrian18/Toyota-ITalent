using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class LearningTypes
    {
        public LearningTypes()
        {
            Courses = new HashSet<Courses>();
        }

        [Key]
        public int LearningTypeId { get; set; }
        [Required]
        [StringLength(32)]
        public string LearningName { get; set; }

        [InverseProperty("LearningType")]
        public virtual ICollection<Courses> Courses { get; set; }
    }
}
