using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class RotateTeamMembers
    {
        public RotateTeamMembers()
        {
            MobileInboxes = new HashSet<MobileInboxes>();
        }

        [Key]
        public int RotateTeamMemberId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int PreviousTeamId { get; set; }
        public int NextTeamId { get; set; }
        public int ApprovalStatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }

        [ForeignKey("ApprovalStatusId")]
        [InverseProperty("RotateTeamMembers")]
        public virtual ApprovalStatus ApprovalStatus { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("RotateTeamMembers")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("NextTeamId")]
        [InverseProperty("RotateTeamMembersNextTeam")]
        public virtual Teams NextTeam { get; set; }
        [ForeignKey("PreviousTeamId")]
        [InverseProperty("RotateTeamMembersPreviousTeam")]
        public virtual Teams PreviousTeam { get; set; }
        [InverseProperty("RotateTeamMember")]
        public virtual ICollection<MobileInboxes> MobileInboxes { get; set; }
    }
}
