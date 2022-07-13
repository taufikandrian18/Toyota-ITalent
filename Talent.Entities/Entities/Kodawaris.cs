using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class Kodawaris
    {
        public Kodawaris()
        {
        }
        [Key]
        public Guid KodawariId { get; set; }
        public Guid KodawariMenuId { get; set; }
        public Guid Cms_MenuId { get; set; }
        public Guid KodawariTypeId { get; set; }
        public Guid Cms_ContentId { get; set; }
        public Guid? Cms_SubContentId { get; set; }
        public int KodawariSequence { get; set; }
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

        [ForeignKey("KodawariMenuId")]
        [InverseProperty("Kodawaris")]
        public virtual KodawariMenus KodawariMenu { get; set; }

        [ForeignKey("Cms_MenuId")]
        [InverseProperty("Kodawaris")]
        public virtual Cms_Menus Cms_Menu { get; set; }

        [ForeignKey("KodawariTypeId")]
        [InverseProperty("Kodawaris")]
        public virtual KodawariTypes KodawariType { get; set; }

        [ForeignKey("Cms_ContentId")]
        [InverseProperty("Kodawaris")]
        public virtual Cms_Contents Cms_Content { get; set; }

        [ForeignKey("Cms_SubContentId")]
        [InverseProperty("Kodawaris")]
        public virtual Cms_SubContents Cms_SubContent { get; set; }
    }
}
