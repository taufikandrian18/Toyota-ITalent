using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class AssessmentCreateModel
    {
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string AssessmentName { get; set; }
        [Required]
        public string Publisher { get; set; }
        public string Description { get; set; }
        [Required]
        public FileContent FileContent { get; set; }
    }

    public class AssessmentFilterModel : GridFilter
    {
        public DateTimeOffset? DateStart { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
        public string OutletName { get; set; }
        public string AssessmentName { get; set; }
        public string PositionName { get; set; }
        public string EmployeeName { get; set; }
        public string PublisherName { get; set; }
    }

    public class AssessmentGridViewModel
    {
        public int AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public string EmployeeName { get; set; }
        public string PositionName { get; set; }
        public string OutletName { get; set; }
        public string PublisherName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AssessmentGridModel
    {
        public List<AssessmentGridViewModel> Assessments { get; set; }
        public int TotalFilterData { get; set; }
    }

    public class AssessmentViewDetailModel
    {
        public int AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public string PublisherName { get; set; }
        public string Description { get; set; }
        public DealerDropdownModel Dealer { get; set; }
        public OutletDropdownModel Outlet { get; set; }
        public PositionDropdownModel Position { get; set; }
        public EmployeeDropdownModel Employee { get; set; }
        public string ImageBlobId { get; set; }
        public string ImageBlobName { get; set; }
        public string ImageBlobMIME { get; set; }
    }

    public class AssessmentUpdateModel
    {
        public int AssessmentId { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string AssessmentName { get; set; }
        [Required]
        public string Publisher { get; set; }
        public string Description { get; set; }
        [Required]
        public Guid BlobId { get; set; }
        public FileContent FileContent { get; set; }
    }
}
