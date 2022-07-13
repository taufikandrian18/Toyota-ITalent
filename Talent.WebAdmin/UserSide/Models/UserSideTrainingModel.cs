using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTrainingModel
    {
        public int TrainingId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int Batch { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public List<UserSideEmployeeTrainingModel> ListEmployeeTraining { get; set; }
    }

    public class UserSideAllTrainingModel
    {
        public int TrainingId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int Batch { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }

    public class UserSideTrainingViewModel
    {
        public List<UserSideAllTrainingModel> ListCourseTraining { get; set; }
    }

    public class UserSideEmployeeTrainingModel
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string PositionName { get; set; }
        public string ImageUrl { get; set; }
        public bool IsEvaluate { get; set; }
        public List<UserSideEvaluateTraineeModel> ListQuestion { get; set; }
    }

    public class UserSideEvaluateTraineeModel
    {
        public int QuestionNumber { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionTypeName { get; set; }
        public string Question { get; set; }
        public int TaskId { get; set; }
        public string Answer { get; set; }
        public string TaskAnswerType {get;set;}
        public int TaskAnswerDetailId { get; set; }
    }

    public class UserSideEvaluateCoachPoints
    {
        public int Points { get; set; }
        public string EmployeeId { get; set; }
    }
}
