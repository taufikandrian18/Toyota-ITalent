using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class HOGuidelines
    {
        public HOGuidelines()
        {
        }
        [Key]
        public Guid HOGuidelineId { get; set; }
        public Guid BlobId { get; set; }
        [Required]
        [StringLength(50)]
        public string HOGuidelineTitle { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string HOGuidelineComment { get; set; }
        [StringLength(50)]
        public string HOGuideLineStatus { get; set; }
        public bool IsApproved { get; set; }
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

        [ForeignKey("BlobId")]
        [InverseProperty("HOGuidelines")]
        public virtual Blobs Blob { get; set; }
    }
}
