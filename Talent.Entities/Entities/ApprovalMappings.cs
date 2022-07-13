using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalMappings
    {
        public ApprovalMappings()
        {
            Approvals = new HashSet<Approvals>();
        }

        [Key]
        public int ApprovalMappingid { get; set; }
        [Required]
        [StringLength(8)]
        public string PageId { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public int ApprovalLevelId { get; set; }

        [ForeignKey("ApprovalLevelId")]
        [InverseProperty("ApprovalMappings")]
        public virtual ApprovalLevels ApprovalLevel { get; set; }
        [ForeignKey("PageId")]
        [InverseProperty("ApprovalMappings")]
        public virtual Pages Page { get; set; }
        [InverseProperty("ApprovalMapping")]
        public virtual ICollection<Approvals> Approvals { get; set; }
    }
}
