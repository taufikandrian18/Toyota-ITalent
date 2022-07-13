using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class CmsWorkPrincipalModel
  {
    public Guid Cms_WorkPrincipalId { get; set; }
    public Guid? BlobId { get; set; }
    public BlobModel Blob { get; set; }
    public string BlobUrl { get; set; }
    public Guid? Cms_WorkPrincipalFileBlobId { get; set; }
    public BlobModel FileBlob { get; set; }
    public string FileBlobUrl { get; set; }
    public string Cms_WorkPrincipalDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class CmsWorkPrincipalPaginate
  {
    public List<CmsWorkPrincipalModel> CmsWorkPrincipals { get; set; }
    public int TotalCmsWorkPrincipals { get; set; }
  }

  public class CmsWorkPrincipalGridFilter : GridFilter
  {
    public string Cms_WorkPrincipalDescription { get; set; }
  }

  public class CmsWorkPrincipalCreateModel
  {
    public FileContent CmsWorkPrincipalPicContent { get; set; }

    public FileContent CmsWorkPrincipalFileContent { get; set; }

    [Required]
    public string Cms_WorkPrincipalDescription { get; set; }
  }

  public class CmsWorkPrincipalUpdateModel
  {
    [Required]
    public Guid Cms_WorkPrincipalId { get; set; }

    public FileContent CmsWorkPrincipalPicContent { get; set; }

    public FileContent CmsWorkPrincipalFileContent { get; set; }
    public bool? isDeleteFile { get; set; }
    public bool? isDeletePic { get; set; }

    [Required]
    public string Cms_WorkPrincipalDescription { get; set; }
  }

  public class DeleteCmsWorkPrincipalModel
  {
    public Guid Cms_WorkPrincipalId { get; set; }
    public bool isDeleteCmsWorkPrincipal { get; set; }
  }
}
