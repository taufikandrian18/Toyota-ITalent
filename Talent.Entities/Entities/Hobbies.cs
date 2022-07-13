using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Hobbies
    {
        public Hobbies()
        {
            EmployeeHobbyMappings = new HashSet<EmployeeHobbyMappings>();
        }

        [Key]
        public int HobbyId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }

        [InverseProperty("Hobby")]
        public virtual ICollection<EmployeeHobbyMappings> EmployeeHobbyMappings { get; set; }
    }
}
