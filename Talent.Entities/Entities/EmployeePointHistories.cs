using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EmployeePointHistories
    {
        [Key]
        public int EmployeePointHistoryId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int RewardPointTypeId { get; set; }
        public int PointTransactionTypeId { get; set; }
        public int Point { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("EmployeePointHistories")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("PointTransactionTypeId")]
        [InverseProperty("EmployeePointHistories")]
        public virtual PointTransactionTypes PointTransactionType { get; set; }
        [ForeignKey("RewardPointTypeId")]
        [InverseProperty("EmployeePointHistories")]
        public virtual RewardPointTypes RewardPointType { get; set; }
    }
}
