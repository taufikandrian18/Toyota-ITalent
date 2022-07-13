using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTaskSequenceTypeModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public class UserSideTaskSequenceChoiceGetModel
    {
        public int Number { get; set; }
        public string Text { get; set; }
        public int Score { get; set; }
    }

    public class UserSideTaskSequenceChoiceModel
    {
        public int Number { get; set; }
        public string Text { get; set; }
        public int Point { get; set; }
        public int Score { get; set; }
    }

    public class UserSideTaskSequenceContentModel
    {
        //Type
        public int QuestionTypeId { get; set; }

        public string Answer { get; set; }
        //Choice
        public List<UserSideTaskSequenceChoiceModel> Choice { get; set; }
    }

    public class UserSideTaskSequenceModel
    {
        //Task
        public int TaskId { get; set; }
        public string TaskImageUrl { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        public string Question { get; set; }


        //Content
        public UserSideTaskSequenceContentModel Content { get; set; }
    }
}
