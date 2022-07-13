using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EmployeeRewardMappings
    {
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int RewardId { get; set; }
        public DateTime RedeemedAt { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("EmployeeRewardMappings")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("RewardId")]
        [InverseProperty("EmployeeRewardMappings")]
        public virtual Rewards Reward { get; set; }
    }
}
