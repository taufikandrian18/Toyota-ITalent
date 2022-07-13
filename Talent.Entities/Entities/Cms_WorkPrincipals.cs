using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class Cms_WorkPrincipals
    {
        public Cms_WorkPrincipals()
        {
            ProductFeatureMappings = new HashSet<ProductFeatureMappings>();
        }
        [Key]
        public Guid Cms_WorkPrincipalId { get; set; }
        public Guid? BlobId { get; set; }
        public Guid? Cms_WorkPrincipalFileBlobId { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string Cms_WorkPrincipalDescription { get; set; }
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
        [InverseProperty("CmsWorkPrincipalBlob")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("Cms_WorkPrincipalFileBlobId")]
        [InverseProperty("CmsWorkPrincipalFileBlob")]
        public virtual Blobs WorkPrincipalFileBlob { get; set; }

        [InverseProperty("Cms_WorkPrincipal")]
        public virtual ICollection<ProductFeatureMappings> ProductFeatureMappings { get; set; }
    }
}
