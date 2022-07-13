using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class News
    {
        public News()
        {
            ApprovalToNews = new HashSet<ApprovalToNews>();
        }

        public int NewsId { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        public string Description { get; set; }
        public int NewsCategoryId { get; set; }
        [StringLength(1024)]
        public string Link { get; set; }
        [StringLength(128)]
        public string Author { get; set; }
        public Guid ThumbnailBlobId { get; set; }
        public Guid? FileBlobId { get; set; }
        public bool IsDownloadable { get; set; }
        public bool IsInternal { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(255)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public int? TotalView { get; set; }

        [ForeignKey("FileBlobId")]
        [InverseProperty("NewsFileBlob")]
        public virtual Blobs FileBlob { get; set; }
        [ForeignKey("NewsCategoryId")]
        [InverseProperty("News")]
        public virtual NewsCategories NewsCategory { get; set; }
        [ForeignKey("ThumbnailBlobId")]
        [InverseProperty("NewsThumbnailBlob")]
        public virtual Blobs ThumbnailBlob { get; set; }
        [InverseProperty("News")]
        public virtual ICollection<ApprovalToNews> ApprovalToNews { get; set; }
    }
}
