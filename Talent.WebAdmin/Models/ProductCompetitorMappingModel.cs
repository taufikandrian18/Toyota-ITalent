using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
  public class ProductCompetitorMappingModel
  {
    public Guid ProductCompetitorMappingId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public Guid ProductTypeId { get; set; }
    public string ProductTypeName { get; set; }
    public Guid ProductCompetitorId { get; set; }
    public string ProductCompetitorName { get; set; }
    public Guid ProductCompetitorTypeId { get; set; }
    public string ProductTypeCompetitorName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class ProductCompetitorMappingPaginate
  {
    public List<ProductCompetitorMappingModel> ProductCompetitorMappings { get; set; }
    public int TotalProductCompetitorMapping { get; set; }
  }

  public class ProductCompetitorMappingGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string ProductCompetitorName { get; set; }
    public Guid ProductId { get; set; }
  }

  public class ProductCompetitorMappingCreateUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductTypeId { get; set; }

    [Required]
    public Guid ProductCompetitorId { get; set; }

    [Required]
    public Guid ProductCompetitorTypeId { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }

  public class ProductCompetitorMappingUpdateModel
  {
    [Required]
    public Guid ProductCompetitorMappingId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductTypeId { get; set; }

    [Required]
    public Guid ProductCompetitorId { get; set; }

    [Required]
    public Guid ProductCompetitorTypeId { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }

  public class DeleteProductCompetitorModel
  {
    public Guid ProductCompetitorMappingId { get; set; }

    public bool isDeleteProductCompetitor { get; set; }
  }

}
