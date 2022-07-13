using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Menus
    {
        public Menus()
        {
            Pages = new HashSet<Pages>();
        }

        [Key]
        [StringLength(8)]
        public string MenuId { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [InverseProperty("Menu")]
        public virtual ICollection<Pages> Pages { get; set; }
    }
}
