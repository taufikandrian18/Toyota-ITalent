using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductGalleries
    {
        public ProductGalleries()
        {
        }
        [Key]
        public Guid ProductGalleryId { get; set; }
        public Guid BlobId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductTypeId { get; set; }
        [StringLength(50)]
        public string ProductGalleryColorCode { get; set; }
        [StringLength(50)]
        public string ProductGalleryColorName { get; set; }
        public bool IsColor { get; set; }
        [StringLength(50)]
        public string IsApproved { get; set; }
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
        [InverseProperty("ProductGalleries")]
        public virtual Blobs Blob { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("ProductGalleries")]
        public virtual Products Product { get; set; }

        [ForeignKey("ProductTypeId")]
        [InverseProperty("ProductGalleries")]
        public virtual ProductTypes ProductTypes { get; set; }
    }
}
