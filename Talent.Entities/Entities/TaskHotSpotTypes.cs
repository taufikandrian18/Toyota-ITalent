using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskHotSpotTypes
    {
        [Key]
        public int TaskId { get; set; }
        public Guid BlobId { get; set; }
        [Required]
        public string Question { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("TaskHotSpotTypes")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("TaskId")]
        [InverseProperty("TaskHotSpotTypes")]
        public virtual Tasks Task { get; set; }
    }
}
