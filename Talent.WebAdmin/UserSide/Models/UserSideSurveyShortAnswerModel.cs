using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveyShortAnswerTypeModel
    {
        public string Question { get; set; }
    }

    public class UserSideSurveyShortAnswerContentModel
    {
        public int QuestionTypeId { get; set; }
    }

    public class UserSideSurveyShortAnswerModel
    {
        public int SurveyQuestionId { get; set; }

        public string TaskImageUrl { get; set; }
        public string Question { get; set; }
        public UserSideSurveyShortAnswerContentModel Content { get; set; }
    }
}
