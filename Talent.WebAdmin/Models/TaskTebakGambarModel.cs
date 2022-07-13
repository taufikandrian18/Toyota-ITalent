using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    

    public class TaskTebakGambarPicturesModel
    {
        public int TaskTebakGambarId { get; set; }
        public int TaskId { get; set; }
        public Guid? BlobId { get; set; }
        public int Number { get; set; }
        public FileContent ImageUpload { get; set; }
    }


    public class TaskTebakGambarTypesModel
    {
        public int TaskId { get; set; }
        public string Question { get; set; }
        public int Answer { get; set; }
        public int Score { get; set; }
    }

    public class TaskTebakGambarModel
    {
        public TaskInsertModel Task { get; set; }
        public TaskTebakGambarTypesModel Type { get; set; }
        public List<TaskTebakGambarPicturesModel> Picture { get; set; }
    }
}
