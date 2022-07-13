using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class RewardPointTypes
    {
        public RewardPointTypes()
        {
            EmployeePointHistories = new HashSet<EmployeePointHistories>();
            RewardPoints = new HashSet<RewardPoints>();
        }

        [Key]
        public int RewardPointTypeId { get; set; }
        [Required]
        [StringLength(32)]
        public string Name { get; set; }

        [InverseProperty("RewardPointType")]
        public virtual ICollection<EmployeePointHistories> EmployeePointHistories { get; set; }
        [InverseProperty("RewardPointType")]
        public virtual ICollection<RewardPoints> RewardPoints { get; set; }
    }
}
