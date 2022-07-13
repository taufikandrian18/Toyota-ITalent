using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class KodawariTypeModel
  {
    public Guid KodawariTypeId { get; set; }
    public Guid? BlobId { get; set; }
    public string ImageUrl { get; set; }
    public string FileTypeBlob { get; set; }
    public string KodawariTypeName { get; set; }
    public string KodawariTypeDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class KodawariTypePaginate
  {
    public List<KodawariTypeModel> KodawariTypes { get; set; }
    public int TotalKodawariTypes { get; set; }
  }
  public class KodawariTypeGridFilter : GridFilter
  {
    public string KodawariTypeName { get; set; }
  }
  public class KodawariTypeCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public string KodawariTypeName { get; set; }

    public FileContent KodawariTypeFileContent { get; set; }

    public string KodawariTypeDescription { get; set; }

  }
  public class KodawariTypeUpdateModel
  {
    [Required]
    public Guid KodawariTypeId { get; set; }
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public string KodawariTypeName { get; set; }

    public FileContent KodawariTypeFileContent { get; set; }

    public string KodawariTypeDescription { get; set; }

  }
  public class DeleteKodawariTypeModel
  {
    public Guid KodawariTypeId { get; set; }
    public bool isDeleteKodawariType { get; set; }
  }
}
