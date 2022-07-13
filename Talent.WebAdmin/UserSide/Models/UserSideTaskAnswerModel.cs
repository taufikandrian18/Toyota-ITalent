using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTaskAnswerModel
    {
        public int SetupModuleId { get; set; }
        public int PopQiuzId { get; set; }
        public int TaskAnswerId { get; set; }
        public int TaskId { get; set; }
        public string Answer { get; set; }
        public Guid? AnswerBlobId { get; set; }
        public int Score { get; set; }
        public int Point { get; set; }
        public bool IsTrue { get; set; }
        public bool IsChecked { get; set; }
        public IFormCollection UploadedFile { get; set; }
    }



    public class TaskAnswerInsertModel
    {
        //Task Answer Model
        //public string EmployeeId { get; set; }
        public int? SetupModuleId { get; set; }
        public int? TrainingId { get; set; }
        public int? PopQuizId { get; set; }
        //public string EndDate { get; set; }
        public List<TaskAnswerDetailModel> Answer { get; set; }
    }

    public class TaskAnswerGetAttemptModel
    {
        //Task Answer Model
        public string EmployeeId { get; set; }
        public int? SetupModuleId { get; set; }
        public int? TrainingId { get; set; }
        public int? PopQuizId { get; set; }
        //public string EndDate { get; set; }
        public List<TaskAnswerDetailModel> Answer { get; set; }
    }

    public class TaskAnswerDetailModel
    {
        //public TaskAnswerModel AnswerDetail;

        public int? QuestionTypeId { get; set; }
        public int TaskId { get; set; }
        //Base64String
        public TaskAnswerFileModel File { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
        public int Point { get; set; }
        public bool? IsCheck { get; set; }
        public bool? IsTrue { get; set; }
        public int Attempts { get; set; }
        public List<TaskSpecialAnswerDetailModel> Special { get; set; }
    }

    public class TaskSpecialAnswerDetailModel
    {
        public int Number { get; set; }

        public string Answer { get; set; }

        public int Score { get; set; }

        public int Point { get; set; }

        public bool? IsTrue { get; set; }
    }

    public class TaskAnswerFileModel
    {
        public string FileName { get; set; }
        public string ContextType { get; set; }
        public string Base64String { get; set; }
    }

    public class EmployeeBadgesViewModel
    {
        public string EmployeeId { get; set; }
        public int TopicId { get; set; }
        public int EbadgeId { get; set; }
    }
}
