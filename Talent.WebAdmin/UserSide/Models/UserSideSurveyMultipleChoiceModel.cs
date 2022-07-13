using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveyMultipleChoiceTypeModel
    {
        public string Question { get; set; }
    }
    public class UserSideSurveyMultipleChoiceChoiceModel
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }

    public class UserSideSurveyMultipleChoiceContentModel
    {
        public int QuestionTypeId { get; set; }
        public List<UserSideSurveyMultipleChoiceChoiceModel> Choice { get; set; }
    }
    public class UserSideSurveyMultipleChoiceModel
    {
        public int SurveyQuestionId { get; set; }

        public string TaskImageUrl { get; set; }
        public string Question { get; set; }
        //Content
        public UserSideSurveyMultipleChoiceContentModel Content { get; set; }
    }
}
