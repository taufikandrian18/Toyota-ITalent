using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Cities
    {
        public Cities()
        {
            Outlets = new HashSet<Outlets>();
        }

        [Key]
        [StringLength(32)]
        public string CityId { get; set; }
        [Required]
        [StringLength(255)]
        public string CityName { get; set; }
        [StringLength(250)]
        public string ParentCode { get; set; }

        [InverseProperty("City")]
        public virtual ICollection<Outlets> Outlets { get; set; }
    }
}
