using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Topics
    {
        public Topics()
        {
            CoachTopicMappings = new HashSet<CoachTopicMappings>();
            EmployeeBadges = new HashSet<EmployeeBadges>();
            EmployeeInterests = new HashSet<EmployeeInterests>();
            ModuleTopicMappings = new HashSet<ModuleTopicMappings>();
        }

        [Key]
        public int TopicId { get; set; }
        [Column("EBadgeId")]
        public int? EbadgeId { get; set; }
        public Guid? BlobId { get; set; }
        [Required]
        [StringLength(255)]
        public string TopicName { get; set; }
        public int TopicMinimumPoints { get; set; }
        public string TopicDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("Topics")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("EbadgeId")]
        [InverseProperty("Topics")]
        public virtual Ebadges Ebadge { get; set; }
        [InverseProperty("Topic")]
        public virtual ICollection<CoachTopicMappings> CoachTopicMappings { get; set; }
        [InverseProperty("Topic")]
        public virtual ICollection<EmployeeBadges> EmployeeBadges { get; set; }
        [InverseProperty("Topic")]
        public virtual ICollection<EmployeeInterests> EmployeeInterests { get; set; }
        [InverseProperty("Topic")]
        public virtual ICollection<ModuleTopicMappings> ModuleTopicMappings { get; set; }
    }
}
