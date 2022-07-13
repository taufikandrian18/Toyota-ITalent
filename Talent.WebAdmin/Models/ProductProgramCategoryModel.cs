using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class ProductProgramCategoryModel
  {
    public Guid ProductProgramCategoryId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? BlobId { get; set; }
    public BlobModel Blob { get; set; }
    public string ImageUrl { get; set; }
    public string ProductProgramCategoryName { get; set; }
    public string ProductProgramCategoryDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class ProductProgramCategoryPaginate
  {
    public List<ProductProgramCategoryModel> ProductProgramCategories { get; set; }
    public int TotalProductProgramCategories { get; set; }
  }

  public class ProductProgramCategoryGridFilter : GridFilter
  {
    public string ProductProgramCategoryName { get; set; }
    public Guid ProductId { get; set; }
  }

  public class ProductProgramCategoryCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public string ProductProgramCategoryName { get; set; }

    public FileContent ProductFileContent { get; set; }

    public string ProductProgramCategoryDescription { get; set; }

    [Required]
    public Guid ProductId { get; set; }

  }
  public class ProductProgramCategoryUpdateModel
  {
    [Required]
    public Guid ProductProgramCategoryId { get; set; }
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public string ProductProgramCategoryName { get; set; }

    public FileContent ProductFileContent { get; set; }

    public string ProductProgramCategoryDescription { get; set; }

    [Required]
    public Guid ProductId { get; set; }

  }
  public class DeleteProductProgramCategoryModel
  {
    public Guid ProductProgramCategoryId { get; set; }
    public bool isDeleteProductProgramCategory { get; set; }
  }
}
