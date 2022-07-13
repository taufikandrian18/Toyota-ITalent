using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskMatchingTypes
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        [StringLength(256)]
        public string Answer { get; set; }
        public int Score { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskMatchingTypes")]
        public virtual Tasks Task { get; set; }
    }
}
