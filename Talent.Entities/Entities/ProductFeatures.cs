using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductFeatures
    {
        public ProductFeatures()
        {
            ProductFeatureMappings = new HashSet<ProductFeatureMappings>();
        }
        [Key]
        public Guid ProductFeatureId { get; set; }
        public Guid BlobId { get; set; }
        public Guid ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductFeatureName { get; set; }
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

        [ForeignKey("ProductId")]
        [InverseProperty("ProductFeatures")]
        public virtual Products Product { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("ProductFeatures")]
        public virtual Blobs Blob { get; set; }

        [InverseProperty("ProductFeature")]
        public virtual ICollection<ProductFeatureMappings> ProductFeatureMappings { get; set; }
    }
}
