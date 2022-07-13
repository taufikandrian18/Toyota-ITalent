using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class ProductFAQModel
  {
    public Guid ProductFaqId { get; set; }
    public Guid ProductFaqCategoryId { get; set; }
    public string ProductFaqCategoryName { get; set; }
    public Guid ProductId { get; set; }
    public Guid BlobId { get; set; }
    public BlobModel Blob { get; set; }
    public string ImageUrl { get; set; }
    public int ProductFaqSequence { get; set; }
    public string ProductFaqTitle { get; set; }
    public string ProductFaqDescription { get; set; }
    public string OutletName { get; set; }
    public string ContributorName { get; set; }
    public string PositionName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class ProductFAQPaginate
  {
    public List<ProductFAQModel> ProductFAQs { get; set; }
    public int TotalProductFAQs { get; set; }
  }

  public class ProductFAQGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public Guid ProductId { get; set; }
    public string ProductFaqTitle { get; set; }
  }

  public class ProductFAQCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductFaqCategoryId { get; set; }

    public FileContent ProductGalleryFileContent { get; set; }

    [Required]
    public int ProductFaqSequence { get; set; }

    [Required]
    public string ProductFaqTitle { get; set; }

    [Required]
    public string ProductFaqDescription { get; set; }
    public DateTime? ApprovedAt { get; set; }

  }

  public class ProductFAQUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid ProductFaqId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductFaqCategoryId { get; set; }

    public FileContent ProductGalleryFileContent { get; set; }

    [Required]
    public int ProductFaqSequence { get; set; }

    [Required]
    public string ProductFaqTitle { get; set; }

    [Required]
    public string ProductFaqDescription { get; set; }
    public DateTime? ApprovedAt { get; set; }
  }

  public class DeleteProductFAQModel
  {
    public Guid ProductFaqId { get; set; }
    public bool isDeleteProductFAQ { get; set; }
  }

  public class ProductFAQUserModel
  {
    public Guid ProductFAQUserId { get; set; }
    public Guid ProductId { get; set; }
    public Guid ProductFaqCategoryId { get; set; }
    public string ProductFaqCategoryName { get; set; }
    public string ProductFAQUserQuestion { get; set; }
    public string OutletName { get; set; }
    public string ContributorName { get; set; }
    public string PositionName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class ProductFAQUserPaginate
  {
    public List<ProductFAQUserModel> ProductFAQUsers { get; set; }
    public int TotalProductFAQUsers { get; set; }
  }

  public class ProductFAQUserGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public Guid ProductId { get; set; }
  }

  public class ProductFAQUserCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductFaqCategoryId { get; set; }

    [Required]
    public string ProductFAQUserQuestion { get; set; }

  }

  public class ProductFAQUserUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid ProductFAQUserId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductFaqCategoryId { get; set; }

    [Required]
    public string ProductFAQUserQuestion { get; set; }
  }

  public class DeleteProductFAQUserModel
  {
    public Guid ProductFAQUserId { get; set; }
    public bool isDeleteProductFAQUser { get; set; }
  }

  public class ProductFAQCategoryModel
  {
    public Guid ProductFaqCategoryId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductFaqCategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class ProductFAQCategoryPaginate
  {
    public List<ProductFAQCategoryModel> ProductFAQCategories { get; set; }
    public int TotalProductFAQCategories { get; set; }
  }

  public class ProductFAQCategoryGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string ProductFaqCategoryName { get; set; }
    public Guid ProductId { get; set; }
  }

  public class ProductFAQCategoryCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public string ProductFaqCategoryName { get; set; }

    [Required]
    public Guid ProductId { get; set; }

  }

  public class ProductFAQCategoryUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid ProductFaqCategoryId { get; set; }

    [Required]
    public string ProductFaqCategoryName { get; set; }

    [Required]
    public Guid ProductId { get; set; }
  }

  public class DeleteProductFAQCategoryModel
  {
    public Guid ProductFaqCategoryId { get; set; }
    public bool isDeleteProductFAQCategory { get; set; }
  }
}
