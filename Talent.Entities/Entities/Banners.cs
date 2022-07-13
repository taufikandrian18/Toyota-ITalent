using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Banners
    {
        public Banners()
        {
            ApprovalToBanners = new HashSet<ApprovalToBanners>();
        }

        [Key]
        public int BannerId { get; set; }
        public int BannerTypeId { get; set; }
        public Guid BlobId { get; set; }
        [Required]
        [StringLength(1024)]
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MobilePageId { get; set; }
        public int? ContentId { get; set; }
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
        public bool IsDeleted { get; set; }

        [ForeignKey("BannerTypeId")]
        [InverseProperty("Banners")]
        public virtual BannerTypes BannerType { get; set; }
        [ForeignKey("BlobId")]
        [InverseProperty("Banners")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("MobilePageId")]
        [InverseProperty("Banners")]
        public virtual MobilePages MobilePage { get; set; }
        [InverseProperty("Banner")]
        public virtual ICollection<ApprovalToBanners> ApprovalToBanners { get; set; }
    }
}
