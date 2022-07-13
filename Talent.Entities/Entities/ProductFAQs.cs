using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductFAQs
    {
        public ProductFAQs()
        {
        }
        [Key]
        public Guid ProductFaqId { get; set; }
        public Guid ProductFaqCategoryId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BlobId { get; set; }
        [Required]
        public int ProductFaqSequence { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductFaqTitle { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string ProductFaqDescription { get; set; }
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

        [ForeignKey("ProductFaqCategoryId")]
        [InverseProperty("ProductFaqs")]
        public virtual ProductFAQCategories ProductFAQCategory { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("ProductFaqs")]
        public virtual Blobs Blob { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("ProductFaqs")]
        public virtual Products Product { get; set; }
    }
}
