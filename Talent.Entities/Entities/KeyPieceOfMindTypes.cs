using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class KeyPieceOfMindTypes
    {
        public KeyPieceOfMindTypes()
        {
            KeyPieceOfMinds = new HashSet<KeyPieceOfMinds>();
        }
        [Key]
        public Guid KeyPieceOfMindTypeId { get; set; }
        public Guid? BlobId { get; set; }
        [Required]
        [StringLength(50)]
        public string KeyPieceOfMindTypeName { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string KeyPieceOfMindTypeDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("KeyPieceOfMindTypes")]
        public virtual Blobs Blob { get; set; }

        [InverseProperty("KeyPieceOfMindType")]
        public virtual ICollection<KeyPieceOfMinds> KeyPieceOfMinds { get; set; }
    }
}
