using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductCustomers
    {
        public ProductCustomers()
        {
        }
        [Key]
        public Guid ProductCustomerId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductCustomerTypeId { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string ProductCustomerStatus { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string ProductCustomerPenghasilan { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string ProductCustomerKebutuhan { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string ProductCustomerUmur { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string ProductCustomerPekerjaan { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string ProductCustomerPrevVehicle { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string ProductCustomerProspectSource { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("ProductCustomers")]
        public virtual Products Product { get; set; }

        [ForeignKey("ProductCustomerTypeId")]
        [InverseProperty("ProductCustomers")]
        public virtual ProductCustomerTypes ProductCustomerType { get; set; }
    }
}
