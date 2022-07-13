using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ProgramTypes
    {
        public ProgramTypes()
        {
            Courses = new HashSet<Courses>();
        }

        [Key]
        public int ProgramTypeId { get; set; }
        [Required]
        [StringLength(128)]
        public string ProgramTypeName { get; set; }

        [InverseProperty("ProgramType")]
        public virtual ICollection<Courses> Courses { get; set; }
    }
}
