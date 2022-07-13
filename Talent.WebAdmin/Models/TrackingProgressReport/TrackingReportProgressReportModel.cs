using System;
using System.Collections.Generic;

namespace Talent.WebAdmin.Models.TrackingProgressReport
{
    public class TrackingReportProgressReportModel
    {
        public int SetupModuleId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string OutletName { get; set; }
        public string AreaName { get; set; }
        public string AreaSpecialistName { get; set; }
        public string PositionName { get; set; }
        public string DealerName { get; set; }
        public string ProgramTypeName { get; set; }
        public string LearningType { get; set; }
        public int TrainingId { get; set; }
        public string CourseName { get; set; }
        public string ModuleName { get; set; }
        public string AssesmentName { get; set; }
        public string Batch { get; set; }
        public string FirstAccess { get; set; }
        public string LastAccess { get; set; }
        public string TotalStudyTime { get; set; }
        public string AverageScore { get; set; }
        public string HighestScore { get; set; }
        public string LatestScore { get; set; }
        public string FinalScore { get; set; }
        public string CompletionStatus { get; set; }
        public string MinimumScore { get; set; }
        public Single Attempts { get; set; }
    }

    public class TrackingReportProgressReportPaginate
    {
        public List<TrackingReportProgressReportModel> TrackingReportProgressReportList { get; set; }
        public int TotalTrackingReportProgressReport { get; set; }
    }

    public class GetAccountParameterProgressTracking
    {
        public int ProgramTypeId { get; set; }
        public int TrainingId { get; set; }
        public string AreaId { get; set; }
        public string AreaSpecialistId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string OutletID { get; set; }
        public string DealerId { get; set; }
        public string LearningType { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
