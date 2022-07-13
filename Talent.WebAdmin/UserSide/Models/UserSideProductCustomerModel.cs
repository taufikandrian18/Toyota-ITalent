using System;
using System.Collections.Generic;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideProductCustomerModel
    {
        public Guid ProductCustomerId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductCustomerTypeId { get; set; }
        public string ProductCustomerUmur { get; set; }
        public string ProductCustomerStatus { get; set; }
        public string ProductCustomerPekerjaan { get; set; }
        public string ProductCustomerPenghasilan { get; set; }
        public string ProductCustomerPrevVehicle { get; set; }
        public string ProductCustomerKebutuhan { get; set; }
        public string ProductCustomerProspectSource { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class UserSideProductCustomerTypeModel
    {
        public Guid ProductCustomerTypeId { get; set; }
        public string ProductCustomerTypeName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class UserSideProductCustomerPaginate
    {
        public List<UserSideProductCustomerModel> ProductCustomers { get; set; }
        public int TotalProductCustomer { get; set; }
        public List<UserSideProductCustomerTypeListView> ProductCustomerTypeNames { get; set; }
    }

    public class UserSideProductCustomerTypeListView
    {
        public string ProductCustomerTypeName { get; set; }
    }
}
