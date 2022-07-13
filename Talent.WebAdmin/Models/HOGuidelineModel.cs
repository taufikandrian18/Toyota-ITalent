using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class HOGuidelineModel
  {
    public Guid HOGuidelineId { get; set; }
    public Guid BlobId { get; set; }
    public string BlobUrl { get; set; }
    public string BlobFileType { get; set; }
    public string HOGuidelineTitle { get; set; }
    public string HOGuidelineComment { get; set; }
    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string OutletName { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class HOGuidelineUploadModel
  {
    public Guid HOGuidelineId { get; set; }
    public Guid BlobId { get; set; }
    public string BlobUrl { get; set; }
    public string BlobFileType { get; set; }
    public string HOGuidelineTitle { get; set; }
    public string HOGuidelineComment { get; set; }
    public string HOGuidelineStatus { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class HOGuidelinePaginate
  {
    public List<HOGuidelineModel> HOGuidelines { get; set; }
    public int TotalHOGuidelines { get; set; }
  }
  public class HOGuidelineGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string HOGuidelineTitle { get; set; }
  }
  public class HOGuidelineUploadPaginate
  {
    public List<HOGuidelineUploadModel> HOGuidelineUploads { get; set; }
    public int TotalHOGuidelineUploads { get; set; }
  }
  public class HOGuidelineUploadGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string HOGuidelineTitle { get; set; }
  }
  public class HOGuidelineUploadCreateModel
  {
    [Required]
    public string HOGuidelineTitle { get; set; }

    public FileContent HOGuidelineFileContent { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }
  public class HOGuidelineUploadUpdateModel
  {
    [Required]
    public Guid HOGuidelineId { get; set; }
    [Required]
    public string HOGuidelineTitle { get; set; }

    public FileContent HOGuidelineFileContent { get; set; }
  }

  public class HOGuidelineCommentUpdateModel
  {
    [Required]
    public Guid HOGuidelineId { get; set; }

    [Required]
    public string HOGuidelineComment { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }
  public class HOGuidelineStatusApprovedUpdateModel
  {
    [Required]
    public Guid HOGuidelineId { get; set; }

    [Required]
    public bool IsApproved { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }
  public class DeleteHOGuidelineModel
  {
    public Guid HOGuidelineId { get; set; }
    public bool isDeleteHOGuidelines { get; set; }
  }
}
