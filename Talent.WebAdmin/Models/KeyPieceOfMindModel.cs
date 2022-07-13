using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
  public class KeyPieceOfMindModel
  {
    public Guid KeyPieceOfMindId { get; set; }
    public Guid KeyPieceOfMindTypeId { get; set; }
    public string KeyPieceOfMindTypeName { get; set; }
    public Guid KeyPieceOfMindMenuId { get; set; }
    public string KeyPieceOfMindMenuName { get; set; }
    public Guid Cms_MenuId { get; set; }
    public string Cms_MenuName { get; set; }
    public Guid Cms_ContentId { get; set; }
    public string Cms_ContentName { get; set; }
    public Guid Cms_ContentBlobId { get; set; }
    public string Cms_ContentBlobImageUrl { get; set; }
    public Guid Cms_ContentVideoBlobId { get; set; }
    public string Cms_ContentVideoBlobImageUrl { get; set; }
    public Guid Cms_ContentFileBlobId { get; set; }
    public string Cms_ContentFileBlobImageUrl { get; set; }
    public string Cms_ContentDescription { get; set; }
    public Guid? Cms_SubContentId { get; set; }
    public string Cms_SubContentName { get; set; }
    public Guid? Cms_SubContentBlobId { get; set; }
    public string Cms_SubContentBlobImageUrl { get; set; }
    public Guid? Cms_SubContentVideoBlobId { get; set; }
    public string Cms_SubContentVideoBlobImageUrl { get; set; }
    public Guid? Cms_SubContentFileBlobId { get; set; }
    public string Cms_SubContentFileBlobImageUrl { get; set; }
    public string Cms_SubContentDescription { get; set; }
    public int IsSequence { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class KeyPieceOfMindPaginate
  {
    public List<KeyPieceOfMindModel> KeyPieceOfMinds { get; set; }
    public int TotalKeyPieceOfMinds { get; set; }
  }

  public class KeyPieceOfMindGridFilter : GridFilter
  {
    public string KeyPieceOfMindMenuName { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
  }

  public class KeyPieceOfMindCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid KeyPieceOfMindTypeId { get; set; }

    [Required]
    public Guid KeyPieceOfMindMenuId { get; set; }

    [Required]
    public Guid Cms_MenuId { get; set; }

    [Required]
    public int IsSequence { get; set; }

    [Required]
    public Guid Cms_ContentId { get; set; }

    public Guid? Cms_SubContentId { get; set; }

    public DateTime? ApprovedAt { get; set; }

  }

  public class KeyPieceOfMindUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    ///

    [Required]
    public Guid KeyPieceOfMindId { get; set; }

    [Required]
    public Guid KeyPieceOfMindTypeId { get; set; }

    [Required]
    public Guid KeyPieceOfMindMenuId { get; set; }

    [Required]
    public Guid Cms_MenuId { get; set; }

    [Required]
    public int IsSequence { get; set; }

    [Required]
    public Guid Cms_ContentId { get; set; }

    public Guid? Cms_SubContentId { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }
  public class DeleteKeyPieceOfMindModel
  {
    public Guid KeyPieceOfMindId { get; set; }
    public bool isDeleteKeyPieceOfMind { get; set; }
  }
}
