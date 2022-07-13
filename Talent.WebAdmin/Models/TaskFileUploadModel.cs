using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class TaskFileUploadModel
    {
        public int TaskId { get; set; }
        public int CompetencyId { get; set; }
        public int QuestionTypeId { get; set; }
        public int ModuleId { get; set; }
        public Guid? BlobId { get; set; }
        public int TaskNumber { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int EvaluationTypeId { get; set; }
        public string Question { get; set; }

    }

    public class TaskFileUploadFormModel
    {
        public int? TaskId { get; set; }
        public int CompetencyId { get; set; }
        public int? QuestionTypeId { get; set; }
        public int ModuleId { get; set; }
        public Guid? BlobId { get; set; }
        public int? TaskNumber { get; set; }
        public int? LayoutType { get; set; }
        [StringLength(5000)]
        public string StoryLineDescription { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
         [StringLength(255)]
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? EvaluationTypeId { get; set; }
        [StringLength(2000)]
        public string Question { get; set; }
        public FileContent FileContent { get; set; }

    }

    public class TaskFileUploadViewDetail
    {
        public int TaskId { get; set; }
        public string Question { get; set; }
        public int CompetencyId { get; set; }
        public int QuestionTypeId { get; set; }
        public int ModuleId { get; set; }
        public Guid? BlobId { get; set; }
        public int TaskNumber { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int EvaluationTypeId { get; set; }
        public string CompetencyTypeName { get; set; }
        public string PrefixCode { get; set; }
        public string EvaluationTypeName { get; set; }
        public string ModuleName { get; set; }
        public string Name { get; set; }
        public string Mime { get; set; }
        public int CompetencyTypeId { get; set; }
    }
}
