using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class RewardPoints
    {
        [Key]
        public int RewardPointId { get; set; }
        public int RewardId { get; set; }
        public int RewardPointTypeId { get; set; }
        public int Score { get; set; }

        [ForeignKey("RewardId")]
        [InverseProperty("RewardPoints")]
        public virtual Rewards Reward { get; set; }
        [ForeignKey("RewardPointTypeId")]
        [InverseProperty("RewardPoints")]
        public virtual RewardPointTypes RewardPointType { get; set; }
    }
}
