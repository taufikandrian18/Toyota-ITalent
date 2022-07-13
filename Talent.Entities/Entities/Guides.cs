using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Guides
    {
        public Guides()
        {
            ApprovalToGuides = new HashSet<ApprovalToGuides>();
        }

        [Key]
        public int GuideId { get; set; }
        public int GuideTypeId { get; set; }
        public Guid BlobId { get; set; }
        [Required]
        [StringLength(1024)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(32)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("Guides")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("GuideTypeId")]
        [InverseProperty("Guides")]
        public virtual GuideTypes GuideType { get; set; }
        [InverseProperty("Guide")]
        public virtual ICollection<ApprovalToGuides> ApprovalToGuides { get; set; }
    }
}
