using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveyAnswerModel
    {
        public int SurveyId { get; set; }
        public string Answer { get; set; }
        public Guid? AnswerBlobId { get; set; }
        public IFormCollection UploadedFile { get; set; }
    }
    public class SurveyAnswerInsertModel
    {
        //Task Answer Model
        //public string EmployeeId { get; set; }
        public int SurveyId { get; set; }
        //public string EndDate { get; set; }
        public List<SurveyAnswerDetailModel> Answer { get; set; }
    }

    public class SurveyAnswerDetailModel
    {
        //public TaskAnswerModel AnswerDetail;

        public int? QuestionTypeId { get; set; }
        public int SurveyQuestionId { get; set; }
        //Base64String
        public SurveyAnswerFileModel File { get; set; }
        public string Answer { get; set; }
        public List<SurveySpecialAnswerDetailModel> Special { get; set; }
    }

    public class SurveySpecialAnswerDetailModel
    {
        public int Number { get; set; }
        public string Answer { get; set; }
    }

    public class SurveyAnswerFileModel
    {
        public string FileName { get; set; }
        public string ContextType { get; set; }
        public string Base64String { get; set; }
    }
}
