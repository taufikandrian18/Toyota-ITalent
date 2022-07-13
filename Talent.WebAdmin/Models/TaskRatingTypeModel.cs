using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class TaskRatingTypeModel
    {
        public int TaskId { get; set; }
        public string Question { get; set; }
        public int Score0To20 { get; set; }
        public int Score21To40 { get; set; }
        public int Score41To60 { get; set; }
        public int Score61To80 { get; set; }
        public int Score81To100 { get; set; }
    }

    public class TaskRatingChoicesModel
    {
        public int TaskRatingChoiceId { get; set; }
        public int TaskId { get; set; }
        public int Number { get; set; }
        public string Text { get; set; }
    }


    public class TaskRatingModel
    {
        public TaskInsertModel Task { get; set; }
        public TaskRatingTypeModel Type { get; set; }
        public List<TaskRatingChoicesModel> Choice { get; set; }
    }
}
