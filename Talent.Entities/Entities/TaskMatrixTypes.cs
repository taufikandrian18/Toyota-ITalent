using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskMatrixTypes
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public string Question { get; set; }
        public int ScoreColumn1 { get; set; }
        public int ScoreColumn2 { get; set; }
        public int ScoreColumn3 { get; set; }
        public int ScoreColumn4 { get; set; }
        public int ScoreColumn5 { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskMatrixTypes")]
        public virtual Tasks Task { get; set; }
    }
}
