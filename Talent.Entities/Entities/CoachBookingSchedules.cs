using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class CoachBookingSchedules
    {
        [Key]
        public int CoachBookingScheduleId { get; set; }
        public int CoachScheduleId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("CoachScheduleId")]
        [InverseProperty("CoachBookingSchedules")]
        public virtual CoachSchedules CoachSchedule { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("CoachBookingSchedules")]
        public virtual Employees Employee { get; set; }
    }
}
