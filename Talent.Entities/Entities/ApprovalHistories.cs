using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalHistories
    {
        [Key]
        public int ApprovalHistoryId { get; set; }
        public int ApprovalId { get; set; }
        public int ApprovalStatusId { get; set; }
        [Required]
        [StringLength(255)]
        public string ApprovalStatus { get; set; }
        [Required]
        [StringLength(32)]
        public string ActionBy { get; set; }
        public DateTime ActionAt { get; set; }

        [ForeignKey("ApprovalId")]
        [InverseProperty("ApprovalHistories")]
        public virtual Approvals Approval { get; set; }
        [ForeignKey("ApprovalStatusId")]
        [InverseProperty("ApprovalHistories")]
        public virtual ApprovalStatus ApprovalStatusNavigation { get; set; }
    }
}
