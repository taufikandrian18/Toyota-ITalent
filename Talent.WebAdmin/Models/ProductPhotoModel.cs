using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class ProductPhotoModel
  {
    public Guid ProductPhotoId { get; set; }
    public Guid BlobId { get; set; }

    public BlobModel Blob { get; set; }
    public string ImageUrl { get; set; }
    public string BlobName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Is360Photos { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class ProductPhotoPaginate
  {
    public List<ProductPhotoModel> ProductPhotos { get; set; }
    public int TotalProductPhotos { get; set; }
  }


  public class ProductPhotoGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string BlobName { get; set; }
    public Guid ProductId { get; set; }
  }

  public class ProductPhotoCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid ProductId { get; set; }

    public FileContent ProductFileContent { get; set; }

    public bool Is360Photo { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }

  public class ProductPhotoUpdateModel
  {
    [Required]
    public Guid ProductId { get; set; }
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid ProductPhotoId { get; set; }


    public FileContent ProductFileContent { get; set; }

    public bool Is360Photo { get; set; }

    public DateTime? ApprovedAt { get; set; }

  }
  public class DeleteProductPhotoModel
  {
    public Guid ProductPhotoId { get; set; }
    public bool isDeleteProduct { get; set; }
  }
}
