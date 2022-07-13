using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class GetAllLearningModel
    {
        public int? TrainingId { get; set; }
        public int? Batch { get; set; }
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public string ProgramTypeName { get; set; }
        public int? SetupModuleId { get; set; }
        public string ModuleName { get; set; }
        public string MaterialTypeName { get; set; }
        public Guid BlobId { get; set; }
        public string MIME { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public int? ProgramTypeId { get; set; }
        public int? LearningTypeId { get; set; }
        public string MaterialTypeId { get; set; }
        public bool? IsPopular { get; set; }
    }

    public class GetAllLearningUpdateModel
    {
        public int? TrainingId { get; set; }
        public int? Batch { get; set; }
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public string ProgramTypeName { get; set; }
        public int? SetupModuleId { get; set; }
        public string ModuleName { get; set; }
        public string MaterialTypeName { get; set; }
        public Guid BlobId { get; set; }
        public string MIME { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public int? ProgramTypeId { get; set; }
        public int? LearningTypeId { get; set; }
        public string MaterialTypeId { get; set; }
        public bool IsPopular { get; set; }
        public double? Ratings { get; set; }
        public int WhoTookWhoLike { get; set; }
        public int Top5Course { get; set; }
        public int CoursePrice { get; set; }
    }
}
