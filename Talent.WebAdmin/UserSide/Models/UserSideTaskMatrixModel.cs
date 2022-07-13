using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{

    public class UserSideTaskMatrixTypeGetModel
    {
        public string Question { get; set; }
        public int ScoreColumn1 { get; set; }
        public int ScoreColumn2 { get; set; }
        public int ScoreColumn3 { get; set; }
        public int ScoreColumn4 { get; set; }
        public int ScoreColumn5 { get; set; }
    }

    public class UserSideTaskMatrixTypeModel
    {
        public string Question { get; set; }
        public int PointColumn1 { get; set; }
        public int PointColumn2 { get; set; }
        public int PointColumn3 { get; set; }
        public int PointColumn4 { get; set; }
        public int PointColumn5 { get; set; }
    }

    public class UserSideTaskMatrixQuestionsModel
    {
        public int? Number { get; set; }
        public string Question { get; set; }
    }

    //public class UserSideTaskMatrixChoicesModel
    //{
    //    public string Text { get; set; }

    //}

    public class UserSideTaskMatrixContentModel
    {
        public int QuestionTypeId { get; set; }
        public int PointColumn1 { get; set; }
        public int PointColumn2 { get; set; }
        public int PointColumn3 { get; set; }
        public int PointColumn4 { get; set; }
        public int PointColumn5 { get; set; }

        public int ScoreColumn1 { get; set; }
        public int ScoreColumn2 { get; set; }
        public int ScoreColumn3 { get; set; }
        public int ScoreColumn4 { get; set; }
        public int ScoreColumn5 { get; set; }
        public List<string> Headers { get; set; }
        public List<UserSideTaskMatrixQuestionsModel> Questions { get; set; }
    }

    public class UserSideTaskMatrixModel
    {
        //Task
        public int TaskId { get; set; }
        public string TaskImageUrl { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        public string Question { get; set; }
        //Content
        public UserSideTaskMatrixContentModel Content { get; set; }
    }
}
