using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalLevels
    {
        public ApprovalLevels()
        {
            ApprovalMappings = new HashSet<ApprovalMappings>();
        }

        [Key]
        public int ApprovalLevelId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [InverseProperty("ApprovalLevel")]
        public virtual ICollection<ApprovalMappings> ApprovalMappings { get; set; }
    }
}
