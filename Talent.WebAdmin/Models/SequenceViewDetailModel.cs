using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class SequenceViewDetailModel
    {
        public TaskModel Task { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string BlobImageName { get; set; }

        public string BlobImageFileType { get; set; }

        public string ModuleName { get; set; }

        public string PrefixCode { get; set; }

        public int CompetencyTypeId { get; set; }

        public int EvaluationTypeId { get; set; }

        public string CompetencyTypeName { get; set; }

        public string EvaluationTypeName { get; set; }

        public List<SequenceChoiceModel> Choices { get; set; }

        public int TotalScore { get; set; }

        public int TotalPoint { get; set; }
    }
}
