using System;

namespace Talent.Entities.DbQueryModels
{
    public class LearningHistoriesQueryModel
    {
        public int LearningHistoryId { get; set; }
        public string EmployeeId { get; set; }
        public int? TrainingId { get; set; }
        public int? SetupModuleId { get; set; }
        public int? PopQuizId { get; set; }
        public string CourseName { get; set; }
        public int? CourseId { get; set; }
        public Guid? BlobId { get; set; }
        public int? LearningTypeId { get; set; }
        public int? ProgramTypeId { get; set; }
        public int? CoursePrice { get; set; }
        public string Mime { get; set; }
        public string Name { get; set; }
        public double? RatingScore { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? SetupCourseApprovedAt { get; set; }
        public string MaterialTypeId { get; set; }
    }
}