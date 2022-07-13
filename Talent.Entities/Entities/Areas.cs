using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Areas
    {
        public Areas()
        {
            Outlets = new HashSet<Outlets>();
        }

        [Key]
        [StringLength(32)]
        public string AreaId { get; set; }
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [InverseProperty("Area")]
        public virtual ICollection<Outlets> Outlets { get; set; }
    }
}
