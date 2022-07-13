using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalToGuides
    {
        [Key]
        public int ApprovalId { get; set; }
        public int GuideId { get; set; }

        [ForeignKey("ApprovalId")]
        [InverseProperty("ApprovalToGuides")]
        public virtual Approvals Approval { get; set; }
        [ForeignKey("GuideId")]
        [InverseProperty("ApprovalToGuides")]
        public virtual Guides Guide { get; set; }
    }
}
