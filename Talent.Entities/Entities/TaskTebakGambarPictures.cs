using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskTebakGambarPictures
    {
        [Key]
        public int TaskTebakGambarId { get; set; }
        public int TaskId { get; set; }
        public Guid BlobId { get; set; }
        public int Number { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("TaskTebakGambarPictures")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("TaskId")]
        [InverseProperty("TaskTebakGambarPictures")]
        public virtual Tasks Task { get; set; }
    }
}
