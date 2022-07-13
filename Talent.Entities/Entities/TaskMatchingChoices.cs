using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskMatchingChoices
    {
        [Key]
        public int TaskMatchingChoiceId { get; set; }
        public int TaskId { get; set; }
        public Guid? BlobId { get; set; }
        [Required]
        [StringLength(3)]
        public string TaskMatchingChoiceCode { get; set; }
        [StringLength(2000)]
        public string Text { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("TaskMatchingChoices")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("TaskId")]
        [InverseProperty("TaskMatchingChoices")]
        public virtual Tasks Task { get; set; }
    }
}
