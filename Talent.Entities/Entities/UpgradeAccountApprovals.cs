using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class UpgradeAccountApprovals
    {
        [Key]
        [StringLength(64)]
        public string Guid { get; set; }
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employees Employee { get; set; }
    }
}
