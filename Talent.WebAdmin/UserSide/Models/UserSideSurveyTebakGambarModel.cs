using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveyTebakGambarPicturesGetModel
    {
        public Guid BlobId { get; set; }
        public int Number { get; set; }
    }

    public class UserSideSurveyTebakGambarPicturesModel
    {
        public string ImageUrl { get; set; }
        //SURVEY CHOICE ID
        public int Number { get; set; }
    }
    public class UserSideSurveyTebakGambarTypesModel
    {
        public string Question { get; set; }
    }
    public class UserSideSurveyTebakGambarContentModel
    {
        public int QuestionTypeId { get; set; }
        public List<UserSideSurveyTebakGambarPicturesModel> Choice { get; set; }
    }
    public class UserSideSurveyTebakGambarModel
    {
        public int SurveyQuestionId { get; set; }

        public string TaskImageUrl { get; set; }
        public string Question { get; set; }
        public UserSideSurveyTebakGambarContentModel Content { get; set; }
    }
}
