using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class PrivilegePageMappings
    {
        public int PrivilegePageMappingsId { get; set; }
        public int RoleId { get; set; }
        [Required]
        [StringLength(8)]
        public string PageId { get; set; }
        public bool IsCreate { get; set; }
        public bool IsRead { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }

        [ForeignKey("PageId")]
        [InverseProperty("PrivilegePageMappings")]
        public virtual Pages Page { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("PrivilegePageMappings")]
        public virtual Roles Role { get; set; }
    }
}
