using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskMatrixQuestions
    {
        [Key]
        public int TaskMatrixQuestionId { get; set; }
        public int? TaskId { get; set; }
        public int? Number { get; set; }
        [Required]
        [StringLength(64)]
        public string Question { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskMatrixQuestions")]
        public virtual Tasks Task { get; set; }
    }
}
