using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class PointTypes
    {
        public PointTypes()
        {
            TimePoints = new HashSet<TimePoints>();
        }

        [Key]
        public int PointTypeId { get; set; }
        [Required]
        [StringLength(64)]
        public string PointTypeName { get; set; }

        [InverseProperty("PointType")]
        public virtual ICollection<TimePoints> TimePoints { get; set; }
    }
}
