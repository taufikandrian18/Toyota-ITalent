using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Roles
    {
        public Roles()
        {
            PrivilegePageMappings = new HashSet<PrivilegePageMappings>();
        }

        [Key]
        public int RoleId { get; set; }
        public int PositionId { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsTamPeople { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(32)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        [StringLength(250)]
        public string DealerPeopleCategoryCode { get; set; }

        [ForeignKey("DealerPeopleCategoryCode")]
        [InverseProperty("Roles")]
        public virtual DealerPeopleCategories DealerPeopleCategoryCodeNavigation { get; set; }
        [ForeignKey("PositionId")]
        [InverseProperty("Roles")]
        public virtual Positions Position { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<PrivilegePageMappings> PrivilegePageMappings { get; set; }
    }
}
