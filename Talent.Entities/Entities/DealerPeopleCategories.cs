using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class DealerPeopleCategories
    {
        public DealerPeopleCategories()
        {
            Employees = new HashSet<Employees>();
            Roles = new HashSet<Roles>();
        }

        [Key]
        [StringLength(250)]
        public string DealerPeopleCategoryCode { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [InverseProperty("DealerPeopleCategoryCodeNavigation")]
        public virtual ICollection<Employees> Employees { get; set; }
        [InverseProperty("DealerPeopleCategoryCodeNavigation")]
        public virtual ICollection<Roles> Roles { get; set; }
    }
}
