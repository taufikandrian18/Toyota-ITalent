using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class GuideTypes
    {
        public GuideTypes()
        {
            Guides = new HashSet<Guides>();
        }

        [Key]
        public int GuideTypeId { get; set; }
        [Required]
        [StringLength(32)]
        public string Name { get; set; }

        [InverseProperty("GuideType")]
        public virtual ICollection<Guides> Guides { get; set; }
    }
}
