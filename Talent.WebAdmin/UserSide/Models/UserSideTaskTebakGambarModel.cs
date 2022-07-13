    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    

    public class UserSideTaskTebakGambarPicturesGetModel
    {
        public Guid BlobId { get; set; }
        public int Number { get; set; }
    }

    public class UserSideTaskTebakGambarPicturesModel
    {
        public string ImageUrl { get; set; }
        public int Number { get; set; }
    }


    public class UserSideTaskTebakGambarTypesModel
    {
        public string Question { get; set; }
        public int Answer { get; set; }
        public int Score { get; set; }
    }

    public class UserSideTaskTebakGambarContentModel
    {
        //Type
        public int QuestionTypeId { get; set; }
        public int Answer { get; set; }
        public int Point { get; set; }
        public int Score { get; set; }
        //Choice
        public List<UserSideTaskTebakGambarPicturesModel> Choice { get; set; }
    }

    public class UserSideTaskTebakGambarModel
    {
        //Task
        public int TaskId { get; set; }
        public string TaskImageUrl { get; set; }
        public int LayoutType { get; set; }
        public string Question { get; set; }
        public string StoryLineDescription { get; set; }
        //Content
        public UserSideTaskTebakGambarContentModel Content { get; set; }    }
}
