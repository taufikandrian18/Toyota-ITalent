using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class ProductModel
  {
    public Guid ProductId { get; set; }
    public Guid BlobId { get; set; }
    public Guid ProductCategoryId { get; set; }
    public string ProductName { get; set; }
    public string ProductCategoryName { get; set; }
    public string ImageUrl { get; set; }
    public int ProductSegment { get; set; }
    public bool? IsCompetitor { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class ProductPaginate
  {
    public List<ProductModel> Products { get; set; }
    public int TotalProducts { get; set; }
  }


  public class ProductGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string ProductCategoryName { get; set; }
    public string ProductName { get; set; }
  }

  public class ProductIsCompetitorDropdownView
  {
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
  }

  public class ProductCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public string ProductName { get; set; }

    [Required]
    public Guid ProductCategoryId { get; set; }

    [Required]
    public int ProductSegment { get; set; }

    public FileContent ProductFileContent { get; set; }

    [Required]
    public bool IsCompetitor { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }

  public class ProductUpdateModel
  {
    [Required]
    public Guid ProductId { get; set; }
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public string ProductName { get; set; }

    [Required]
    public Guid ProductCategoryId { get; set; }

    [Required]
    public int ProductSegment { get; set; }

    public FileContent ProductFileContent { get; set; }

    [Required]
    public bool IsCompetitor { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }
  public class DeleteProductModel
  {
    public Guid ProductId { get; set; }
    public bool isDeleteProduct { get; set; }
  }
}
