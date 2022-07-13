using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    [Table("EBadges")]
    public partial class Ebadges
    {
        public Ebadges()
        {
            EmployeeBadges = new HashSet<EmployeeBadges>();
            Topics = new HashSet<Topics>();
        }

        [Key]
        [Column("EBadgeId")]
        public int EbadgeId { get; set; }
        [Required]
        [Column("EBadgeName")]
        [StringLength(64)]
        public string EbadgeName { get; set; }

        [InverseProperty("Ebadge")]
        public virtual ICollection<EmployeeBadges> EmployeeBadges { get; set; }
        [InverseProperty("Ebadge")]
        public virtual ICollection<Topics> Topics { get; set; }
    }
}
