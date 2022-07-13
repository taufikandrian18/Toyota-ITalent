using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class Products
    {
        public Products()
        {
            ProductCustomers = new HashSet<ProductCustomers>();
            ProductGalleries = new HashSet<ProductGalleries>();
            ProductTypes = new HashSet<ProductTypes>();
            ProductFaqs = new HashSet<ProductFAQs>();
            ProductFAQCategories = new HashSet<ProductFAQCategories>();
            ProductFaqUsers = new HashSet<ProductFAQUsers>();
            ProductPhoto360Mappings = new HashSet<ProductPhoto360Mappings>();
            ProductMappings = new HashSet<ProductCompetitorMappings>();
            ProductCompetitorMappings = new HashSet<ProductCompetitorMappings>();
            ProductFeatureMappings = new HashSet<ProductFeatureMappings>();
            ProductFeatures = new HashSet<ProductFeatures>();
            ProductTips = new HashSet<ProductTips>();
            ProductTipCategories = new HashSet<ProductTipCategories>();
            ProductProgramMappings = new HashSet<ProductProgramMappings>();
            ProductProgramCategories = new HashSet<ProductProgramCategories>();
            ProductSPWACategories = new HashSet<ProductSPWACategories>();
            ProductSPWAMappings = new HashSet<ProductSPWAMappings>();
            Cms_Contents = new HashSet<Cms_Contents>();
        }
        [Key]
        public Guid ProductId { get; set; }
        public Guid BlobId { get; set; }
        public Guid ProductCategoryId { get; set; }
        [Required]
        [StringLength(150)]
        public string ProductName { get; set; }
        public int ProductSegment { get; set; }
        public bool? IsCompetitor { get; set; }
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
        [InverseProperty("Products")]
        public virtual Blobs Blob { get; set; }

        [ForeignKey("ProductCategoryId")]
        [InverseProperty("Products")]
        public virtual ProductCategories ProductCategory { get; set; }

        [InverseProperty("Product")]
        public virtual ICollection<ProductGalleries> ProductGalleries { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductTypes> ProductTypes { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductCustomers> ProductCustomers { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductFAQs> ProductFaqs { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductFAQCategories> ProductFAQCategories { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductFAQUsers> ProductFaqUsers { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductPhoto360Mappings> ProductPhoto360Mappings { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductCompetitorMappings> ProductMappings { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductFeatureMappings> ProductFeatureMappings { get; set; }
        [InverseProperty("ProductCompetitor")]
        public virtual ICollection<ProductCompetitorMappings> ProductCompetitorMappings { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductFeatures> ProductFeatures { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductTips> ProductTips { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductTipCategories> ProductTipCategories { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductProgramMappings> ProductProgramMappings { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductProgramCategories> ProductProgramCategories { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductSPWACategories> ProductSPWACategories { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductSPWAMappings> ProductSPWAMappings { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Cms_Contents> Cms_Contents { get; set; }
    }
}
