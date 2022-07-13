using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Regions
    {
        public Regions()
        {
            Outlets = new HashSet<Outlets>();
        }

        [Key]
        public int RegionId { get; set; }
        [Required]
        [StringLength(64)]
        public string RegionName { get; set; }

        [InverseProperty("Region")]
        public virtual ICollection<Outlets> Outlets { get; set; }
    }
}
