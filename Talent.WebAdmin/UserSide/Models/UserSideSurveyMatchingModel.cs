using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveyMatchingTypeModel
    {
        public string Question { get; set; }
    }

    public class UserSideSurveyMatchingChoiceGetModel
    {
        public Guid? BlobId { get; set; }
        public string SurveyMatchingChoiceCode { get; set; }
        public string Text { get; set; }
    }

    public class UserSideSurveyMatchingChoiceModel
    {
        public string ImageUrl { get; set; }
        public int Number { get; set; }
        public string Text { get; set; }
    }

    public class UserSideSurveyMatchingLeftRightChoiceModel
    {
        public UserSideSurveyMatchingChoiceModel Left { get; set; }
        public UserSideSurveyMatchingChoiceModel Right { get; set; }
    }

    public class UserSideSurveyMatchingContentModel
    {
        //Type
        public int QuestionTypeId { get; set; }
        //Choice
        public List<UserSideSurveyMatchingLeftRightChoiceModel> Choice { get; set; }
    }

    public class UserSideSurveyMatchingModel
    {
        public int SurveyQuestionId { get; set; }
        public string Question { get; set; }
        //Content
        public UserSideSurveyMatchingContentModel Content { get; set; }
    }
}
