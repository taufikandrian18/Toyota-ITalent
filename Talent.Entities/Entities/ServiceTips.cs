using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ServiceTips
    {
        public ServiceTips()
        {
        }
        [Key]
        public Guid ServiceTipId { get; set; }
        public Guid ServiceTipTypeId { get; set; }
        public Guid ServiceTipMenuId { get; set; }
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

        [ForeignKey("ServiceTipTypeId")]
        [InverseProperty("ServiceTips")]
        public virtual ServiceTipTypes ServiceTipType { get; set; }

        [ForeignKey("ServiceTipMenuId")]
        [InverseProperty("ServiceTips")]
        public virtual ServiceTipMenus ServiceTipMenu { get; set; }

        [ForeignKey("Cms_MenuId")]
        [InverseProperty("ServiceTips")]
        public virtual Cms_Menus Cms_Menu { get; set; }

        [ForeignKey("Cms_ContentId")]
        [InverseProperty("ServiceTips")]
        public virtual Cms_Contents Cms_Content { get; set; }

        [ForeignKey("Cms_SubContentId")]
        [InverseProperty("ServiceTips")]
        public virtual Cms_SubContents Cms_SubContent { get; set; }
    }
}
