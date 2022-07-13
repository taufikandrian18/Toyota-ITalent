using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class TaskHotSpotTypeModel
    {
        public int TaskId { get; set; }
        public Guid? BlobId { get; set; }
        public string Question { get; set; }
        public FileContent FileContent { get; set; }
    }

    public class TaskHotSpotAnswerModel
    {
        public int TaskHotSpotAnswerId { get; set; }
        public int TaskId { get; set; }
        public int Number { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
    }

    public class TaskHotSpotModel
    {
        public TaskInsertModel Task { get; set; }
        public TaskHotSpotTypeModel Type { get; set; }
        public List<TaskHotSpotAnswerModel> Choice { get; set; }
    }
}
