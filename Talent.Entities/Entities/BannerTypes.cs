using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class BannerTypes
    {
        public BannerTypes()
        {
            Banners = new HashSet<Banners>();
        }

        [Key]
        public int BannerTypeId { get; set; }
        [Required]
        [StringLength(32)]
        public string Name { get; set; }

        [InverseProperty("BannerType")]
        public virtual ICollection<Banners> Banners { get; set; }
    }
}
