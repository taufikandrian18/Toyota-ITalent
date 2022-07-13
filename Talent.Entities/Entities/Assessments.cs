using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Assessments
    {
        [Key]
        public int AssessmentId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public Guid BlobId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Publisher { get; set; }
        public string Description { get; set; }
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
        [InverseProperty("Assessments")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("Assessments")]
        public virtual Employees Employee { get; set; }

 
    }
}
