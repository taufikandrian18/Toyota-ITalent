using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductFeatureMappings
    {
        public ProductFeatureMappings()
        {
        }
        [Key]
        public Guid ProductFeatureMappingId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductTypeId { get; set; }
        public Guid ProductFeatureId { get; set; }
        public Guid ProductFeatureCategoryId { get; set; }
        public Guid? Cms_FmbId { get; set; }
        public Guid? Cms_WorkPrincipalId { get; set; }
        public Guid? Cms_ContentId { get; set; }
        public Guid? Cms_OperationId { get; set; }
        public Guid? Cms_SettingId { get; set; }
        public bool? IsSpecial { get; set; }
        public string ProductFeatureMappingApprovalStatus { get; set; }
        public string EnumContributorFlagging { get; set; }
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
        [InverseProperty("ProductFeatureMappings")]
        public virtual Products Product { get; set; }

        [ForeignKey("ProductTypeId")]
        [InverseProperty("ProductFeatureMappings")]
        public virtual ProductTypes ProductType { get; set; }

        [ForeignKey("ProductFeatureId")]
        [InverseProperty("ProductFeatureMappings")]
        public virtual ProductFeatures ProductFeature { get; set; }

        [ForeignKey("ProductFeatureCategoryId")]
        [InverseProperty("ProductFeatureMappings")]
        public virtual ProductFeatureCategories ProductFeatureCategory { get; set; }

        [ForeignKey("Cms_FmbId")]
        [InverseProperty("ProductFeatureMappings")]
        public virtual Cms_Fmbs Cms_Fmb { get; set; }

        [ForeignKey("Cms_WorkPrincipalId")]
        [InverseProperty("ProductFeatureMappings")]
        public virtual Cms_WorkPrincipals Cms_WorkPrincipal { get; set; }

        [ForeignKey("Cms_ContentId")]
        [InverseProperty("ProductFeatureMappings")]
        public virtual Cms_Contents Cms_Content { get; set; }

        [ForeignKey("Cms_OperationId")]
        [InverseProperty("ProductFeatureMappings")]
        public virtual Cms_Operations Cms_Operation { get; set; }

        [ForeignKey("Cms_SettingId")]
        [InverseProperty("ProductFeatureMappings")]
        public virtual Cms_Settings Cms_Setting { get; set; }
    }
}
