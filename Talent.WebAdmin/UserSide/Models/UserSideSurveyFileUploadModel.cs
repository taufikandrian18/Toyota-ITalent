using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveyFileUploadTypeModel
    {
        public string Question { get; set; }
    }

    public class UserSideSurveyFileUploadContentModel
    {
        public int QuestionTypeId { get; set; }
    }

    public class UserSideSurveyFileUploadModel
    {
        public int SurveyQuestionId { get; set; }

        public string TaskImageUrl { get; set; }
        public string Question { get; set; }
        public UserSideSurveyFileUploadContentModel Content { get; set; }
    }
}
