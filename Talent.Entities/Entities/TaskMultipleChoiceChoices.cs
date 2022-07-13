using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskMultipleChoiceChoices
    {
        [Key]
        public int TaskMultipleChoiceChoiceId { get; set; }
        public int TaskId { get; set; }
        public int Number { get; set; }
        [Required]
        [StringLength(2000)]
        public string Text { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskMultipleChoiceChoices")]
        public virtual Tasks Task { get; set; }
    }
}
