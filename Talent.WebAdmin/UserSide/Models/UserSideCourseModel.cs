using System;
using System.Collections.Generic;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.UserSide.Models
{

    public class UserSideCourseViewModel
    {
        //public int ID { get; set; }
        public string TrainingId { get; set; }
        public string TrainingName { get; set; }
        public string ProgramTypeName { get; set; }
        public List<string> TopicName { get; set; }
        public int? Duration { get; set; }
        public int? Points { get; set; }
        public double? Rating { get; set; }
        public int? Price { get; set; }
        public string ImageUrl { get; set; }
        public int Batch { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsQueue { get; set; }
        public bool IsEnroll { get; set; }
        public bool IsJoined { get; set; }
        public bool IsAllowEnroll { get; set; }
        public bool? IsPassed { get; set; }
        public int RemedialLevel { get; set; }
        public bool? CanEnroll { get; set; }
        public string RelatedTrainingId {get;set;}
        public string RelatedTrainingName {get;set;}
        public bool RelatedTrainingAssesment {get;set;}
    }


    public class UserSideModuleViewModel
    {
        public string ImageUrl { get; set; }
        public int Points { get; set; }
        public string ModuleName { get; set; }
        public string MaterialTypeName { get; set; }
        public string ModuleDescription { get; set; }
        public List<string> TopicNames { get; set; }
        public int Duration { get; set; }
        public bool IsQueue { get; set; }
        public bool IsEnroll { get; set; }
        public bool IsJoined { get; set; }
         public string RelatedTrainingId {get;set;}
        public string RelatedTrainingName {get;set;}
    }

    public class UserSideCourseOverviewModel
    {
        public string CourseDesc { get; set; }
        public List<string> LearningObjs { get; set; }
        public List<UserSidePrerequisiteModel> Prerequisites { get; set; }
    }

    public class UserSideCourseTrainingViewModel
    {
        public string TrainingTypeName { get; set; }
        public int MinScore { get; set; }
        public string WorkloadReq { get; set; }
        public int Percentage { get; set; }
    }

    public class UserSideCourseCoachViewModel
    {
        public int? CoachId { get; set; }
        public string CoachName { get; set; }

        public string ImageUrl { get; set; }
    }

    //model to store temporary data from linq
    public class UserSideTempViewModel
    {
        public int? TrainingId { get; set; }
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string LearningName { get; set; }
        public int? NextId { get; set; }
        public int? PrevID { get; set; }
        public int? NextSMId { get; set; }
    }

    public class UserSidePrerequisiteModel
    {
        public int? TrainingId { get; set; }
        public int? SetupModuleId { get; set; }
        public string Image { get; set; }
        public string ModuleName { get; set; }
        public int? CourseId { get; set; }
        public int? Point { get; set; }
        public int? Price { get; set; }
        public int? Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CourseName { get; set; }
        public int Percentage { get; set; }
    }

    public class UserSideTrainingCourseModel
    {
        public string TrainingTypeName { get; set; }
        public int MinScore { get; set; }
        public string WorkloadReq { get; set; }
        public int Percentage { get; set; }
    }

    /// <summary>
    /// Model class for storing homepage's course data.
    /// </summary>
    public class UserSideMyLearningHomepageCourseModel
    {
        public int? TrainingId { get; set; }
        public int? CourseId { get; set; }
        public int? Price { get; set; }
        public int? Points { get; set; }
        public int? SetupModuleId { get; set; }
        public int? TrainingBatch { get; set; }
        public string ImageUrl { get; set; }
        public string TrainingName { get; set; }
        public double? Rating { get; set; }
        public string ProgramTypeName { get; set; }
        public string MaterialTypeName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public double Percentage { get; set; }
    }

    public class UserSideFreePopularCoursesModel
    {
        public int CourseID { get; set; }
        public string ImageUrl { get; set; }
        public string CoursesName { get; set; }
        public string ProgramTypeName { get; set; }
    }

    /// <summary>
    /// model untuk homepage my learning
    /// </summary>
    public class UserSideMyLearningHomepageModuleModel
    {
        public int SetupModuleId { get; set; }
        public string ImageUrl { get; set; }
        public string ModuleName { get; set; }
        public string MaterialTypeName { get; set; }
        public DateTime? ApprovedAt { get; set; }

    }

    public class UserSideMyLearningHomepageCourseBadgeModel
    {
        public int TrainingId { get; set; }
        public int TrainingBatch { get; set; }
        public string ImageUrl { get; set; }
        public string TrainingName { get; set; }
        public double? Rating { get; set; }
        public string ProgramTypeName { get; set; }
        public int BronzeBadge { get; set; }
        public int SilverBadge { get; set; }
        public int GoldBadge { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UserSideMyLearningHomepageCourseBadgeModelNew
    {
        public int? TrainingId { get; set; }
        public int? TrainingBatch { get; set; }
        public int? SetupModuleId { set; get; }
        public int? CourseId { set; get; }
        public string ImageUrl { get; set; }
        public string TrainingName { get; set; }
        public double? Rating { get; set; }
        public string ProgramTypeName { get; set; }
        public int BronzeBadge { get; set; }
        public int SilverBadge { get; set; }
        public int GoldBadge { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UserSideCourseLikePeopleListModel
    {
        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string Position { get; set; }

        public string ImageUrl { get; set; }
    }

    public class UserSideCourseLikePeopleSampleImage
    {
        public List<string> ImageUrl { get; set; }
        public int Count { get; set; }
    }

    public class UserSideWhoTookTheCourseSampleImage
    {
        public List<string> ImageUrl { get; set; }
        public int Count { get; set; }
    }

    public class UserSidePeopleWhoTookTheCourseListModel
    {
        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string Position { get; set; }

        public string ImageUrl { get; set; }
    }

    public class ModuleContentViewModel
    {
        public int? SetupModuleId { get; set; }
        public int? ModuleId { get; set; }
        public string AssesmentId { get; set; }

        public string ContentName { get; set; }

        public string ModuleType { get; set; }

        public string ImageUrl { get; set; }

        public long FileSize { get; set; }

        public int? ModuleTimeLength { get; set; }

        public float? Orders { get; set; }

        public int? ModuleTotalPoints { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsEnrolled { get; set; }
        public bool IsAvailable { get; set; }
        public bool? IsPublished { get; set; }
        public DateTime? ModuleStartTime { get; set; }
        public DateTime? ModuleEndTime { get; set; }
        public bool IsLast { get; set; }
        public bool IsDownloaded{get;set;}
        public int? CourseId { get; set; }
    }

    public class ModuleContentViewPaginationModel
    {
        public List<ModuleContentViewModel> Modules { get; set; }
        public int Index { get; set; }
        public int TotalData { get; set; }
        public int ItemPerPage { get; set; }
        public int TotalPages { get; set; }
    } 

    public class CourseReviewViewModel
    {
        public string EmployeeName { get; set; }
        public DateTime SubmitTime { get; set; }
        public int Score { get; set; }
        public string Review { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CourseReviewSummaryViewModel
    {
        public double Average { get; set; }
        public int CountOverall { get; set; }
        public int Count5Stars { get; set; }
        public int Count4Stars { get; set; }
        public int Count3Stars { get; set; }
        public int Count2Stars { get; set; }
        public int Count1Stars { get; set; }
        public List<CourseReviewViewModel> Reviews { get; set; }
    }

    public class TotalPointsModel
    {
        public int TaskTrueFalseTypesScore { get; set; }
        public int TaskTebakGambarTypesScore { get; set; }
        public int TaskSequenceChoicesScore { get; set; }
        //TaskRatingTypesScore { get; set; }
        //TaskMatrixTypesScore { get; set; }
        public int TaskMatchingTypesScore { get; set; }
        public int TaskHotSpotAnswersScore { get; set; }
        public int TaskChecklistChoicesScore { get; set; }
    }

    public class TeamMemberDetail
    {
        public string EmployeeId { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string ImageUrl { get; set; }

        public bool IscChecked { get; set; }

        public bool IsValidAsignee { get; set; }
    }

    public class TeamMemberAssignModel
    {
        public List<string> EmployeeId { get; set; }
        public int? TrainingId { get; set; }
        public int? SetupModuleId { get; set; }
    }

    public class TeamMemberGridModel
    {
        public List<TeamMemberDetail> TeamMember { get; set; }
    }

    public class MemberGridFilter : GridFilter
    {
        public int TrainingId { get; set; }
        public string EmployeeName { get; set; }
    }


    public class UserSideMyProfileCourseResponseModel
    {
        public List<UserSideMyProfileCourseModel> Courses { get; set; }
        public int TotalData { get; set; }
    }


    public class UserSideMyProfileCourseFilterModel
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
    }


    public class UserSideMyProfileCourseModel
    {
        public int TrainingId { get; set; }
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public string ImageUrl { get; set; }
        public double Rating { get; set; }
        public bool IsAssign { get; set; }
    }

    public class UserSideMyProfileAsignLearningModel
    {
        public int TrainingId { get; set; }

        /// <summary>
        /// Assign to Employee / Member Id
        /// </summary>
        public string AssignedTo { get; set; }
    }

    public class GetModule
    {
        public int SetupModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public string FileUrl { get; set; }
        public string FileDownloadUrl { get; set; }
        public long FileSize { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Double Rating { get; set; }
        public int point { get; set; }
        public int? Duration { get; set; }
        public string MaterialType { get; set; }
        public int? MinimumScore { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TestTime { get; set; }
        public bool? IsForRemedial { get; set; }
        public bool? IsDownloadable { get; set; }
        public bool? CanEnroll { get; set; }
    }

    public class GetModuleTemp
    {
        public int SetupModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string Mime { get; set; }
        public double Rating { get; set; }
        public int point { get; set; }
        public int? Duration { get; set; }
        public string MaterialType { get; set; }
        public int? MinimumScore { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TestTime { get; set; }
        public bool? isForRemedial { get; set; }
        public bool? IsDownloadble { get; set; }

    }

    public class EnrollQueueModel
    {
        public int TrainingId { get; set; }
    }

    public class CheckModuleStartModel
    {
        public int? TrainingId { get; set; }
        public int SetupModuleId { get; set; }
    }

    public class GroupedTaskModel
    {
        public int QuestionTypeId { get; set; }
        public List<int> TaskIds { get; set; }
    }

    public class SetupLearningContentQueryModel
    {
        public int Batch { get; set; }
        public int RatingScore { get; set; }
        public int? Price { get; set; }
        public int? Time { get; set; }
        public int? CourseId { get; set; }
        public string AssesmentGUID { get; set; }
        public int? TrainingId { get; set; }
        public int ModuleId { get; set; }
        public int RemedialLevel { get; set; }
        public Guid BlobId { get; set; }
        public int Points { get; set; }
        public bool? IsPassed { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsOffline { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? EndDate { get; set; }
        public string CourseName { get; set; }
        public string TopicName { get; set; }
        public string AssesmentName { get; set; }
        public string ProgramTypeName { get; set; }
        public string Mime { get; set; }
    }
}
