using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{

    public class UserSideTaskRatingTypeModel
    {
        public string Question { get; set; }
        public int Score0To20 { get; set; }
        public int Score21To40 { get; set; }
        public int Score41To60 { get; set; }
        public int Score61To80 { get; set; }
        public int Score81To100 { get; set; }
    }
    public class UserSideTaskRatingChoicesModel
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }

    public class UserSideTaskRatingContentModel
    {
        //Type
        public int QuestionTypeId { get; set; }
        public int Score0To20 { get; set; }
        public int Score21To40 { get; set; }
        public int Score41To60 { get; set; }
        public int Score61To80 { get; set; }
        public int Score81To100 { get; set; }
        public int Point0To20 { get; set; }
        public int Point21To40 { get; set; }
        public int Point41To60 { get; set; }
        public int Point61To80 { get; set; }
        public int Point81To100 { get; set; }
        //Choice
        public List<UserSideTaskRatingChoicesModel> Choice { get; set; }
    }

    public class UserSideTaskRatingModel
    {
        //Task
        public int TaskId { get; set; }
        public string TaskImageUrl { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        public string Question { get; set; }
        public UserSideTaskRatingContentModel Content { get; set; }
    }
}
