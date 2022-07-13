using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ApprovalModel
    {
        public int ApprovalId { get; set; }
        public string ContentName { get; set; }
        public string ContentCategory { get; set; }
        public int? ApprovalMappingId { get; set; }
        public int ApprovalLevel { get; set; }
        public int ApprovalStatusId { get; set; }
        public string ActionBy { get; set; }
        public DateTime ActionAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ApprovalFormModel
    {
        public int ApprovalId { get; set; }
        [Required]
        [StringLength(1024)]
        public string ContentName { get; set; }
        [Required]
        [StringLength(255)]
        public string ContentCategory { get; set; }
        public int? ApprovalMappingId { get; set; }
        public int ApprovalLevel { get; set; }
        public int ApprovalStatusId { get; set; }
        [Required]
        [StringLength(32)]
        public string ActionBy { get; set; }
        public DateTime ActionAt { get; set; }
        [Required]
        [StringLength(32)]
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ApprovalCreateFormModel
    {
        public string ContentName { get; set; }
        public string ContentCategory { get; set; }
        public int ApprovalStatusId { get; set; }
        public string PageEnum { get; set; }
        public int ContentId { get; set; }
    }

    public class ApprovalUpdateFormModel
    {
        public int ApprovalId { get; set; }
        public string ContentName { get; set; }
        public string PageEnum { get; set; }
        public int ApprovalStatusId { get; set; }
        public int ContentId { get; set; }
        public string ContentCategory { get; set; }

    }

    public class ApprovalOctopusModel
    {
        public string PageEnums { get; set; }
        public int ContentId { get; set; }
        public int ApprovalId { get; set; }
    }
}
