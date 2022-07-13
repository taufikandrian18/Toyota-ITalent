using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EmployeePositionMappings
    {
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string Information { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("EmployeePositionMappings")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("PositionId")]
        [InverseProperty("EmployeePositionMappings")]
        public virtual Positions Position { get; set; }
    }
}
