using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
  public class ProductSPWAMappingModel
  {
    public Guid ProductSPWAMappingId { get; set; }
    public Guid ProductSPWACategoryId { get; set; }
    public string ProductSPWACategoryName { get; set; }
    public Guid ProductId { get; set; }
    public Guid Cms_MenuId { get; set; }
    public string Cms_MenuName { get; set; }
    public string Cms_MenuCategory { get; set; }
    public Guid Cms_ContentId { get; set; }
    public string Cms_ContentName { get; set; }
    public Guid Cms_ContentBlobId { get; set; }
    public string Cms_ContentImageUrl { get; set; }
    public int IsSequence { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class ProductSPWAMappingPaginate
  {
    public List<ProductSPWAMappingModel> ProductSPWAMappings { get; set; }
    public int TotalProductSPWAMapping { get; set; }
  }

  public class ProductSPWAMappingGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public Guid ProductId { get; set; }
    public string Query { get; set; }
  }

  public class ProductSPWAMappingCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductSPWACategoryId { get; set; }

    [Required]
    public int IsSequence { get; set; }

    [Required]
    public Guid Cms_ContentId { get; set; }

    [Required]
    public Guid Cms_MenuId { get; set; }

    public DateTime? ApprovedAt { get; set; }

  }

  public class ProductSPWAMappingUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid ProductSPWAMappingId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductSPWACategoryId { get; set; }

    [Required]
    public int IsSequence { get; set; }

    [Required]
    public Guid Cms_ContentId { get; set; }

    [Required]
    public Guid Cms_MenuId { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }
  public class DeleteProductSPWAMappingModel
  {
    public Guid ProductSPWAMappingId { get; set; }
    public bool isDeleteProductSPWAMapping { get; set; }
  }
}
