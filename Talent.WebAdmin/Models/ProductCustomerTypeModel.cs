using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
    public class ProductCustomerTypeModel
    {
        public Guid ProductCustomerTypeId { get; set; }
        public string ProductCustomerTypeName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ProductCustomerTypePaginate
    {
        public List<ProductCustomerTypeModel> ProductCustomerTypes { get; set; }
        public int TotalProductCustomerTypes { get; set; }
    }

    public class ProductCustomerTypeGridFilter : GridFilter
    {
        public string ProductCustomerTypeName { get; set; }
    }

    public class ProductCustomerTypeCreateModel
    {
        /// <summary>
        /// store product name to be created
        /// </summary>
        [Required]
        public string ProductCustomerTypeName { get; set; }

    }
    public class ProductCustomerTypeUpdateModel
    {
        [Required]
        public Guid ProductCustomerTypeId { get; set; }
        /// <summary>
        /// store product name to be created
        /// </summary>
        [Required]
        public string ProductCustomerTypeName { get; set; }

    }
    public class DeleteProductCustomerTypeModel
    {
        public Guid ProductCustomerTypeId { get; set; }
        public bool isDeleteProductCustomerType { get; set; }
    }
}
