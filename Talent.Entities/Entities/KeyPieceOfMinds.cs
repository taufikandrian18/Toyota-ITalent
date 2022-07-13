using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class KeyPieceOfMinds
    {
        public KeyPieceOfMinds()
        {
        }
        [Key]
        public Guid KeyPieceOfMindId { get; set; }
        public Guid KeyPieceOfMindTypeId { get; set; }
        public Guid KeyPieceOfMindMenuId { get; set; }
        public Guid Cms_MenuId { get; set; }
        public Guid Cms_ContentId { get; set; }
        public Guid? Cms_SubContentId { get; set; }
        public int IsSequence { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("KeyPieceOfMindTypeId")]
        [InverseProperty("KeyPieceOfMinds")]
        public virtual KeyPieceOfMindTypes KeyPieceOfMindType { get; set; }

        [ForeignKey("KeyPieceOfMindMenuId")]
        [InverseProperty("KeyPieceOfMinds")]
        public virtual KeyPieceOfMindMenus KeyPieceOfMindMenu { get; set; }

        [ForeignKey("Cms_MenuId")]
        [InverseProperty("KeyPieceOfMinds")]
        public virtual Cms_Menus Cms_Menu { get; set; }

        [ForeignKey("Cms_ContentId")]
        [InverseProperty("KeyPieceOfMinds")]
        public virtual Cms_Contents Cms_Content { get; set; }

        [ForeignKey("Cms_SubContentId")]
        [InverseProperty("KeyPieceOfMinds")]
        public virtual Cms_SubContents Cms_SubContent { get; set; }
    }
}
