using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskHotSpotAnswers
    {
        [Key]
        public int TaskHotSpotAnswerId { get; set; }
        public int TaskId { get; set; }
        public int Number { get; set; }
        [Required]
        [StringLength(2000)]
        public string Answer { get; set; }
        public int Score { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskHotSpotAnswers")]
        public virtual Tasks Task { get; set; }
    }
}
