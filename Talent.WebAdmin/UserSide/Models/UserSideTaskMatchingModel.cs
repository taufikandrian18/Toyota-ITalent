using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTaskMatchingTypeModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
    }

    public class UserSideTaskMatchingChoiceGetModel
    {
        public Guid? BlobId { get; set; }
        public string TaskMatchingChoiceCode { get; set; }
        public string Text { get; set; }
    }

    public class UserSideTaskMatchingChoiceModel
    {
        public string ImageUrl { get; set; }
        public int Number { get; set; }
        public string Text { get; set; }
    }

    public class UserSideTaskMatchingLeftRightChoiceModel
    {
        public UserSideTaskMatchingChoiceModel Left { get; set; }
        public UserSideTaskMatchingChoiceModel Right { get; set; }
    }

    public class UserSideTaskMatchingContentModel
    {
        //Type
        public int QuestionTypeId { get; set; }
        public string Answer { get; set; }
        public int Point { get; set; }
        public int Score { get; set; }
        //Choice
        public List<UserSideTaskMatchingLeftRightChoiceModel> Choice { get; set; }
    }

    public class UserSideTaskMatchingModel
    {
        //Task
        public int TaskId { get; set; }
        public string Question { get; set; }
        public string TaskImageUrl { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        //Content
        public UserSideTaskMatchingContentModel Content { get; set; }
    }
}
