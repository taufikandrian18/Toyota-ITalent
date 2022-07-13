using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductProgramMappings
    {
        public ProductProgramMappings()
        {
        }
        [Key]
        public Guid ProductProgramMappingId { get; set; }
        public Guid ProductProgramCategoryId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? Cms_MenuId { get; set; }
        public Guid Cms_ContentId { get; set; }
        public int IsSequence { get; set; }
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

        [ForeignKey("ProductProgramCategoryId")]
        [InverseProperty("ProductProgramMappings")]
        public virtual ProductProgramCategories ProductProgramCategory { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("ProductProgramMappings")]
        public virtual Products Product { get; set; }

        [ForeignKey("Cms_MenuId")]
        [InverseProperty("ProductProgramMappings")]
        public virtual Cms_Menus Cms_Menu { get; set; }

        [ForeignKey("Cms_ContentId")]
        [InverseProperty("ProductProgramMappings")]
        public virtual Cms_Contents Cms_Content { get; set; }
    }
}
