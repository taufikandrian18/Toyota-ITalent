using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveyMatrixTypeGetModel
    {
        public string Question { get; set; }
    }

    public class UserSideSurveyMatrixTypeModel
    {
        public string Question { get; set; }
    }

    public class UserSideSurveyMatrixQuestionsModel
    {
        public int? Number { get; set; }
        public string Question { get; set; }
    }

    public class UserSideSurveyMatrixContentModel
    {
        public int QuestionTypeId { get; set; }
        public List<string> Headers { get; set; }
        public List<UserSideSurveyMatrixQuestionsModel> Questions { get; set; }
    }

    public class UserSideSurveyMatrixModel
    {
        //Survey
        public int SurveyQuestionId { get; set; }
        public string Question { get; set; }
        //Content
        public UserSideSurveyMatrixContentModel Content { get; set; }
    }
}
