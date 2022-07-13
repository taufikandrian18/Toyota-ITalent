using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class GuideModel
    {
        public int GuideId { get; set; }
        public int GuideTypeId { get; set; }
        public Guid BlobId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }

    public class GuideViewModel
    {
        public List<GuideModel> ListGuideModel { get; set; }
        public int TotalItem { get; set; }
    }

    public class GuideFormModel
    {
        public int GuideId { get; set; }
        public int GuideTypeId { get; set; }
        public Guid? BlobId { get; set; }
        [Required]
        [StringLength(1024)]
        public string Name { get; set; }
        [StringLength(1024)]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(32)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public int ApprovalId { get; set; }
        public FileContent FileContent { get; set; }
    }

    public class GuideFilter : PageAbstract
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string GuideName { get; set; }
        public string GuideTypeName { get; set; }
        public string CreatedBy { get; set; }
        public string ApprovalStatus { get; set; }
    }

    public class GuideJoinModel
    {
        public int GuideId { get; set; }
        public int GuideTypeId { get; set; }
        public string GuideTypeName { get; set; }
        public Guid BlobId { get; set; }
        public string FileName { get; set; }
        public string Mime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string ApprovalStatus { get; set; }
    }

    public class GuideJoinViewModel
    {
        public List<GuideJoinModel> ListGuideJoinModel { get; set; }
        public int TotalItem { get; set; }
    }
}
