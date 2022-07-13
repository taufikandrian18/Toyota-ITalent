using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class MobilePages
    {
        public MobilePages()
        {
            Banners = new HashSet<Banners>();
        }

        [Key]
        public int MobilePageId { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
        [Required]
        [StringLength(512)]
        public string Route { get; set; }
        public bool IsForBanner { get; set; }

        [InverseProperty("MobilePage")]
        public virtual ICollection<Banners> Banners { get; set; }
    }
}
