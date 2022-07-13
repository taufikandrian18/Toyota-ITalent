using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductTypes
    {
        public ProductTypes()
        {
            ProductGalleries = new HashSet<ProductGalleries>();
            ProductTypeMappings = new HashSet<ProductCompetitorMappings>();
            ProductTypeCompetitorMappings = new HashSet<ProductCompetitorMappings>();
            ProductFeatureMappings = new HashSet<ProductFeatureMappings>();
        }

        [Key]
        public Guid ProductTypeId { get; set; }
        public Guid BlobId { get; set; }
        public Guid ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductTypeName { get; set; }
        [StringLength(250)]
        public string ProductTypeSalesTalk { get; set; }
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
        [InverseProperty("ProductTypes")]
        public virtual Blobs Blob { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("ProductTypes")]
        public virtual Products Product { get; set; }

        [InverseProperty("ProductTypes")]
        public virtual ICollection<ProductGalleries> ProductGalleries { get; set; }
        [InverseProperty("ProductType")]
        public virtual ICollection<ProductCompetitorMappings> ProductTypeMappings { get; set; }
        [InverseProperty("ProductCompetitorType")]
        public virtual ICollection<ProductCompetitorMappings> ProductTypeCompetitorMappings { get; set; }
        [InverseProperty("ProductType")]
        public virtual ICollection<ProductFeatureMappings> ProductFeatureMappings { get; set; }
    }
}
