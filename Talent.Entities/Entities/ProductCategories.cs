using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductCategories
    {
        public ProductCategories()
        {
            Products = new HashSet<Products>();
        }
        [Key]
        public Guid ProductCategoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductCategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty("ProductCategory")]
        public virtual ICollection<Products> Products { get; set; }
    }
}
