using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EmployeeCertificates
    {
        public int EmployeeCertificateId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        [Required]
        [StringLength(255)]
        public string Type { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [Required]
        [StringLength(255)]
        public string TrainingName { get; set; }
        public DateTime EventDate { get; set; }
        [Required]
        [StringLength(255)]
        public string Host { get; set; }
        [Required]
        [StringLength(255)]
        public string CertificationHeader { get; set; }
        public string Venue { get; set; }
        public Guid? BlobId { get; set; }
        public int? CourseId { get; set; }
        public int? TrainingId { get; set; }
        
        [StringLength(255)] 
        public string CertificateNumber { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("EmployeeCertificates")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("CourseId")]
        [InverseProperty("EmployeeCertificates")]
        public virtual Courses Course { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("EmployeeCertificates")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("TrainingId")]
        [InverseProperty("EmployeeCertificates")]
        public virtual Trainings Training { get; set; }
    }
}
