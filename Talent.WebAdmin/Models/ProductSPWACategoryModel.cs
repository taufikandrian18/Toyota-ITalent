using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class ProductSPWACategoryModel
  {
    public Guid ProductSPWACategoryId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? BlobId { get; set; }
    public BlobModel Blob { get; set; }
    public string ImageUrl { get; set; }
    public string ProductSPWACategoryName { get; set; }
    public string ProductSPWACategoryDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class ProductSPWACategoryPaginate
  {
    public List<ProductSPWACategoryModel> ProductSPWACategories { get; set; }
    public int TotalProductSPWACategories { get; set; }
  }
  public class ProductSPWACategoryGridFilter : GridFilter
  {
    public string ProductSPWACategoryName { get; set; }
    public Guid ProductId { get; set; }
  }
  public class ProductSPWACategoryCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public string ProductSPWACategoryName { get; set; }

    public FileContent ProductFileContent { get; set; }

    public string ProductSPWACategoryDescription { get; set; }

    [Required]
    public Guid ProductId { get; set; }

  }
  public class ProductSPWACategoryUpdateModel
  {
    [Required]
    public Guid ProductSPWACategoryId { get; set; }
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public string ProductSPWACategoryName { get; set; }

    public FileContent ProductFileContent { get; set; }

    public string ProductSPWACategoryDescription { get; set; }

    [Required]
    public Guid ProductId { get; set; }

  }
  public class DeleteProductSPWACategoryModel
  {
    public Guid ProductSPWACategoryId { get; set; }
    public bool isDeleteProductSPWACategory { get; set; }
  }
}
