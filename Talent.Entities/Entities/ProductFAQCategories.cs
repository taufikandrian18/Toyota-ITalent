using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductFAQCategories
    {
        public ProductFAQCategories()
        {
            ProductFaqs = new HashSet<ProductFAQs>();
            ProductFaqUsers = new HashSet<ProductFAQUsers>();
        }
        [Key]
        public Guid ProductFaqCategoryId { get; set; }
        public Guid ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductFaqCategoryName { get; set; }
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
        [InverseProperty("ProductFAQCategories")]
        public virtual Products Product { get; set; }

        [InverseProperty("ProductFAQCategory")]
        public virtual ICollection<ProductFAQs> ProductFaqs { get; set; }
        [InverseProperty("ProductFAQCategory")]
        public virtual ICollection<ProductFAQUsers> ProductFaqUsers { get; set; }
    }
}
