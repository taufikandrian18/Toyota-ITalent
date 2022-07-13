using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class ServiceTipTypeModel
  {
    public Guid ServiceTipTypeId { get; set; }
    public Guid? BlobId { get; set; }
    public string ImageUrl { get; set; }
    public string FileTypeBlob { get; set; }
    public string ServiceTipTypeName { get; set; }
    public string ServiceTipTypeDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }
  public class ServiceTipTypePaginate
  {
    public List<ServiceTipTypeModel> ServiceTipTypes { get; set; }
    public int TotalServiceTipTypes { get; set; }
  }
  public class ServiceTipTypeGridFilter : GridFilter
  {
    public string ServiceTipTypeName { get; set; }
  }
  public class ServiceTipTypeCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public string ServiceTipTypeName { get; set; }

    public FileContent ProductFileContent { get; set; }

    public string ServiceTipTypeDescription { get; set; }

  }
  public class ServiceTipTypeUpdateModel
  {
    [Required]
    public Guid ServiceTipTypeId { get; set; }
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public string ServiceTipTypeName { get; set; }

    public FileContent ProductFileContent { get; set; }

    public string ServiceTipTypeDescription { get; set; }

  }
  public class DeleteServiceTipTypeModel
  {
    public Guid ServiceTipTypeId { get; set; }
    public bool isDeleteServiceTipType { get; set; }
  }
}
