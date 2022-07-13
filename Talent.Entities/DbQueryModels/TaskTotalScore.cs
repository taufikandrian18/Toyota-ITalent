using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class TaskTotalScore
    {
        public int TaskId { get; set; }

        public string QuestionName { get; set; }

        public int TotalScore { get; set; }
    }
}
