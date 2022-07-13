using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Dealers
    {
        public Dealers()
        {
            Outlets = new HashSet<Outlets>();
        }

        [Key]
        [StringLength(64)]
        public string DealerId { get; set; }
        [Required]
        [StringLength(128)]
        public string DealerName { get; set; }
        public bool OtherCompany { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }

        [InverseProperty("Dealer")]
        public virtual ICollection<Outlets> Outlets { get; set; }
    }
}
