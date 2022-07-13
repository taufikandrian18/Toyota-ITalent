using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTaskMultipleChoiceTypeModel
    {
        public int AnswerId { get; set; }
        public string Question { get; set; }
        public int Score { get; set; }
    }
    public class UserSideTaskMultipleChoiceChoiceModel
    {
        public int TaskMultipleChoiceChoiceId { get; set; }
        //public int TaskId { get; set; }
        public int Number { get; set; }
        public string Text { get; set; }
    }

    public class UserSideTaskMultipleChoiceContentModel {

        public int QuestionTypeId { get; set; }
        public int Answer { get; set; }
        public int Score { get; set; }
        public int Point { get; set; }
        public List<UserSideTaskMultipleChoiceChoiceModel> Choice { get; set; }
    }
    public class UserSideTaskMultipleChoiceModel
    {
        public int TaskId { get; set; }
        public string TaskImageUrl { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        public string Question { get; set; }
        //Content
        public UserSideTaskMultipleChoiceContentModel Content { get; set; }

    }
}
