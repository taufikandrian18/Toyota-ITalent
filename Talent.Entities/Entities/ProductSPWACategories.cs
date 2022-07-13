using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductSPWACategories
    {
        public ProductSPWACategories()
        {
            ProductSPWAMappings = new HashSet<ProductSPWAMappings>();
        }
        [Key]
        public Guid ProductSPWACategoryId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? BlobId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductSPWACategoryName { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string ProductSPWACategoryDescription { get; set; }
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
        [InverseProperty("ProductSPWACategories")]
        public virtual Products Product { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("ProductSPWACategories")]
        public virtual Blobs Blob { get; set; }

        [InverseProperty("ProductSPWACategory")]
        public virtual ICollection<ProductSPWAMappings> ProductSPWAMappings { get; set; }
    }
}
