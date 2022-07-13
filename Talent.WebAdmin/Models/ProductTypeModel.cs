using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class ProductTypeModel
  {
    public Guid ProductTypeId { get; set; }
    public Guid ProductId { get; set; }
    public Guid BlobId { get; set; }
    public string ImageUrl { get; set; }
    public string ProductTypeName { get; set; }
    public string ProductTypeSalesTalk { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class ProductTypePaginate
  {
    public List<ProductTypeModel> ProductTypes { get; set; }
    public int TotalProductTypes { get; set; }
  }
  public class ProductTypeGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string ProductTypeName { get; set; }
    public Guid ProductId { get; set; }
  }
  public class ProductTypeDropdownView
  {
    public Guid ProductTypeId { get; set; }
    public string ProductTypeName { get; set; }
  }
  public class ProductTypeCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public string ProductTypeName { get; set; }

    [Required]
    public string ProductTypeSalesTalk { get; set; }

    public FileContent ProductTypeFileContent { get; set; }

    public DateTime? ApprovedAt { get; set; }

  }
  public class ProductTypeUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid ProductTypeId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public string ProductTypeName { get; set; }

    [Required]
    public string ProductTypeSalesTalk { get; set; }

    public FileContent ProductTypeFileContent { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }
  public class DeleteProductTypeModel
  {
    public Guid ProductTypeId { get; set; }
    public bool isDeleteProductType { get; set; }
  }
}
