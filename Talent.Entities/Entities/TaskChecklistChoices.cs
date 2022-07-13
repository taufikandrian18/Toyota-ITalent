using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskChecklistChoices
    {
        [Key]
        public int TaskChecklistChoiceId { get; set; }
        public int TaskId { get; set; }
        public int Number { get; set; }
        [Required]
        [StringLength(2000)]
        public string Text { get; set; }
        public bool IsAnswer { get; set; }
        public int Score { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskChecklistChoices")]
        public virtual Tasks Task { get; set; }
    }
}
