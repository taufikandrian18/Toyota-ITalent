using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TeamDetails
    {
        [Key]
        public int TeamDetailId { get; set; }
        public int TeamId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("TeamDetails")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("TeamId")]
        [InverseProperty("TeamDetails")]
        public virtual Teams Team { get; set; }
    }
}
