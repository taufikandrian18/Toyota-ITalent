using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveySequenceTypeModel
    {
        public string Question { get; set; }
    }
    public class UserSideSurveySequenceModel
    {
        //Task
        public int SurveyQuestionId { get; set; }
        public string Question { get; set; }
        //Content
        public UserSideSurveySequenceContentModel Content { get; set; }
    }
    public class UserSideSurveySequenceChoiceModel
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }
    public class UserSideSurveySequenceContentModel
    {
        //Type
        public int QuestionTypeId { get; set; }
        //Choice
        public List<UserSideSurveySequenceChoiceModel> Choice { get; set; }
    }
}

