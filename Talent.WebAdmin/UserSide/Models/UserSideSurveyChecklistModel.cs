using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveyChecklistTypeModel
    {
        public string Question { get; set; }
    }
    //datanya itu berbentuk list
    public class UserSideSurveyChecklistChoiceModel
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }

    public class UserSideSurveyChecklistContentModel
    {
        //Type
        public int QuestionTypeId { get; set; }
        //Choice
        public List<UserSideSurveyChecklistChoiceModel> Choice { get; set; }
    }

    public class UserSideSurveyChecklistModel
    {
        public int SurveyQuestionId { get; set; }

        public string TaskImageUrl { get; set; }
        public string Question { get; set; }
        //Content
        public UserSideSurveyChecklistContentModel Content { get; set; }

    }
}
