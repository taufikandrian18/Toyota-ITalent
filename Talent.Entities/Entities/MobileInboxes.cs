using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class MobileInboxes
    {
        [Key]
        public int MobileInboxId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int MobileInboxTypeId { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Message { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }
        [StringLength(64)]
        public string ResignEmployeeId { get; set; }
        public int? RotateTeamMemberId { get; set; }
        public int? TeamMemberRequestId { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public bool IsRead { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("MobileInboxesEmployee")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("MobileInboxTypeId")]
        [InverseProperty("MobileInboxes")]
        public virtual MobileInboxTypes MobileInboxType { get; set; }
        [ForeignKey("ResignEmployeeId")]
        [InverseProperty("MobileInboxesResignEmployee")]
        public virtual Employees ResignEmployee { get; set; }
        [ForeignKey("RotateTeamMemberId")]
        [InverseProperty("MobileInboxes")]
        public virtual RotateTeamMembers RotateTeamMember { get; set; }
        [ForeignKey("TeamMemberRequestId")]
        [InverseProperty("MobileInboxes")]
        public virtual TeamMemberRequests TeamMemberRequest { get; set; }
    }
}
