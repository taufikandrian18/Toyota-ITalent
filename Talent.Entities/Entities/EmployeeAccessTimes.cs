using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EmployeeAccessTimes
    {
        [Key]
        public int EmployeeAccessTimeId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("EmployeeAccessTimes")]
        public virtual Employees Employee { get; set; }
    }
}
