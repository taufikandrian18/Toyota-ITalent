using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class PointTransactionTypes
    {
        public PointTransactionTypes()
        {
            EmployeePointHistories = new HashSet<EmployeePointHistories>();
        }

        [Key]
        public int PointTransactionTypeId { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [InverseProperty("PointTransactionType")]
        public virtual ICollection<EmployeePointHistories> EmployeePointHistories { get; set; }
    }
}
