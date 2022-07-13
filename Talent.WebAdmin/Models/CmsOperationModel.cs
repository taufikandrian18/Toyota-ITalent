using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class CmsOperationModel
  {
    public Guid Cms_OperationId { get; set; }
    public Guid? BlobId { get; set; }
    public BlobModel Blob { get; set; }
    public string BlobUrl { get; set; }
    public Guid? Cms_OperationFileBlobId { get; set; }
    public BlobModel FileBlob { get; set; }
    public string FileBlobUrl { get; set; }
    public string Cms_OperationDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class CmsOperationPaginate
  {
    public List<CmsOperationModel> CmsOperations { get; set; }
    public int TotalCmsOperations { get; set; }
  }

  public class CmsOperationGridFilter : GridFilter
  {
    public string Cms_OperationDescription { get; set; }
  }

  public class CmsOperationCreateModel
  {
    public FileContent CmsOperationPicContent { get; set; }

    public FileContent CmsOperationFileContent { get; set; }

    [Required]
    public string Cms_OperationDescription { get; set; }
  }

  public class CmsOperationUpdateModel
  {
    [Required]
    public Guid Cms_OperationId { get; set; }

    public FileContent CmsOperationPicContent { get; set; }

    public FileContent CmsOperationFileContent { get; set; }
    public bool? isDeleteFile { get; set; }
    public bool? isDeletePic { get; set; }

    [Required]
    public string Cms_OperationDescription { get; set; }
  }

  public class DeleteCmsOperationModel
  {
    public Guid Cms_OperationId { get; set; }
    public bool isDeleteCmsOperation { get; set; }
  }
}
