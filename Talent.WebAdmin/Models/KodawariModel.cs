using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
  public class KodawariModel
  {
    public Guid KodawariId { get; set; }
    public Guid KodawariMenuId { get; set; }
    public string KodawariMenuName { get; set; }
    public Guid Cms_MenuId { get; set; }
    public string Cms_MenuName { get; set; }
    public Guid KodawariTypeId { get; set; }
    public string KodawariTypeName { get; set; }
    public Guid Cms_ContentId { get; set; }
    public string Cms_ContentName { get; set; }
    public string Cms_ContentDescription { get; set; }
    public Guid Cms_ContentBlobId { get; set; }
    public string Cms_ContentBlobImageUrl { get; set; }
    public Guid Cms_ContentVideoBlobId { get; set; }
    public string Cms_ContentVideoBlobImageUrl { get; set; }
    public Guid Cms_ContentFileBlobId { get; set; }
    public string Cms_ContentFileBlobImageUrl { get; set; }
    public Guid? Cms_SubContentId { get; set; }
    public string Cms_SubContentName { get; set; }
    public string Cms_SubContentDescription { get; set; }
    public Guid? Cms_SubContentBlobId { get; set; }
    public string Cms_SubContentBlobImageUrl { get; set; }
    public Guid? Cms_SubContentVideoBlobId { get; set; }
    public string Cms_SubContentVideoBlobImageUrl { get; set; }
    public Guid? Cms_SubContentFileBlobId { get; set; }
    public string Cms_SubContentFileBlobImageUrl { get; set; }
    public int KodawariSequence { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class KodawariPaginate
  {
    public List<KodawariModel> Kodawaris { get; set; }
    public int TotalKodawaris { get; set; }
  }

  public class KodawariGridFilter : GridFilter
  {
    public string KodawariMenuName { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
  }

  public class KodawariCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid KodawariTypeId { get; set; }

    [Required]
    public Guid KodawariMenuId { get; set; }

    [Required]
    public Guid Cms_MenuId { get; set; }

    [Required]
    public int KodawariSequence { get; set; }

    [Required]
    public Guid Cms_ContentId { get; set; }

    public Guid? Cms_SubContentId { get; set; }

    public DateTime? ApprovedAt { get; set; }

  }

  public class KodawariUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid KodawariId { get; set; }

    [Required]
    public Guid KodawariTypeId { get; set; }

    [Required]
    public Guid KodawariMenuId { get; set; }

    [Required]
    public Guid Cms_MenuId { get; set; }

    [Required]
    public int KodawariSequence { get; set; }

    [Required]
    public Guid Cms_ContentId { get; set; }

    public Guid? Cms_SubContentId { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }
  public class DeleteKodawariModel
  {
    public Guid KodawariId { get; set; }
    public bool isDeleteKodawari { get; set; }
  }
}
