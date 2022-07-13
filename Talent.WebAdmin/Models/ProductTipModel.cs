using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class ProductTipModel
  {
    public Guid ProductTipId { get; set; }
    public Guid ProductId { get; set; }
    public Guid BlobId { get; set; }
    public BlobModel Blob { get; set; }
    public string ImageUrl { get; set; }
    public string OutletName { get; set; }
    public string ContributorName { get; set; }
    public string PositionName { get; set; }
    public Guid ProductTipCategoryId { get; set; }
    public string ProductTipCategoryName { get; set; }
    public string ProductTipTitle { get; set; }
    public string ProductTipDescription { get; set; }
    public int IsSequence { get; set; }
    public string IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class ProductTipPaginate
  {
    public List<ProductTipModel> ProductTips { get; set; }
    public int TotalProductTips { get; set; }
  }

  public class ProductTipGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public Guid ProductId { get; set; }
    public string ProductTipTitle { get; set; }
  }

  public class ProductTipCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductTipCategoryId { get; set; }

    public FileContent ProductGalleryFileContent { get; set; }

    [Required]
    public int IsSequence { get; set; }

    [Required]
    public string ProductTipTitle { get; set; }

    [Required]
    public string ProductTipDescription { get; set; }

    public DateTime? ApprovedAt { get; set; }

  }

  public class ProductTipUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid ProductTipId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductTipCategoryId { get; set; }

    public FileContent ProductGalleryFileContent { get; set; }

    [Required]
    public int IsSequence { get; set; }

    [Required]
    public string ProductTipTitle { get; set; }

    [Required]
    public string ProductTipDescription { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }

  public class DeleteProductTipModel
  {
    public Guid ProductTipId { get; set; }
    public bool isDeleteProductTip { get; set; }
  }

  public class UpdateProductTipApproval
  {
    public Guid ProductTipId { get; set; }
    public string IsApproved { get; set; }
    public bool isUpdateProductTipApproval { get; set; }
  }

  public class ProductTipCategoryModel
  {
    public Guid ProductTipCategoryId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductTipCategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class ProductTipCategoryPaginate
  {
    public List<ProductTipCategoryModel> ProductTipCategories { get; set; }
    public int TotalProductTipCategories { get; set; }
  }

  public class ProductTipCategoryGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string ProductTipCategoryName { get; set; }
    public Guid ProductId { get; set; }
  }

  public class ProductTipCategoryCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public string ProductTipCategoryName { get; set; }

    [Required]
    public Guid ProductId { get; set; }

  }

  public class ProductTipCategoryUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid ProductTipCategoryId { get; set; }

    [Required]
    public string ProductTipCategoryName { get; set; }

    [Required]
    public Guid ProductId { get; set; }
  }

  public class DeleteProductTipCategoryModel
  {
    public Guid ProductTipCategoryId { get; set; }
    public bool isDeleteProductTipCategory { get; set; }
  }
}
