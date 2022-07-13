using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class RewardTypes
    {
        public RewardTypes()
        {
            Rewards = new HashSet<Rewards>();
        }

        [Key]
        public int RewardTypeId { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [InverseProperty("RewardType")]
        public virtual ICollection<Rewards> Rewards { get; set; }
    }
}
