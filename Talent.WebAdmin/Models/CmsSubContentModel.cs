using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class CmsSubContentModel
  {
    public Guid Cms_SubContentId { get; set; }
    public Guid? BlobId { get; set; }
    public BlobModel Blob { get; set; }
    public string BlobUrl { get; set; }
    public Guid? CmsSubContentVideoBlobId { get; set; }
    public BlobModel VideoBlob { get; set; }
    public string VideoBlobUrl { get; set; }
    public Guid? CmsSubContentFileBlobId { get; set; }
    public BlobModel FileBlob { get; set; }
    public string FileBlobUrl { get; set; }
    public string Cms_SubContentName { get; set; }
    public string Cms_SubContentDescription { get; set; }
    public string Cms_SubContentCategory { get; set; }
    public int? Cms_SubContentSequence { get; set; }
    public bool? Cms_SubContentIsSequence { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class CmsSubContentPaginate
  {
    public List<CmsSubContentModel> CmsSubContents { get; set; }
    public int TotalCmsSubContents { get; set; }
  }

  public class CmsSubContentGridFilter : GridFilter
  {
    public string Cms_SubContentName { get; set; }
    public string Cms_SubContentCategory { get; set; }
  }

  public class CmsSubContentCreateModel
  {
    public FileContent CmsSubContentPicContent { get; set; }

    public FileContent CmsSubContentVidContent { get; set; }

    public FileContent CmsSubContentFileContent { get; set; }

    [Required]
    public string Cms_SubContentName { get; set; }

    [Required]
    public string Cms_SubContentDescription { get; set; }

    [Required]
    public string Cms_SubContentCategory { get; set; }

    public int Cms_SubContentSequence { get; set; }

    public bool Cms_SubContentIsSequence { get; set; }
  }

  public class CmsSubContentUpdateModel
  {
    [Required]
    public Guid Cms_SubContentId { get; set; }

    public FileContent CmsSubContentPicContent { get; set; }
    public bool? isDeletePic { get; set; }

    public FileContent CmsSubContentVidContent { get; set; }
    public bool? isDeleteVid { get; set; }

    public FileContent CmsSubContentFileContent { get; set; }
    public bool? isDeleteFile { get; set; }

    [Required]
    public string Cms_SubContentName { get; set; }

    [Required]
    public string Cms_SubContentDescription { get; set; }

    [Required]
    public string Cms_SubContentCategory { get; set; }

    public int Cms_SubContentSequence { get; set; }

    public bool Cms_SubContentIsSequence { get; set; }
  }

  public class DeleteCmsSubContentModel
  {
    public Guid Cms_SubContentId { get; set; }
    public bool isDeleteCmsSubContent { get; set; }
  }
}
