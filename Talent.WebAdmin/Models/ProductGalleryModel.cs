using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class ProductGalleryModel
  {
    public Guid ProductGalleryId { get; set; }
    public Guid ProductId { get; set; }
    public Guid BlobId { get; set; }
    public string ContributorName { get; set; }
    public string OutletName { get; set; }
    public string PositionName { get; set; }
    public string ImageUrl { get; set; }
    public Guid ProductTypeId { get; set; }
    public string ProductTypeName { get; set; }
    public string ProductGalleryColorCode { get; set; }
    public string ProductGalleryColorName { get; set; }
    public bool IsColor { get; set; }
    public string IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class ProductGalleryPaginate
  {
    public List<ProductGalleryModel> ProductGalleries { get; set; }
    public int TotalProductGalleries { get; set; }
  }

  public class ProductGalleryGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string ProductGalleryColorName { get; set; }
    public Guid ProductId { get; set; }
  }

  public class ProductGalleryCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductTypeId { get; set; }

    public string ProductGalleryColorCode { get; set; }

    public string ProductGalleryColorName { get; set; }

    public FileContent ProductGalleryFileContent { get; set; }

    [Required]
    public bool IsColor { get; set; }
    public DateTime? ApprovedAt { get; set; }
  }
  public class ProductGalleryUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid ProductGalleryId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductTypeId { get; set; }

    public string ProductGalleryColorCode { get; set; }

    public string ProductGalleryColorName { get; set; }

    public FileContent ProductGalleryFileContent { get; set; }

    [Required]
    public bool IsColor { get; set; }
    public DateTime? ApprovedAt { get; set; }
  }
  public class UpdateProductGalleryApproval
  {
    public Guid ProductGalleryId { get; set; }
    public string IsApproved { get; set; }
    public bool isUpdateProductGalleryApproval { get; set; }
  }
  public class DeleteProductGalleryModel
  {
    public Guid ProductGalleryId { get; set; }
    public bool isDeleteProductGallery { get; set; }
  }
}
