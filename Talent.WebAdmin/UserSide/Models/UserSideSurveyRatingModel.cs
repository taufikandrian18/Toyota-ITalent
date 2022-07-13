using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveyRatingTypeModel
    {
        public string Question { get; set; }
    }
    public class UserSideSurveyRatingChoiceModel
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }

    public class UserSideSurveyRatingContentModel
    {
        public int QuestionTypeId { get; set; }
        public List<UserSideSurveyRatingChoiceModel> Choice { get; set; }
    }

    public class UserSideSurveyRatingModel
    {
        public int SurveyQuestionId { get; set; }

        public string TaskImageUrl { get; set; }
        public string Question { get; set; }
        public UserSideSurveyRatingContentModel Content { get; set; }
    }
}
