using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalPositionMappings
    {
        [Key]
        public int PositionId { get; set; }
        public int ApprovalLevel { get; set; }
        [Required]
        public string Description { get; set; }

        [ForeignKey("PositionId")]
        [InverseProperty("ApprovalPositionMappings")]
        public virtual Positions Position { get; set; }
    }
}
