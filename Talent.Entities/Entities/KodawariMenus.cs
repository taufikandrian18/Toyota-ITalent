using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class KodawariMenus
    {
        public KodawariMenus()
        {
            Kodawaris = new HashSet<Kodawaris>();
            KodawariDownloads = new HashSet<KodawariDownloads>();
        }
        [Key]
        public Guid KodawariMenuId { get; set; }
        public string KodawariMenuName { get; set; }
        public int KodawariMenuSequence { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty("KodawariMenu")]
        public virtual ICollection<Kodawaris> Kodawaris { get; set; }
        [InverseProperty("KodawariMenu")]
        public virtual ICollection<KodawariDownloads> KodawariDownloads { get; set; }
    }
}
