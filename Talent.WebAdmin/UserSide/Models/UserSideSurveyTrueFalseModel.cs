using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveyTrueFalseTypeModel
    {
        public string Question { get; set; }

    }

    public class UserSideSurveyTrueFalseContentModel
    {
        public int QuestionTypeId { get; set; }
    }
    public class UserSideSurveyTrueFalseModel
    {
        public int SurveyQuestionId { get; set; }

        public string TaskImageUrl { get; set; }
        public string Question { get; set; }
        public UserSideSurveyTrueFalseContentModel Content { get; set; }
    }
}

