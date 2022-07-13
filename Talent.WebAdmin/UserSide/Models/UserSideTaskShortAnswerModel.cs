using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTaskShortAnswerTypeModel
    {
        public string Question { get; set; }
    }

    public class UserSideTaskShortAnswerContentModel
    {
        public int QuestionTypeId { get; set; }
    }

    public class UserSideTaskShortAnswerModel
    {
        //Task
        public int TaskId { get; set; }
        public string TaskImageUrl { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        public string Question { get; set; }
        //Content
        public UserSideTaskShortAnswerContentModel Content { get; set; }
    }
}
