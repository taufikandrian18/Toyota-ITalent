using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskTebakGambarTypes
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public string Question { get; set; }
        public int Answer { get; set; }
        public int Score { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskTebakGambarTypes")]
        public virtual Tasks Task { get; set; }
    }
}
