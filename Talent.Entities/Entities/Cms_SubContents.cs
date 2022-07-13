using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class Cms_SubContents
    {
        public Cms_SubContents()
        {
            KeyPieceOfMinds = new HashSet<KeyPieceOfMinds>();
            Kodawaris = new HashSet<Kodawaris>();
            ServiceTips = new HashSet<ServiceTips>();
        }
        [Key]
        public Guid Cms_SubContentId { get; set; }
        public Guid? BlobId { get; set; }
        public Guid? CmsSubContentVideoBlobId { get; set; }
        public Guid? CmsSubContentFileBlobId { get; set; }
        [StringLength(50)]
        public string Cms_SubContentName { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string Cms_SubContentDescription { get; set; }
        [Required]
        [StringLength(50)]
        public string Cms_SubContentCategory { get; set; }
        public int? Cms_SubContentSequence { get; set; }
        public bool? Cms_SubContentIsSequence { get; set; }
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
        [InverseProperty("CmsSubContentBlob")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("CmsSubContentVideoBlobId")]
        [InverseProperty("CmsSubContentVideoBlob")]
        public virtual Blobs SubContentVideoBlob { get; set; }
        [ForeignKey("CmsSubContentFileBlobId")]
        [InverseProperty("CmsSubContentFileBlob")]
        public virtual Blobs SubContentFileBlob { get; set; }

        [InverseProperty("Cms_SubContent")]
        public virtual ICollection<KeyPieceOfMinds> KeyPieceOfMinds { get; set; }
        [InverseProperty("Cms_SubContent")]
        public virtual ICollection<Kodawaris> Kodawaris { get; set; }
        [InverseProperty("Cms_SubContent")]
        public virtual ICollection<ServiceTips> ServiceTips { get; set; }
    }
}
