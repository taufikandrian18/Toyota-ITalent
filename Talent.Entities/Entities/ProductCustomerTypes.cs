using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductCustomerTypes
    {
        public ProductCustomerTypes()
        {
            ProductCustomers = new HashSet<ProductCustomers>();
        }
        [Key]
        public Guid ProductCustomerTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductCustomerTypeName { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty("ProductCustomerType")]
        public virtual ICollection<ProductCustomers> ProductCustomers { get; set; }
    }
}
