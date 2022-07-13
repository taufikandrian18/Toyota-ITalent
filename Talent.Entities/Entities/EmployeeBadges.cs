using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EmployeeBadges
    {
        [Key]
        public int EmployeeTopicMappingId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int? TopicId { get; set; }
        [Column("EBadgeId")]
        public int? EbadgeId { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("EbadgeId")]
        [InverseProperty("EmployeeBadges")]
        public virtual Ebadges Ebadge { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("EmployeeBadges")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("TopicId")]
        [InverseProperty("EmployeeBadges")]
        public virtual Topics Topic { get; set; }
    }
}
