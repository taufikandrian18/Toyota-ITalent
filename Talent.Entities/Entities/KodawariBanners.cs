using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class KodawariBanners
    {
        public KodawariBanners()
        {
        }
        [Key]
        public Guid KodawariBannerId { get; set; }
        public Guid BlobId { get; set; }
        [Required]
        [StringLength(50)]
        public string KodawariBannerTitle { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("KodawariBanners")]
        public virtual Blobs Blob { get; set; }
    }
}
