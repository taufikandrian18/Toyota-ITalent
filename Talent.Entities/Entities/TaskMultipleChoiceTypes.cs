using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskMultipleChoiceTypes
    {
        [Key]
        public int TaskId { get; set; }
        public int AnswerId { get; set; }
        [Required]
        public string Question { get; set; }
        public int Score { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskMultipleChoiceTypes")]
        public virtual Tasks Task { get; set; }
    }
}
