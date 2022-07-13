using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ServiceTipMenus
    {
        public ServiceTipMenus()
        {
            ServiceTips = new HashSet<ServiceTips>();
        }
        [Key]
        public Guid ServiceTipMenuId { get; set; }
        public string ServiceTipMenuName { get; set; }
        public int ServiceTipMenuSequence { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty("ServiceTipMenu")]
        public virtual ICollection<ServiceTips> ServiceTips { get; set; }
    }
}
