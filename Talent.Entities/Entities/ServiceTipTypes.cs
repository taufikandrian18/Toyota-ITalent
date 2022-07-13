using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ServiceTipTypes
    {
        public ServiceTipTypes()
        {
            ServiceTips = new HashSet<ServiceTips>();
        }
        [Key]
        public Guid ServiceTipTypeId { get; set; }
        public Guid? BlobId { get; set; }
        [Required]
        [StringLength(50)]
        public string ServiceTipTypeName { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string ServiceTipTypeDescription { get; set; }
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
        [InverseProperty("ServiceTipTypes")]
        public virtual Blobs Blob { get; set; }

        [InverseProperty("ServiceTipType")]
        public virtual ICollection<ServiceTips> ServiceTips { get; set; }
    }
}
