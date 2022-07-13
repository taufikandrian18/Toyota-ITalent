using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class ProductFeatureModel
  {
    public Guid ProductFeatureId { get; set; }
    public Guid BlobId { get; set; }
    public BlobModel Blob { get; set; }
    public string ImageUrl { get; set; }
    public Guid ProductId { get; set; }
    public string ProductFeatureName { get; set; }
    public string IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class ProductFeaturePaginate
  {
    public List<ProductFeatureModel> ProductFeatures { get; set; }
    public int TotalProductFeatures { get; set; }
  }

  public class ProductFeatureGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string ProductFeatureName { get; set; }
    //public Guid ProductId { get; set; }
  }

  public class ProductFeatureCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public string ProductFeatureName { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    public FileContent ProductFeatureFileContent { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }

  public class ProductFeatureUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid ProductFeatureId { get; set; }

    [Required]
    public string ProductFeatureName { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    public FileContent ProductFeatureFileContent { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }

  public class DeleteProductFeatureModel
  {
    public Guid ProductFeatureId { get; set; }
    public bool isDeleteProductFeature { get; set; }
  }

  public class UpdateProductFeatureApprovedModel
  {
    public Guid ProductFeatureId { get; set; }
    public string IsApproved { get; set; }
    public bool isApprovedProductFeature { get; set; }
  }
}
