using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class DictionaryModel
  {
    public Guid DictionaryId { get; set; }
    public Guid BlobId { get; set; }
    public BlobModel Blob { get; set; }
    public string ImageUrl { get; set; }
    public string DictionaryName { get; set; }
    public bool DictionaryStatus { get; set; }
    public bool IsFavorite { get; set; }
    public string DictionaryArti { get; set; }
    public string DictionaryManfaat { get; set; }
    public string DictionaryFakta { get; set; }
    public string DictionaryBasicOperation { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class DictionaryPaginate
  {
    public List<DictionaryModel> DIctionaries { get; set; }
    public int TotalDictionaries { get; set; }
  }


  public class DictionaryGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string DictionaryName { get; set; }
    public bool? ApprovedAt { get; set; }
  }

  public class DictionaryCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public string DictionaryName { get; set; }

    [Required]
    public string DictionaryArti { get; set; }

    [Required]
    public string DictionaryManfaat { get; set; }

    [Required]
    public string DictionaryFakta { get; set; }

    [Required]
    public bool DictionaryStatus { get; set; }

    public FileContent ProductFileContent { get; set; }

    public string DictionaryBasicOperation { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }

  public class DictionaryUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid DictionaryId { get; set; }

    [Required]
    public string DictionaryName { get; set; }

    [Required]
    public string DictionaryArti { get; set; }

    [Required]
    public string DictionaryManfaat { get; set; }

    [Required]
    public string DictionaryFakta { get; set; }

    [Required]
    public bool DictionaryStatus { get; set; }

    public FileContent ProductFileContent { get; set; }

    public string DictionaryBasicOperation { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }

  public class DeleteDictionaryModel
  {
    public Guid DictionaryId { get; set; }
    public bool isDeleteDictionary { get; set; }
  }
}
