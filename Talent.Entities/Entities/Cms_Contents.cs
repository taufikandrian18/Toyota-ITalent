using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class Cms_Contents
    {
        public Cms_Contents()
        {
            KeyPieceOfMinds = new HashSet<KeyPieceOfMinds>();
            Kodawaris = new HashSet<Kodawaris>();
            ProductFeatureMappings = new HashSet<ProductFeatureMappings>();
            ProductProgramMappings = new HashSet<ProductProgramMappings>();
            ProductSPWAMappings = new HashSet<ProductSPWAMappings>();
            ServiceTips = new HashSet<ServiceTips>();
        }
        [Key]
        public Guid Cms_ContentId { get; set; }
        public Guid? BlobId { get; set; }
        public Guid? CmsContentVideoBlobId { get; set; }
        public Guid? CmsContentFileBlobId { get; set; }
        public Guid? ProductId { get; set; }
        [StringLength(50)]
        public string Cms_ContentName { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string Cms_ContentDescription { get; set; }
        [Required]
        [StringLength(50)]
        public string Cms_ContentCategory { get; set; }
        public int? Cms_ContentSequence { get; set; }
        public bool? Cms_ContentIsSequence { get; set; }
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
        [InverseProperty("CmsContentBlob")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("CmsContentVideoBlobId")]
        [InverseProperty("CmsContentVideoBlob")]
        public virtual Blobs ContentVideoBlob { get; set; }
        [ForeignKey("CmsContentFileBlobId")]
        [InverseProperty("CmsContentFileBlob")]
        public virtual Blobs ContentFileBlob { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("Cms_Contents")]
        public virtual Products Product { get; set; }

        [InverseProperty("Cms_Content")]
        public virtual ICollection<KeyPieceOfMinds> KeyPieceOfMinds { get; set; }
        [InverseProperty("Cms_Content")]
        public virtual ICollection<Kodawaris> Kodawaris { get; set; }
        [InverseProperty("Cms_Content")]
        public virtual ICollection<ProductFeatureMappings> ProductFeatureMappings { get; set; }
        [InverseProperty("Cms_Content")]
        public virtual ICollection<ProductProgramMappings> ProductProgramMappings { get; set; }
        [InverseProperty("Cms_Content")]
        public virtual ICollection<ProductSPWAMappings> ProductSPWAMappings { get; set; }
        [InverseProperty("Cms_Content")]
        public virtual ICollection<ServiceTips> ServiceTips { get; set; }
    }
}
