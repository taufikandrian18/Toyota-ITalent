using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductPhotos
    {
        public ProductPhotos()
        {
            ProductPhoto360Mappings = new HashSet<ProductPhoto360Mappings>();
        }
        [Key]
        public Guid ProductPhotoId { get; set; }
        [Required]
        public Guid BlobId { get; set; }
        public bool Is360Photo { get; set; }
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
        [InverseProperty("ProductPhotos")]
        public virtual Blobs Blob { get; set; }

        [InverseProperty("ProductPhoto")]
        public virtual ICollection<ProductPhoto360Mappings> ProductPhoto360Mappings { get; set; }
    }
}
