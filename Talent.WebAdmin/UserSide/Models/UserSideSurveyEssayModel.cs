using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveyEssayTypeModel
    {
        public string Question { get; set; }
    }

    public class UserSideSurveyEssayContentModel
    {
        public int QuestionTypeId { get; set; }
    }

    public class UserSideSurveyEssayModel
    {
        public int SurveyQuestionId { get; set; }

        public string TaskImageUrl { get; set; }
        public string Question { get; set; }
        public UserSideSurveyEssayContentModel Content { get; set; }
    }
}
