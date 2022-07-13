using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskMatrixChoices
    {
        [Key]
        public int TaskMatrixChoiceId { get; set; }
        public int? TaskId { get; set; }
        [Required]
        [StringLength(2000)]
        public string Text { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskMatrixChoices")]
        public virtual Tasks Task { get; set; }
    }
}
