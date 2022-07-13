using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class CmsFmbModel
  {
    public Guid Cms_FmbId { get; set; }
    public Guid? BlobId { get; set; }
    public BlobModel Blob { get; set; }
    public string BlobUrl { get; set; }
    public Guid? CmsFmbFileBlobId { get; set; }
    public BlobModel FileBlob { get; set; }
    public string FileBlobUrl { get; set; }
    public string Cms_FmbDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class CmsFmbPaginate
  {
    public List<CmsFmbModel> CmsFmbs { get; set; }
    public int TotalCmsFmbs { get; set; }
  }

  public class CmsFmbGridFilter : GridFilter
  {
    public string Cms_FmbDescription { get; set; }
  }

  public class CmsFmbCreateModel
  {
    public FileContent CmsFmbPicContent { get; set; }

    public FileContent CmsFmbFileContent { get; set; }

    [Required]
    public string Cms_FmbDescription { get; set; }
  }

  public class CmsFmbUpdateModel
  {
    [Required]
    public Guid Cms_FmbId { get; set; }

    public FileContent CmsFmbPicContent { get; set; }

    public FileContent CmsFmbFileContent { get; set; }
    public bool? isDeleteFile { get; set; }
    public bool? isDeletePic { get; set; }

    [Required]
    public string Cms_FmbDescription { get; set; }
  }

  public class DeleteCmsFmbModel
  {
    public Guid Cms_FmbId { get; set; }
    public bool isDeleteCmsFmb { get; set; }
  }
}
