using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskRatingTypes
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public string Question { get; set; }
        public int Score0To20 { get; set; }
        public int Score21To40 { get; set; }
        public int Score41To60 { get; set; }
        public int Score61To80 { get; set; }
        public int Score81To100 { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskRatingTypes")]
        public virtual Tasks Task { get; set; }
    }
}
