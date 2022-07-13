using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class CourseCategories
    {
        public CourseCategories()
        {
            Courses = new HashSet<Courses>();
        }

        [Key]
        public int CourseCategoryId { get; set; }
        [Required]
        [StringLength(64)]
        public string CourseCategoryName { get; set; }

        [InverseProperty("CourseCategory")]
        public virtual ICollection<Courses> Courses { get; set; }
    }
}
