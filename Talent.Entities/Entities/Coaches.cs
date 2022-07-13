using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Coaches
    {
        public Coaches()
        {
            CoachEmployeeMappings = new HashSet<CoachEmployeeMappings>();
            CoachRatings = new HashSet<CoachRatings>();
            CoachSchedules = new HashSet<CoachSchedules>();
            CoachTopicMappings = new HashSet<CoachTopicMappings>();
            Rewards = new HashSet<Rewards>();
            TrainingModuleMappings = new HashSet<TrainingModuleMappings>();
        }

        [Key]
        public int CoachId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public bool CoachIsActive { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("Coaches")]
        public virtual Employees Employee { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<CoachEmployeeMappings> CoachEmployeeMappings { get; set; }
        [InverseProperty("Coach")]
        public virtual ICollection<CoachRatings> CoachRatings { get; set; }
        [InverseProperty("Coach")]
        public virtual ICollection<CoachSchedules> CoachSchedules { get; set; }
        [InverseProperty("Coach")]
        public virtual ICollection<CoachTopicMappings> CoachTopicMappings { get; set; }
        [InverseProperty("Coach")]
        public virtual ICollection<Rewards> Rewards { get; set; }
        [InverseProperty("Coach")]
        public virtual ICollection<TrainingModuleMappings> TrainingModuleMappings { get; set; }
    }
}
