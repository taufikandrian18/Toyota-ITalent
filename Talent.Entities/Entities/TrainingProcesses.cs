using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TrainingProcesses
    {
        [Key]
        public int TrainingProcessId { get; set; }
        public int TrainingInvitationId { get; set; }
        public int? AccomodationId { get; set; }
        public DateTime? DateofStayStart { get; set; }
        public DateTime? DateofStayEnd { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }

        [ForeignKey("AccomodationId")]
        [InverseProperty("TrainingProcesses")]
        public virtual Accommodations Accomodation { get; set; }
        [ForeignKey("TrainingInvitationId")]
        [InverseProperty("TrainingProcesses")]
        public virtual TrainingInvitations TrainingInvitation { get; set; }
    }
}
