using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class TaskMultipleChoiceTypeModel
    {
        public int TaskId { get; set; }
        public int AnswerId { get; set; }
        public string Question { get; set; }
        public int Score { get; set; }
    }

    public class TaskMultipleChoiceChoiceModel
    {
        public int TaskMultipleChoiceChoiceId { get; set; }
        public int TaskId { get; set; }
        public int Number { get; set; }
        public string Text { get; set; }
    }

    public class TaskMultipleChoiceModel
    {
        public TaskInsertModel Task { get; set; }
        public TaskMultipleChoiceTypeModel Type { get; set; }
        public List<TaskMultipleChoiceChoiceModel> Choice { get; set; }
    }
}
