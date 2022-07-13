using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class TaskMatrixTypeFormModel
    {
        public int TaskId { get; set; }
        public int CompetencyId { get; set; }
        public int QuestionTypeId { get; set; }
        public int ModuleId { get; set; }
        public Guid? BlobId { get; set; }
        public string FileName { get; set; }
        public string Mime { get; set; }
        public int TaskNumber { get; set; }
        public int LayoutType { get; set; }
        [StringLength(5000)]
        public string StoryLineDescription { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(255)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int EvaluationTypeId { get; set; }
        [Required]
        [StringLength(2000)]
        public string Question { get; set; }
        public int ScoreColumn1 { get; set; }
        public int ScoreColumn2 { get; set; }
        public int ScoreColumn3 { get; set; }
        public int ScoreColumn4 { get; set; }
        public int ScoreColumn5 { get; set; }

        public List<TaskMatrixChoiceFormModel> TaskMatrixChoices { get; set; }
        public List<TaskMatrixQuestionFormModel> TaskMatrixQuestions { get; set; }

        public FileContent ImageUpload { get; set; }
    }

    public class TaskMatrixChoiceFormModel
    {
        public int TaskMatrixChoiceId { get; set; }
        public int? TaskId { get; set; }
        [Required]
        [StringLength(64)]
        public string Text { get; set; }
        public int Number { get; set; }
    }

    public class TaskMatrixQuestionFormModel
    {
        public int? TaskId { get; set; }
        public int? Number { get; set; }
        [Required]
        [StringLength(64)]
        public string Question { get; set; }
    }
}
