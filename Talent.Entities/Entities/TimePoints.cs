using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TimePoints
    {
        public TimePoints()
        {
            SetupModules = new HashSet<SetupModules>();
            TrainingModuleMappings = new HashSet<TrainingModuleMappings>();
        }

        [Key]
        public int TimePointId { get; set; }
        public int PointTypeId { get; set; }
        public int? Time { get; set; }
        public int? Score { get; set; }
        public int Points { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("PointTypeId")]
        [InverseProperty("TimePoints")]
        public virtual PointTypes PointType { get; set; }
        [InverseProperty("TimePoint")]
        public virtual ICollection<SetupModules> SetupModules { get; set; }
        [InverseProperty("TimePoint")]
        public virtual ICollection<TrainingModuleMappings> TrainingModuleMappings { get; set; }
    }
}
