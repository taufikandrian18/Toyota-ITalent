using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class CmsContentModel
  {
    public Guid Cms_ContentId { get; set; }
    public Guid? BlobId { get; set; }
    public BlobModel Blob { get; set; }
    public string BlobUrl { get; set; }
    public Guid? CmsContentVideoBlobId { get; set; }
    public BlobModel VideoBlob { get; set; }
    public string VideoBlobUrl { get; set; }
    public Guid? CmsContentFileBlobId { get; set; }
    public Guid? ProductId { get; set; }
    public BlobModel FileBlob { get; set; }
    public string FileBlobUrl { get; set; }
    public string Cms_ContentName { get; set; }
    public string Cms_ContentDescription { get; set; }
    public string Cms_ContentCategory { get; set; }
    public int? Cms_ContentSequence { get; set; }
    public bool? Cms_ContentIsSequence { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class CmsContentPaginate
  {
    public List<CmsContentModel> CmsContents { get; set; }
    public int TotalCmsContents { get; set; }
  }

  public class CmsContentGridFilter : GridFilter
  {
    public string Cms_ContentName { get; set; }
    public string Cms_ContentCategory { get; set; }
    public Guid? ProductId { get; set; }
  }

  public class CmsContentCreateModel
  {
    public FileContent CmsContentPicContent { get; set; }

    public FileContent CmsContentVidContent { get; set; }

    public FileContent CmsContentFileContent { get; set; }

    [Required]
    public string Cms_ContentName { get; set; }

    [Required]
    public string Cms_ContentDescription { get; set; }

    [Required]
    public string Cms_ContentCategory { get; set; }

    public int Cms_ContentSequence { get; set; }

    public bool Cms_ContentIsSequence { get; set; }

    public Guid? ProductId { get; set; }
  }

  public class CmsContentUpdateModel
  {
    [Required]
    public Guid Cms_ContentId { get; set; }

    public FileContent CmsContentPicContent { get; set; }
    public bool? isDeletePic { get; set; }

    public FileContent CmsContentVidContent { get; set; }
    public bool? isDeleteVid { get; set; }

    public FileContent CmsContentFileContent { get; set; }
    public bool? isDeleteFile { get; set; }

    [Required]
    public string Cms_ContentName { get; set; }

    [Required]
    public string Cms_ContentDescription { get; set; }

    [Required]
    public string Cms_ContentCategory { get; set; }

    public int Cms_ContentSequence { get; set; }

    public bool Cms_ContentIsSequence { get; set; }

    public Guid? ProductId { get; set; }
  }

  public class DeleteCmsContentModel
  {
    public Guid Cms_ContentId { get; set; }
    public bool isDeleteCmsContent { get; set; }
  }
}
