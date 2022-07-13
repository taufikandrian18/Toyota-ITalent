using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class KodawariDownloadModel
  {
    public Guid KodawariDownloadId { get; set; }
    public Guid KodawariMenuId { get; set; }
    public string KodawariMenuName { get; set; }
    public Guid BlobId { get; set; }
    public string ImageUrl { get; set; }
    public string FileTypeBlob { get; set; }
    public string KodawariDownloadTitle { get; set; }
    public bool IsDownloadable { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class KodawariDownloadPaginate
  {
    public List<KodawariDownloadModel> KodawariDownloads { get; set; }
    public int TotalKodawariDownloads { get; set; }
  }
  public class KodawariDownloadGridFilter : GridFilter
  {
    public string KodawariDownloadTitle { get; set; }
  }
  public class KodawariDownloadCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid KodawariMenuId { get; set; }

    [Required]
    public string KodawariDownloadTitle { get; set; }

    public FileContent KodawariDownloadFileContent { get; set; }

    public bool IsDownloadable { get; set; }

    public DateTime? ApprovedAt { get; set; }

  }
  public class KodawariDownloadUpdateModel
  {
    [Required]
    public Guid KodawariDownloadId { get; set; }
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public Guid KodawariMenuId { get; set; }

    [Required]
    public string KodawariDownloadTitle { get; set; }

    public FileContent KodawariDownloadFileContent { get; set; }

    public bool IsDownloadable { get; set; }

    public DateTime? ApprovedAt { get; set; }

  }
  public class DeleteKodawariDownloadModel
  {
    public Guid KodawariDownloadId { get; set; }
    public bool isDeleteKodawariDownload { get; set; }
  }
  public class UpdateKodawariDownloadStatusDownloadModel
  {
    public Guid KodawariDownloadId { get; set; }
    public bool isThisFileDownloadAble { get; set; }
  }
}
