using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ApprovalTrainingViewModel
    {
        public List<ApprovalTrainingModel> ListApprovalTraining { set; get; }
        public int TotalItem { set; get; }
    }
    public class ApprovalTrainingModel
    {
        public int TrainingId { set; get; }
        public string CourseName { set; get; }
        public string ProgramType { set; get; }
        public int Batch { set; get; }
        public int? Enrolment { set; get; }
        public int? Quota { set; get; }
        public DateTime? TrainingStartDate { set; get; }
        public DateTime? TrainingEndDate { set; get; }
    }
    public class ApprovalTrainingFilter : PageAbstract
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CourseName { get; set; }
        public string ProgramTypeName { get; set; }
        public int? Batch { get; set; }
        public int? Enrolment { get; set; }
        public int? Quota { get; set; }
    }
    public class ApprovalTrainingDetailViewModel
    {
        public int TrainingId { set; get; }
        public string CourseName { set; get; }
        public string ProgramTypeName { set; get; }
        public int Batch { set; get; }
        public int? Quota { set; get; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<ApprovalTrainingEmployee> Employees { set; get; }
        public int LearningTypeId { get; set; }
    }
    public class ApprovalTrainingEmployee
    {
        public int EnrollLearningId { set; get; }
        public string EmployeeId { set; get; }
        public string EmployeeName { set; get; }
        public string DealerName { set; get; }
        public string OutletName { set; get; }
        public string PositionId { get; set; }
        public string PositionName { get; set; }
        public bool IsJoined { set; get; }
        public bool IsLocked { get; set; }
        public bool IsDrafted { get; set; }
        public int? TrainingInvitationApprovalStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class TrainingInvitationModel
    {
        public int TrainingId { set; get; }
        public List<string> EmployeeIds { set; get; }
    }
}
