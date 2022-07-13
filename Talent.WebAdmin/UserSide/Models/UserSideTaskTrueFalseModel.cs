using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTaskTrueFalseTypeModel
    {
        public string Question { get; set; }
        public bool Answer { get; set; }
        public int Score { get; set; }

    }

    public class UserSideTaskTrueFalseContentModel
    {
        public int QuestionTypeId { get; set; }
        public bool Answer { get; set; }
        public int Point { get; set; }
        public int Score { get; set; }
    }

    public class UserSideTaskTrueFalseModel
    {
        public int TaskId { get; set; }
        public string TaskImageUrl { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        public string Question { get; set; }
        public UserSideTaskTrueFalseContentModel Content { get; set; }
    }

}
