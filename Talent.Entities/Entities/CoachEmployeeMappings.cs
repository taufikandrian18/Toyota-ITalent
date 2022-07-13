using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class CoachEmployeeMappings
    {
        public int TeamId { get; set; }
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("CoachEmployeeMappings")]
        public virtual Coaches Employee { get; set; }
        [ForeignKey("TeamId")]
        [InverseProperty("CoachEmployeeMappings")]
        public virtual Teams Team { get; set; }
    }
}
