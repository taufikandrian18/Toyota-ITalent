using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductTipCategories
    {
        public ProductTipCategories()
        {
            ProductTips = new HashSet<ProductTips>();
        }
        [Key]
        public Guid ProductTipCategoryId { get; set; }
        public Guid ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductTipCategoryName { get; set; }
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
        [InverseProperty("ProductTipCategories")]
        public virtual Products Product { get; set; }

        [InverseProperty("ProductTipCategory")]
        public virtual ICollection<ProductTips> ProductTips { get; set; }
    }
}
