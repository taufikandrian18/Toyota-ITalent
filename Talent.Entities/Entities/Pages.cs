using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Pages
    {
        public Pages()
        {
            ApprovalMappings = new HashSet<ApprovalMappings>();
            PrivilegePageMappings = new HashSet<PrivilegePageMappings>();
        }

        [Key]
        [StringLength(8)]
        public string PageId { get; set; }
        [StringLength(8)]
        public string MenuId { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
        public bool NeedApproval { get; set; }

        [ForeignKey("MenuId")]
        [InverseProperty("Pages")]
        public virtual Menus Menu { get; set; }
        [InverseProperty("Page")]
        public virtual ICollection<ApprovalMappings> ApprovalMappings { get; set; }
        [InverseProperty("Page")]
        public virtual ICollection<PrivilegePageMappings> PrivilegePageMappings { get; set; }
    }
}
