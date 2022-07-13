using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class CoachSchedules
    {
        public CoachSchedules()
        {
            CoachBookingSchedules = new HashSet<CoachBookingSchedules>();
        }

        [Key]
        public int CoachScheduleId { get; set; }
        public int CoachId { get; set; }
        [StringLength(64)]
        public string ScheduleName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        [ForeignKey("CoachId")]
        [InverseProperty("CoachSchedules")]
        public virtual Coaches Coach { get; set; }
        [InverseProperty("CoachSchedule")]
        public virtual ICollection<CoachBookingSchedules> CoachBookingSchedules { get; set; }
    }
}
