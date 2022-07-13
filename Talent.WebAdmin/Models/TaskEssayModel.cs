using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class TaskEssayTypeFormModel
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
        public FileContent FileContent { get; set; }
    }
}
