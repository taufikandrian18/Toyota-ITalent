using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    [Table("CFAMs")]
    public partial class Cfams
    {
        public Cfams()
        {
            Outlets = new HashSet<Outlets>();
        }

        [Key]
        [Column("CFAMId")]
        [StringLength(32)]
        public string Cfamid { get; set; }
        [Required]
        [Column("CFAMName")]
        [StringLength(255)]
        public string Cfamname { get; set; }

        [InverseProperty("Cfam")]
        public virtual ICollection<Outlets> Outlets { get; set; }
    }
}
