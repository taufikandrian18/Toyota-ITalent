using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Rewards
    {
        public Rewards()
        {
            EmployeeRewardMappings = new HashSet<EmployeeRewardMappings>();
            RewardPoints = new HashSet<RewardPoints>();
        }

        [Key]
        public int RewardId { get; set; }
        public int RewardTypeId { get; set; }
        public int? ModuleId { get; set; }
        public int? CoachId { get; set; }
        public int? EventId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        [Required]
        public string TermsAndConditions { get; set; }
        [Required]
        public string HowToUse { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("CoachId")]
        [InverseProperty("Rewards")]
        public virtual Coaches Coach { get; set; }
        [ForeignKey("EventId")]
        [InverseProperty("Rewards")]
        public virtual Events Event { get; set; }
        [ForeignKey("ModuleId")]
        [InverseProperty("Rewards")]
        public virtual Modules Module { get; set; }
        [ForeignKey("RewardTypeId")]
        [InverseProperty("Rewards")]
        public virtual RewardTypes RewardType { get; set; }
        [InverseProperty("Reward")]
        public virtual ICollection<EmployeeRewardMappings> EmployeeRewardMappings { get; set; }
        [InverseProperty("Reward")]
        public virtual ICollection<RewardPoints> RewardPoints { get; set; }
    }
}
