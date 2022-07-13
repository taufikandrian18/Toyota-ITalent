using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class KodawariDownloads
    {
        public KodawariDownloads()
        {
        }
        [Key]
        public Guid KodawariDownloadId { get; set; }
        public Guid KodawariMenuId { get; set; }
        public Guid BlobId { get; set; }
        [Required]
        [StringLength(50)]
        public string KodawariDownloadTitle { get; set; }
        public bool IsDownloadable { get; set; }
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

        [ForeignKey("KodawariMenuId")]
        [InverseProperty("KodawariDownloads")]
        public virtual KodawariMenus KodawariMenu { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("KodawariDownloads")]
        public virtual Blobs Blob { get; set; }
    }
}
