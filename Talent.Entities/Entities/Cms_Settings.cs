using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class Cms_Settings
    {
        public Cms_Settings()
        {
            ProductFeatureMappings = new HashSet<ProductFeatureMappings>();
        }
        [Key]
        public Guid Cms_SettingId { get; set; }
        public Guid? BlobId { get; set; }
        public Guid? Cms_SettingFileBlobId { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string Cms_SettingDescription { get; set; }
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
        [InverseProperty("CmsSettingBlob")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("Cms_SettingFileBlobId")]
        [InverseProperty("CmsSettingFileBlob")]
        public virtual Blobs SettingFileBlob { get; set; }

        [InverseProperty("Cms_Setting")]
        public virtual ICollection<ProductFeatureMappings> ProductFeatureMappings { get; set; }
    }
}
