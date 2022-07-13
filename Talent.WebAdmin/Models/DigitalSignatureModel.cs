using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class DigitalSignatureModel
    {
        public int DigitalSignatureId { get; set; }
        public string EmployeeId { get; set; }
        public Guid? BlobId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class DigitalSignatureJoinedModel
    {
        public int DigitalSignatureId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public Guid? BlobId { get; set; }
        public string BlobName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Mime { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class DigitalSignatureViewModel
    {
        public List<DigitalSignatureJoinedModel> ListDigitalSignature { get; set; }
        public int TotalItem { get; set; }

    }

    public class DigitalSignatureFilter : PageAbstract
    {
        public string EmployeeName { get; set; }
    }

    public class DigitalSignatureFormModel
    {
        public int? DigitalSignatureId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public Guid? BlobId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public FileContent FileContent { get; set; }
    }
}
