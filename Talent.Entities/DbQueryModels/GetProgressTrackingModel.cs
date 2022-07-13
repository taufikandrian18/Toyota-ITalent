using System;
namespace Talent.Entities.DbQueryModels
{
    public class GetProgressTrackingModel
    {
        public int SetupModuleId { get; set; }
        public int ProgramTypeId { get; set; }
        public string ProgramTypeName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string OutletId { get; set; }
        public string OutletName { get; set; }
        public string AreaId { get; set; }
        public string AreaName { get; set; }
        public string AreaSpecialistId { get; set; }
        public string AreaSpecialistName { get; set; }
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string DealerId { get; set; }
        public string DealerName { get; set; }
        public string Flagging { get; set; }
        public int TrainingId { get; set; }
        public string CourseName { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string AssesmentId { get; set; }
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
}
