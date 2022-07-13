using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class Cms_Fmbs
    {
        public Cms_Fmbs()
        {
            ProductFeatureMappings = new HashSet<ProductFeatureMappings>();
        }
        [Key]
        public Guid Cms_FmbId { get; set; }
        public Guid? BlobId { get; set; }
        public Guid? CmsFmbFileBlobId { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string Cms_FmbDescription { get; set; }
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
        [InverseProperty("CmsFmbBlob")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("CmsFileBlobId")]
        [InverseProperty("CmsFmbFileBlob")]
        public virtual Blobs FmbFileBlob { get; set; }

        [InverseProperty("Cms_Fmb")]
        public virtual ICollection<ProductFeatureMappings> ProductFeatureMappings { get; set; }
    }
}
