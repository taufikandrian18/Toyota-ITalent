using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
  public class ProductFeatureMappingModel
  {
    public Guid ProductFeatureMappingId { get; set; }
    public Guid ProductId { get; set; }
    public string OutletName { get; set; }
    public string ContributorName { get; set; }
    public string PositionName { get; set; }
    public Guid ProductTypeId { get; set; }
    public string ProductTypeName { get; set; }
    public Guid ProductFeatureCategoryId { get; set; }
    public string ProductFeatureCategoryName { get; set; }
    public Guid ProductFeatureId { get; set; }
    public string ProductFeatureName { get; set; }
    public Guid? Cms_FmbId { get; set; }
    public string Cms_FmbDescription { get; set; }
    public CmsFmbModel Cms_Fmb { get; set; }
    public Guid? Cms_WorkPrincipalId { get; set; }
    public string Cms_WorkPrincipalDescription { get; set; }
    public CmsWorkPrincipalModel Cms_WorkPrincipal { get; set; }
    public Guid? Cms_ContentId { get; set; }
    public string Cms_ContentName { get; set; }
    public string Cms_ContentDescription { get; set; }
    public CmsContentModel Cms_Content { get; set; }
    public Guid? Cms_OperationId { get; set; }
    public string Cms_OperationDescription { get; set; }
    public CmsOperationModel Cms_Operation { get; set; }
    public Guid? Cms_SettingId { get; set; }
    public string Cms_SettingDescription { get; set; }
    public CmsSettingModel Cms_Setting { get; set; }
    public bool? IsSpecial { get; set; }
    public string ProductFeatureMappingApprovalStatus { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class ProductFeatureMappingPaginate
  {
    public List<ProductFeatureMappingModel> ProductFeatureMappings { get; set; }
    public int TotalProductFeatureMapping { get; set; }
  }

  public class ProductFeatureMappingGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public Guid ProductId { get; set; }

    public String Query { get; set; }

    public bool? ApprovedAt { get; set; }
  }

  public class ProductFeatureMappingCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductTypeId { get; set; }

    [Required]
    public Guid ProductFeatureCategoryId { get; set; }

    [Required]
    public Guid ProductFeatureId { get; set; }

    public Guid? Cms_FmbId { get; set; }

    public Guid? Cms_WorkPrincipalId { get; set; }

    public Guid Cms_ContentId { get; set; }

    public Guid? Cms_OperationId { get; set; }

    public Guid? Cms_SettingId { get; set; }

    public bool IsSpecial { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }

  public class ProductFeatureMappingUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid ProductFeatureMappingId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductTypeId { get; set; }

    [Required]
    public Guid ProductFeatureCategoryId { get; set; }

    [Required]
    public Guid ProductFeatureId { get; set; }

    public Guid? Cms_FmbId { get; set; }

    public Guid? Cms_WorkPrincipalId { get; set; }

    public Guid Cms_ContentId { get; set; }

    public Guid? Cms_OperationId { get; set; }

    public Guid? Cms_SettingId { get; set; }

    public bool IsSpecial { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }

  public class DeleteProductFeatureMappingModel
  {
    public Guid ProductFeatureMappingId { get; set; }
    public bool isDeleteProductFeatureMapping { get; set; }
  }

  public class ProductFeatureMappingUpdateApprovalModel
  {
    public Guid ProductFeatureMappingId { get; set; }
    public bool isUpdateProductFeatureMappingApproval { get; set; }
  }
}
