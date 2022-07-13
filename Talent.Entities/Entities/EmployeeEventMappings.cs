using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EmployeeEventMappings
    {
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int EventId { get; set; }
        public bool? IsJoined { get; set; }
        public DateTime? JoinedAt { get; set; }
        public bool? IsSaved { get; set; }
        public DateTime? SavedAt { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("EmployeeEventMappings")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("EventId")]
        [InverseProperty("EmployeeEventMappings")]
        public virtual Events Event { get; set; }
    }
}
