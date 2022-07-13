using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
  public class ProductProgramMappingModel
  {
    public Guid ProductProgramMappingId { get; set; }
    public Guid ProductProgramCategoryId { get; set; }
    public string ProductProgramCategoryName { get; set; }
    public Guid ProductId { get; set; }
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

  public class ProductProgramMappingPaginate
  {
    public List<ProductProgramMappingModel> ProductProgramMappings { get; set; }
    public int TotalProductProgramMapping { get; set; }
  }

  public class ProductProgramMappingGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public Guid ProductId { get; set; }

    public string Query { get; set; }
  }

  public class ProductProgramMappingCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductProgramCategoryId { get; set; }

    [Required]
    public int IsSequence { get; set; }

    [Required]
    public Guid Cms_ContentId { get; set; }

    public DateTime? ApprovedAt { get; set; }

  }

  public class ProductProgramMappingUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid ProductProgramMappingId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductProgramCategoryId { get; set; }

    [Required]
    public int IsSequence { get; set; }

    [Required]
    public Guid Cms_ContentId { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }

  public class DeleteProductProgramMappingModel
  {
    public Guid ProductProgramMappingId { get; set; }
    public bool isDeleteProductProgramMapping { get; set; }
  }
}
