using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class CmsSettingModel
  {
    public Guid Cms_SettingId { get; set; }
    public Guid? BlobId { get; set; }
    public BlobModel Blob { get; set; }
    public string BlobUrl { get; set; }
    public Guid? Cms_SettingFileBlobId { get; set; }
    public BlobModel FileBlob { get; set; }
    public string FileBlobUrl { get; set; }
    public string Cms_SettingDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class CmsSettingPaginate
  {
    public List<CmsSettingModel> CmsSettings { get; set; }
    public int TotalCmsSettings { get; set; }
  }

  public class CmsSettingGridFilter : GridFilter
  {
    public string Cms_SettingDescription { get; set; }
  }

  public class CmsSettingCreateModel
  {
    public FileContent CmsSettingPicContent { get; set; }

    public FileContent CmsSettingFileContent { get; set; }

    [Required]
    public string Cms_SettingDescription { get; set; }
  }

  public class CmsSettingUpdateModel
  {
    [Required]
    public Guid Cms_SettingId { get; set; }

    public FileContent CmsSettingPicContent { get; set; }

    public FileContent CmsSettingFileContent { get; set; }
    public bool? isDeleteFile { get; set; }
    public bool? isDeletePic { get; set; }

    [Required]
    public string Cms_SettingDescription { get; set; }
  }

  public class DeleteCmsSettingModel
  {
    public Guid Cms_SettingId { get; set; }
    public bool isDeleteCmsSetting { get; set; }
  }
}
