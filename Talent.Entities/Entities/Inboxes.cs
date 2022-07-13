using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Inboxes
    {
        [Key]
        public int InboxId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [Required]
        [StringLength(255)]
        public string Message { get; set; }
        public string LinkToPage { get; set; }
        public int? ApprovalId { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsRead { get; set; }
        public bool IsDone { get; set; }

        [ForeignKey("ApprovalId")]
        [InverseProperty("Inboxes")]
        public virtual Approvals Approval { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("Inboxes")]
        public virtual Employees Employee { get; set; }
    }
}
