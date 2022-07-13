using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class KodawariBannerModel
  {
    public Guid KodawariBannerId { get; set; }
    public Guid BlobId { get; set; }
    public string ImageUrl { get; set; }
    public string FileTypeBlob { get; set; }
    public string KodawariBannerTitle { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class KodawariBannerPaginate
  {
    public List<KodawariBannerModel> KodawariBanners { get; set; }
    public int TotalKodawariBanners { get; set; }
  }

  public class KodawariBannersGridFilter : GridFilter
  {
    public string KodawariBannerTitle { get; set; }
  }

  public class KodawariBannerCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public string KodawariBannerTitle { get; set; }

    public FileContent KodawariBannerFileContent { get; set; }

    public DateTime? ApprovedAt { get; set; }

  }
  public class KodawariBannerUpdateModel
  {
    [Required]
    public Guid KodawariBannerId { get; set; }
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public string KodawariBannerTitle { get; set; }

    public FileContent KodawariBannerFileContent { get; set; }

    public DateTime? ApprovedAt { get; set; }

  }
  public class DeleteKodawariBannerModel
  {
    public Guid KodawariBannerId { get; set; }
    public bool isDeleteKodawariBanners { get; set; }
  }
}
