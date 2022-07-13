using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveyHotSpotTypeModel
    {
        public string Question { get; set; }
    }

    public class UserSideSurveyHotSpotAnswerGetModel
    {
        public int Number { get; set; }
    }
    public class UserSideSurveyHotSpotAnswerModel
    {
        public int Number { get; set; }
    }

    public class UserSideSurveyHotSpotContentModel
    {
        //Type
        public int QuestionTypeId { get; set; }
        public string ImageUrl { get; set; }
        //Choice
        public List<UserSideSurveyHotSpotAnswerModel> Choice { get; set; }
    }

    public class UserSideSurveyHotSpotModel
    {
        //Survey
        public int SurveyQuestionId { get; set; }
        public string Question { get; set; }
        //Content
        public UserSideSurveyHotSpotContentModel Content { get; set; }
    }
}
