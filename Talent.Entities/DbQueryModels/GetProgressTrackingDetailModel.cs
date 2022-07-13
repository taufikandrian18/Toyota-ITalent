using System;
namespace Talent.Entities.DbQueryModels
{
    public class GetProgressTrackingDetailModel
    {
        public int TaskAnswerDetailId { get; set; }
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
        public int TrainingId { get; set; }
        public int TaskId { get; set; }
        public string CourseName { get; set; }
        public string ModuleName { get; set; }
        public int AttemptedCount { get; set; }
        public string AttemptedDate { get; set; }
        public Decimal Score { get; set; }
        public Decimal MinimumScore { get; set; }
        public Decimal TotalScore { get; set; }
        public string QuestionType { get; set; }
        public string Questions { get; set; }
        public string Answers { get; set; }
    }

    public class GetProgressTrackingDetailAssesment
    {
        public string GUID { get; set; }
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
        public int TrainingId { get; set; }
        public string CourseName { get; set; }
        public string AssesmentName { get; set; }
        public int AttemptedCount { get; set; }
        public string AttemptedDate { get; set; }
        public Decimal Score { get; set; }
        public Decimal MinimumScore { get; set; }
        public Decimal TotalScore { get; set; }
        public string SkillCheckName { get; set; }
    }
}
