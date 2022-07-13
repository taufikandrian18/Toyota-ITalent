using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalStatus
    {
        public ApprovalStatus()
        {
            ApprovalHistories = new HashSet<ApprovalHistories>();
            Approvals = new HashSet<Approvals>();
            RotateTeamMembers = new HashSet<RotateTeamMembers>();
            TeamMemberRequests = new HashSet<TeamMemberRequests>();
            TrainingInvitations = new HashSet<TrainingInvitations>();
        }

        public int ApprovalStatusId { get; set; }
        [Required]
        [StringLength(64)]
        public string ApprovalName { get; set; }

        [InverseProperty("ApprovalStatusNavigation")]
        public virtual ICollection<ApprovalHistories> ApprovalHistories { get; set; }
        [InverseProperty("ApprovalStatus")]
        public virtual ICollection<Approvals> Approvals { get; set; }
        [InverseProperty("ApprovalStatus")]
        public virtual ICollection<RotateTeamMembers> RotateTeamMembers { get; set; }
        [InverseProperty("ApprovalStatus")]
        public virtual ICollection<TeamMemberRequests> TeamMemberRequests { get; set; }
        [InverseProperty("ApprovalStatus")]
        public virtual ICollection<TrainingInvitations> TrainingInvitations { get; set; }
    }
}
