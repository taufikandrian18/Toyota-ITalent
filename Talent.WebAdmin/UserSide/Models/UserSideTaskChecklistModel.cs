using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTaskChecklistTypeModel
    {
        public string Question { get; set; }
    }
    //DATA DARI DB 
    //datanya itu berbentuk list
    public class UserSideTaskChecklistChoiceGetModel
    {
        public int Number { get; set; }
        public string Text { get; set; }
        public bool IsAnswer { get; set; }
        public int Score { get; set; }
    }

    //DATA YANG DIBUTUHKAN OLEH MOBILE
    //datanya itu berbentuk list
    public class UserSideTaskChecklistChoiceModel
    {
        public int Number { get; set; }
        public string Text { get; set; }
        public bool IsAnswer { get; set; }
        public int Point { get; set; }
        public int Score { get; set; }
    }

    public class UserSideTaskChecklistContentModel
    {
        //Type
        public int QuestionTypeId { get; set; }
        //Choice
        public List<UserSideTaskChecklistChoiceModel> Choice { get; set; }
    }

    public class UserSideTaskChecklistModel
    {
        //Task
        public int TaskId { get; set; }
        public string TaskImageUrl { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        public string Question { get; set; }
        //Content
        public UserSideTaskChecklistContentModel Content { get; set;}
        
    }
}
