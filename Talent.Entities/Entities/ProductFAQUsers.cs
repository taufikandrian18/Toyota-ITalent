using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductFAQUsers
    {
        public ProductFAQUsers()
        {
        }
        [Key]
        public Guid ProductFAQUserId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductFaqCategoryId { get; set; }
        [Required]
        [StringLength(250)]
        public string ProductFAQUserQuestion { get; set; }
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
        [InverseProperty("ProductFaqUsers")]
        public virtual Products Product { get; set; }

        [ForeignKey("ProductFaqCategoryId")]
        [InverseProperty("ProductFaqUsers")]
        public virtual ProductFAQCategories ProductFAQCategory { get; set; }
    }
}
