using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EmployeeHobbyMappings
    {
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int HobbyId { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("EmployeeHobbyMappings")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("HobbyId")]
        [InverseProperty("EmployeeHobbyMappings")]
        public virtual Hobbies Hobby { get; set; }
    }
}
