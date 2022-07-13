using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Levels
    {
        public Levels()
        {
            Courses = new HashSet<Courses>();
        }

        [Key]
        public int LevelId { get; set; }
        [Required]
        [StringLength(128)]
        public string LevelName { get; set; }

        [InverseProperty("Level")]
        public virtual ICollection<Courses> Courses { get; set; }
    }
}
