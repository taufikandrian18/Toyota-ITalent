using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Talent.Entities.Entities;

namespace Talent.WebAdmin.Models
{
  public class AreaSpecialistModel
  {
    public string AreaSpecialistId { get; set; }
    public string AreaSpecialistName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public List<Outlets> Outlets { get; set; }
  }

  public class AreaSpecialistPaginate
  {
    public List<AreaSpecialistModel> AreaSpecialists { get; set; }
    public int TotalAreaSpecialists { get; set; }
  }

  public class AreaSpecialistGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string AreaSpecialistName { get; set; }
  }

  public class AreaSpecialistCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public string AreaSpecialistName { get; set; }

    [Required]
    public List<string> OutletId { get; set; }

  }

  public class AreaSpecialistUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public string AreaSpecialistId { get; set; }

    [Required]
    public string AreaSpecialistName { get; set; }

    [Required]
    public List<string> OutletId { get; set; }

  }

  public class DeleteAreaSpecialistModel
  {
    public string AreaSpecialistId { get; set; }
    public bool isDeleteAreaSpecialist { get; set; }
  }
}
