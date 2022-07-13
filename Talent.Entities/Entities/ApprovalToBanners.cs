using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalToBanners
    {
        [Key]
        public int ApprovalId { get; set; }
        public int BannerId { get; set; }

        [ForeignKey("ApprovalId")]
        [InverseProperty("ApprovalToBanners")]
        public virtual Approvals Approval { get; set; }
        [ForeignKey("BannerId")]
        [InverseProperty("ApprovalToBanners")]
        public virtual Banners Banner { get; set; }
    }
}
