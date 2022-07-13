using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductCompetitorMappings
    {
        [Key]
        public Guid ProductCompetitorMappingId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductTypeId { get; set; }
        public Guid ProductCompetitorId { get; set; }
        public Guid ProductCompetitorTypeId { get; set; }
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

        [ForeignKey("ProductId")]
        [InverseProperty("ProductMappings")]
        public virtual Products Product { get; set; }
        [ForeignKey("ProductTypeId")]
        [InverseProperty("ProductTypeMappings")]
        public virtual ProductTypes ProductType { get; set; }
        [ForeignKey("ProductCompetitorId")]
        [InverseProperty("ProductCompetitorMappings")]
        public virtual Products ProductCompetitor { get; set; }
        [ForeignKey("ProductCompetitorTypeId")]
        [InverseProperty("ProductTypeCompetitorMappings")]
        public virtual ProductTypes ProductCompetitorType { get; set; }
    }
}
