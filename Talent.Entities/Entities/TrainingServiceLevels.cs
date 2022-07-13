using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TrainingServiceLevels
    {
        [Key]
        public int TrainingServiceLevelId { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
        public int BasicLevel { get; set; }
        public int FundamentalLevel { get; set; }
        public int AdvanceLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
    }
}
