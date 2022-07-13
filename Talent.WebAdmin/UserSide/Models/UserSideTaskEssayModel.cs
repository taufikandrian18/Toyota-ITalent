using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTaskEssayTypesModel
    {
        public string Question { get; set; }
    }
    public class UserSideTaskEssayContentModel
    {
        public int QuestionTypeId { get; set; }
    }

    public class UserSideTaskEssayModel
    {
        //Task
        public int TaskId { get; set; }
        public string Question { get; set; }
        public string TaskImageUrl { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        //Content
        public UserSideTaskEssayContentModel Content { get; set; }
    }
}
