using System;
using System.Collections.Generic;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideLearningHistoryModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int LearningHistoryId { get; set; }
        public string LearningHistoryName { get; set; }
        public int SetupModuleId { get; set; }
        public double? RatingScore { get; set; }
        public DateTime? SetupCourseApprovedAt { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int TrainingId { get; set; }
    }

    public class UserSideLearningHistoryFilterModel
    {
        /// <summary>
        /// Page Index (current page).
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Page Size (data per-page).
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Keyword (search name)
        /// </summary>
        public string Keyword { get; set; }

        public List<int?> ProgramTypeIds { get; set; }
        public List<int?> LearningTypeIds { get; set; }
        public List<int?> MaterialTypeIds { get; set; }
        public bool IsFree { get; set; }
        public bool IsNotFree { get; set; }
    }

    public class UserSideLearningHistoryFilterValueModel
    {
        public List<UserSideLearningHistoryFilterValueTypeModel> ProgramTypes { get; set; }
        public List<UserSideLearningHistoryFilterValueTypeModel> LearningTypes { get; set; }
        public List<UserSideLearningHistoryFilterValueTypeModel> MaterialTypes { get; set; }
    }

    public class UserSideLearningHistoryFilterValueTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserSideLearningHistoryResponseModel
    {
        public string EmployeeId { get; set; }
        public List<UserSideLearningHistoryModel> Learnings { get; set; }
        public int TotalData { get; set; }
    }

    public class UserSideDetailLearningHistoryModel
    {
        public string EmployeeId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<UserSideModuleLearningHistoryModel> Modules { get; set; }
    }

    public class UserSideModuleLearningHistoryModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public List<UserSideEvaluationLearningHistoryModel> Evaluations { get; set; }
    }

    public class UserSideEvaluationLearningHistoryModel
    {
        public int EvaluationTypeId { get; set; }
        public string EvaluationTypeName { get; set; }
        public float EvaluationScore { get; set; }
    }
}