using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
  public class ServiceTipModel
  {
    public Guid ServiceTipId { get; set; }
    public Guid ServiceTipTypeId { get; set; }
    public string ServiceTipTypeName { get; set; }
    public Guid ServiceTipMenuId { get; set; }
    public string ServiceTipMenuName { get; set; }
    public Guid Cms_MenuId { get; set; }
    public string Cms_MenuName { get; set; }
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
    public int IsSequence { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class ServiceTipPaginate
  {
    public List<ServiceTipModel> ServiceTips { get; set; }
    public int TotalServiceTips { get; set; }
  }

  public class ServiceTipGridFilter : GridFilter
  {
    public string ServiceTipMenuName { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
  }

  public class ServiceTipCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    public Guid ServiceTipTypeId { get; set; }

    [Required]
    public Guid ServiceTipMenuId { get; set; }

    [Required]
    public Guid Cms_MenuId { get; set; }

    [Required]
    public int IsSequence { get; set; }

    [Required]
    public Guid Cms_ContentId { get; set; }

    public Guid? Cms_SubContentId { get; set; }

    public DateTime? ApprovedAt { get; set; }

  }

  public class ServiceTipUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid ServiceTipId { get; set; }

    public Guid ServiceTipTypeId { get; set; }

    [Required]
    public Guid ServiceTipMenuId { get; set; }

    [Required]
    public Guid Cms_MenuId { get; set; }

    [Required]
    public int IsSequence { get; set; }

    [Required]
    public Guid Cms_ContentId { get; set; }

    public Guid? Cms_SubContentId { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }
  public class DeleteServiceTipModel
  {
    public Guid ServiceTipId { get; set; }
    public bool isDeleteServiceTip { get; set; }
  }
}
