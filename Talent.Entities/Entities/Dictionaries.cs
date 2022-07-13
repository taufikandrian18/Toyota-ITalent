using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class Dictionaries
    {
        public Dictionaries()
        {
            DictionaryMappings = new HashSet<DictionaryMappings>();
        }
        [Key]
        public Guid DictionaryId { get; set; }
        public Guid BlobId { get; set; }
        [Required]
        [StringLength(50)]
        public string DictionaryName { get; set; }
        public bool DictionaryStatus { get; set; }
        public bool? IsFavorite { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string DictionaryArti { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string DictionaryManfaat { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string DictionaryFakta { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string DictionaryBasicOperation { get; set; }
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
        [InverseProperty("Dictionaries")]
        public virtual Blobs Blob { get; set; }

        [InverseProperty("Dictionary")]
        public virtual ICollection<DictionaryMappings> DictionaryMappings { get; set; }
    }
}
