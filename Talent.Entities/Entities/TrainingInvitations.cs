using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TrainingInvitations
    {
        public TrainingInvitations()
        {
            TrainingProcesses = new HashSet<TrainingProcesses>();
        }

        [Key]
        public int TrainingInvitationId { get; set; }
        public int TrainingId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int ApprovalStatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }

        [ForeignKey("ApprovalStatusId")]
        [InverseProperty("TrainingInvitations")]
        public virtual ApprovalStatus ApprovalStatus { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("TrainingInvitations")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("TrainingId")]
        [InverseProperty("TrainingInvitations")]
        public virtual Trainings Training { get; set; }
        [InverseProperty("TrainingInvitation")]
        public virtual ICollection<TrainingProcesses> TrainingProcesses { get; set; }
    }
}
