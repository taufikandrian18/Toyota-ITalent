using System;
namespace Talent.Entities.DbQueryModels
{
    public class GetListDoubleDataModel
    {
        public string employeeguid { get; set; }
        public string skillcheckguid { get; set; }
        public Single attempts { get; set; }
        public int doubleData { get; set; }
    }

    public class GetListDoubleModuleDataModel
    {
        public int taskanswerid { get; set; }
        public int taskid { get; set; }
        public Single attempts { get; set; }
        public int doubleData { get; set; }
    }

    public class GetListModuleDoubleModel
    {
        public int TaskAnswerDetailId { get; set; }
        public int TaskAnswerId { get; set; }
        public int TaskId { get; set; }
        public string Answer { get; set; }
        public Guid? AnswerBlobId { get; set; }
        public int? Score { get; set; }
        public int? Point { get; set; }
        public float Attempts { get; set; }
        public bool IsFinished { get; set; }
        public string ScorerGUID { get; set; }
        public string Feedback { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class GetListAssesmentDoubleModel
    {
        public string GUID { get; set; }
        public string EmployeeGUID { get; set; }
        public string ScorerGUID { get; set; }
        public string ScorerType { get; set; }
        public string SkillCheckGUID { get; set; }
        public string Answer { get; set; }
        public string Text { get; set; }
        public Single Point { get; set; }
        public Single Score { get; set; }
        public bool IsDraft { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Single Attempts { get; set; }
        public bool IsScored { get; set; }
        public bool IsFinished { get; set; }
        public string AssesmentGUID { get; set; }
        public int TrainingId { get; set; }
    }
}
