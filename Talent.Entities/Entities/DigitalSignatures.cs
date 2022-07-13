using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class DigitalSignatures
    {
        [Key]
        public int DigitalSignatureId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public Guid? BlobId { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("DigitalSignatures")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("DigitalSignatures")]
        public virtual Employees Employee { get; set; }
    }
}
