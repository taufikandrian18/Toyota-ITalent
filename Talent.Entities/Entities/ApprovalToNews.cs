using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalToNews
    {
        [Key]
        public int ApprovalId { get; set; }
        public int NewsId { get; set; }

        [ForeignKey("ApprovalId")]
        [InverseProperty("ApprovalToNews")]
        public virtual Approvals Approval { get; set; }
        [ForeignKey("NewsId")]
        [InverseProperty("ApprovalToNews")]
        public virtual News News { get; set; }
    }
}
