using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskFileUploadTypes
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public string Question { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskFileUploadTypes")]
        public virtual Tasks Task { get; set; }
    }
}
