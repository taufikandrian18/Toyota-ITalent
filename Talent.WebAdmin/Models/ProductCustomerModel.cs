using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
  public class ProductCustomerModel
  {
    public Guid ProductCustomerId { get; set; }
    public Guid ProductId { get; set; }
    public Guid ProductCustomerTypeId { get; set; }
    public string ProductCustomerTypeName { get; set; }
    public string ProductCustomerStatus { get; set; }
    public string ProductCustomerKebutuhan { get; set; }
    public string ProductCustomerPenghasilan { get; set; }
    public string ProductCustomerUmur { get; set; }
    public string ProductCustomerPekerjaan { get; set; }
    public string ProductCustomerPrevVehicle { get; set; }
    public string ProductCustomerProspectSource { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class ProductCustomerPaginate
  {
    public List<ProductCustomerModel> ProductCustomers { get; set; }
    public int TotalProductCustomers { get; set; }
  }

  public class ProductCustomerGridFilter : GridFilter
  {
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public Guid ProductId { get; set; }
  }

  public class ProductCustomerCreateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductCustomerTypeId { get; set; }

    [Required]
    public string ProductCustomerStatus { get; set; }

    [Required]
    public string ProductCustomerKebutuhan { get; set; }

    public string ProductCustomerPenghasilan { get; set; }

    [Required]
    public string ProductCustomerUmur { get; set; }

    [Required]
    public string ProductCustomerPekerjaan { get; set; }

    [Required]
    public string ProductCustomerPrevVehicle { get; set; }

    public string ProductCustomerProspectSource { get; set; }

    public DateTime? ApprovedAt { get; set; }

  }

  public class ProductCustomerUpdateModel
  {
    /// <summary>
    /// store product name to be created
    /// </summary>

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductCustomerTypeId { get; set; }

    [Required]
    public Guid ProductCustomerId { get; set; }

    [Required]
    public string ProductCustomerStatus { get; set; }

    [Required]
    public string ProductCustomerKebutuhan { get; set; }

    public string ProductCustomerPenghasilan { get; set; }

    [Required]
    public string ProductCustomerUmur { get; set; }

    [Required]
    public string ProductCustomerPekerjaan { get; set; }

    [Required]
    public string ProductCustomerPrevVehicle { get; set; }

    public string ProductCustomerProspectSource { get; set; }

    public DateTime? ApprovedAt { get; set; }
  }

  public class DeleteProductCustomerModel
  {
    public Guid ProductCustomerId { get; set; }
    public bool isDeleteProductCustomer { get; set; }
  }
}
