using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class Cms_Operations
    {
        public Cms_Operations()
        {
            ProductFeatureMappings = new HashSet<ProductFeatureMappings>();
        }
        [Key]
        public Guid Cms_OperationId { get; set; }
        public Guid? BlobId { get; set; }
        public Guid? Cms_OperationFileBlobId { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string Cms_OperationDescription { get; set; }
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
        [InverseProperty("CmsOperationBlob")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("Cms_OperationFileBlobId")]
        [InverseProperty("CmsOperationFileBlob")]
        public virtual Blobs OperationFileBlob { get; set; }

        [InverseProperty("Cms_Operation")]
        public virtual ICollection<ProductFeatureMappings> ProductFeatureMappings { get; set; }
    }
}
