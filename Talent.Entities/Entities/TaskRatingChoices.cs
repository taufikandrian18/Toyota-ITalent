using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskRatingChoices
    {
        [Key]
        public int TaskRatingChoiceId { get; set; }
        public int TaskId { get; set; }
        public int Number { get; set; }
        [Required]
        [StringLength(2000)]
        public string Text { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskRatingChoices")]
        public virtual Tasks Task { get; set; }
    }
}
