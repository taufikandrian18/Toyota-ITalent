using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTaskFileUploadTypeModel
    {
        public string Question { get; set; }
    }

    public class UserSideTaskFileUploadContentModel
    {
        public int QuestionTypeId { get; set; }
    }
    public class UserSideTaskFileUploadModel
    {
        public int TaskId { get; set; }
        public string TaskImageUrl { get; set; }
        public string Question { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        public UserSideTaskFileUploadContentModel Content { get; set; }
    }
}
