using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductTips
    {
        public ProductTips()
        {
        }
        [Key]
        public Guid ProductTipId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BlobId { get; set; }
        public Guid ProductTipCategoryId { get; set; }
        public string OutletId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductTipTitle { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string ProductTipDescription { get; set; }
        public int IsSequence { get; set; }
        [StringLength(50)]
        public string IsApproved { get; set; }
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

        [ForeignKey("ProductId")]
        [InverseProperty("ProductTips")]
        public virtual Products Product { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("ProductTips")]
        public virtual Blobs Blob { get; set; }

        [ForeignKey("ProductTipCategoryId")]
        [InverseProperty("ProductTips")]
        public virtual ProductTipCategories ProductTipCategory { get; set; }

        [ForeignKey("OutletId")]
        [InverseProperty("ProductTips")]
        public virtual Outlets Outlet { get; set; }
    }
}
