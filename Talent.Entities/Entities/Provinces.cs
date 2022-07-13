using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Provinces
    {
        public Provinces()
        {
            Outlets = new HashSet<Outlets>();
        }

        [Key]
        [StringLength(32)]
        public string ProvinceId { get; set; }
        [Required]
        [StringLength(255)]
        public string ProvinceName { get; set; }
        [StringLength(250)]
        public string ParentCode { get; set; }

        [InverseProperty("Province")]
        public virtual ICollection<Outlets> Outlets { get; set; }
    }
}
