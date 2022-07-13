using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class MaterialTypes
    {
        public MaterialTypes()
        {
            Modules = new HashSet<Modules>();
        }

        [Key]
        public int MaterialTypeId { get; set; }
        [Required]
        [StringLength(64)]
        public string MaterialTypeName { get; set; }

        [InverseProperty("MaterialType")]
        public virtual ICollection<Modules> Modules { get; set; }
    }
}
