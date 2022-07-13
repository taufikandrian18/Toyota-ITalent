using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class CourseModel
    {
        public int CourseId { get; set; }
        public int ProgramTypeId { get; set; }
        public int LevelId { get; set; }
        public int CourseCategoryId { get; set; }
        public int LearningTypeId { get; set; }
        public Guid BlobId { get; set; }
        public int? ApprovalId { get; set; }
        public string CourseName { get; set; }
        public int? CoursePrice { get; set; }
        public string CourseDescription { get; set; }
        public string Others { get; set; }
        public bool? IsRecommendedForYou { get; set; }
        public bool? IsPopular { get; set; }
        public bool? IsPublished { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CourseApprovedAt { get; set; }
        public DateTime? SetupCourseApprovedAt { get; set; }
    }

    public class CourseViewModel
    {
        public List<CourseModel> ListCourseModel { get; set; }
        public int TotalItem { get; set; }
    }

    public class CourseFormModel
    {
        public int CourseId { get; set; }
        public int ProgramTypeId { get; set; }
        public int LevelId { get; set; }
        public int CourseCategoryId { get; set; }
        public int LearningTypeId { get; set; }
        public Guid BlobId { get; set; }
        public int ApprovalId { get; set; }
        [Required]
        [StringLength(255)]
        public string CourseName { get; set; }
        public int CoursePrice { get; set; }
        [StringLength(1024)]
        public string CourseDescription { get; set; }
        [StringLength(64)]
        public string Others { get; set; }
        public bool IsRecommendedForYou { get; set; }
        public bool IsPopular { get; set; }
        public bool IsPublished { get; set; }
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }

        public FileContent ImageUpload { get; set; }
    }

    public class CourseFilter : PageAbstract
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string CourseName { get; set; }
        public string ProgramTypeName { get; set; }
        public string LearningName { get; set; }
        public string CourseCategoryName { get; set; }
        public string Pricing { get; set; }
        public string ApprovalName { get; set; }
    }

    public class CourseJoinModel
    {
        public int CourseId { get; set; }

        public int ProgramTypeId { get; set; }
        public int LevelId { get; set; }
        public int CourseCategoryId { get; set; }
        public int LearningTypeId { get; set; }
        public Guid BlobId { get; set; }
        public int? ApprovalId { get; set; }

        public string ProgramTypeName { get; set; }
        public string LevelName { get; set; }
        public string CourseCategoryName { get; set; }
        public string LearningName { get; set; }
        public string Mime { get; set; }
        public string FileName { get; set; }
        public string ApprovalName { get; set; }

        public string CourseName { get; set; }
        public int? CoursePrice { get; set; }
        public string CourseDescription { get; set; }
        public string Others { get; set; }
        public bool? IsRecommendedForYou { get; set; }
        public bool? IsPopular { get; set; }
        public bool? IsPublished { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CourseApprovedAt { get; set; }
        public DateTime? SetupCourseApprovedAt { get; set; }
    }

    public class CourseJoinViewModel
    {
        public List<CourseJoinModel> ListCourseJoinModel { get; set; }
        public int TotalItem { get; set; }
    }
}
