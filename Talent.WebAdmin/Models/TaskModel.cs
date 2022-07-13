using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{

    public class TaskViewModel
    {
        public string TaskCode { get; set; }
        public int? QuestionTypeId { get; set; }
        public string ModuleName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAtDate { get; set; }
        public DateTime? UpdateAtDate { get; set; }
        public string UpdateAt { get; set; }
        public string CreatedAt { get; set; }
        public int TaskId { get; set; }
    }

    public class TaskModel
    {
        public int TaskId { get; set; }
        public int CompetencyId { get; set; }
        public int QuestionTypeId { get; set; }
        public int ModuleId { get; set; }
        public Guid? BlobId { get; set; }
        public int TaskNumber { get; set; }
        public int LayoutType { get; set; }
        [StringLength(5000)]
        public string StoryLineDescription { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int EvaluationTypeId { get; set; }
    }

    public class TaskFormModel
    {
        public int TaskId { get; set; }
        public int CompetencyId { get; set; }
        public int QuestionTypeId { get; set; }
        public int ModuleId { get; set; }
        public int TaskNumber { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(255)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class GetNumberTaskModel
    {
        public int CompetencyId { get; set; }
        public int ModuleId { get; set; }
        public int EvaluationTypeId { get; set; }
    }

    public class TaskInsertModel
    {
        public int TaskId { get; set; }
        public int CompetencyId { get; set; }
        public int QuestionTypeId { get; set; }
        public int ModuleId { get; set; }
        public Guid? BlobId { get; set; }
        public int TaskNumber { get; set; }
        public int LayoutType { get; set; }
        [StringLength(5000)]
        public string StoryLineDescription { get; set; }
        public int EvaluationTypeId { get; set; }
        public FileContent FileContent { get; set; }
    }

    public class TaskPaginationModel
    {
        public List<TaskViewModel> TaskData { get; set; }
        public int TotalData { get; set; }
    }

    public class TaskPagingModel : GridFilter
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string TaskCode { get; set; }
        public int? QuestionTypeId { get; set; }
        public string ModuleName { get; set; }
        public string CreatedBy { get; set; }
        public int TaskId { get; set; }
    }

}
