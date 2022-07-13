using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductFeatureCategories
    {
        public ProductFeatureCategories()
        {
            ProductFeatureMappings = new HashSet<ProductFeatureMappings>();
        }
        [Key]
        public Guid ProductFeatureCategoryId { get; set; }
        [Required]
        [StringLength(150)]
        public string ProductFeatureCategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty("ProductFeatureCategory")]
        public virtual ICollection<ProductFeatureMappings> ProductFeatureMappings { get; set; }
    }
}
