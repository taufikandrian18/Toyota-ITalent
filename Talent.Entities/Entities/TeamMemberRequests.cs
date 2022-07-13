using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TeamMemberRequests
    {
        public TeamMemberRequests()
        {
            MobileInboxes = new HashSet<MobileInboxes>();
        }

        [Key]
        public int TeamMemberRequestId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int TeamId { get; set; }
        public int ApprovalStatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }

        [ForeignKey("ApprovalStatusId")]
        [InverseProperty("TeamMemberRequests")]
        public virtual ApprovalStatus ApprovalStatus { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("TeamMemberRequests")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("TeamId")]
        [InverseProperty("TeamMemberRequests")]
        public virtual Teams Team { get; set; }
        [InverseProperty("TeamMemberRequest")]
        public virtual ICollection<MobileInboxes> MobileInboxes { get; set; }
    }
}
