using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalToEvents
    {
        [Key]
        public int ApprovalId { get; set; }
        public int EventId { get; set; }

        [ForeignKey("ApprovalId")]
        [InverseProperty("ApprovalToEvents")]
        public virtual Approvals Approval { get; set; }
        [ForeignKey("EventId")]
        [InverseProperty("ApprovalToEvents")]
        public virtual Events Event { get; set; }
    }
}
