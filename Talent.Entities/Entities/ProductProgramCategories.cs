using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductProgramCategories
    {
        public ProductProgramCategories()
        {
            ProductProgramMappings = new HashSet<ProductProgramMappings>();
        }
        [Key]
        public Guid ProductProgramCategoryId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? BlobId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductProgramCategoryName { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string ProductProgramCategoryDescription { get; set; }
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
        [InverseProperty("ProductProgramCategories")]
        public virtual Blobs Blob { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("ProductProgramCategories")]
        public virtual Products Product { get; set; }

        [InverseProperty("ProductProgramCategory")]
        public virtual ICollection<ProductProgramMappings> ProductProgramMappings { get; set; }
    }
}
