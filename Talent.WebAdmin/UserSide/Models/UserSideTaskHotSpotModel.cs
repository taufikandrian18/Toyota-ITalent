using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTaskHotSpotTypeModel
    {
        public Guid BlobId { get; set; }
        public string Question { get; set; }
        public string ImageUrl { get; set; }
    }

    public class UserSideTaskHotSpotAnswerGetModel
    {
        public int Number { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
    }
    public class UserSideTaskHotSpotAnswerModel
    {
        public int Number { get; set; }
        public string Answer { get; set; }
        public int Point { get; set; }
        public int Score { get; set; }
    }

    public class UserSideTaskHotSpotContentModel
    {
        //Type
        public int QuestionTypeId { get; set; }
        public string ImageUrl { get; set; }
        //Choice
        public List<UserSideTaskHotSpotAnswerModel> Choice { get; set; }
    }

    public class UserSideTaskHotSpotModel
    {
        //Task
        public int TaskId { get; set; }
        public string TaskImageUrl { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        public string Question { get; set; }
        //Content
        public UserSideTaskHotSpotContentModel Content { get; set; }
    }
}
