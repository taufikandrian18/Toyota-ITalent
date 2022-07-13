using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class KeyPieceOfMindMenus
    {
        public KeyPieceOfMindMenus()
        {
            KeyPieceOfMinds = new HashSet<KeyPieceOfMinds>();
        }
        [Key]
        public Guid KeyPieceOfMindMenuId { get; set; }
        public string KeyPieceOfMindMenuName { get; set; }
        public int KeyPieceOfMindMenuSequence { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty("KeyPieceOfMindMenu")]
        public virtual ICollection<KeyPieceOfMinds> KeyPieceOfMinds { get; set; }
    }
}
