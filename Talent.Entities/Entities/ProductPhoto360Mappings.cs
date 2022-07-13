using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class ProductPhoto360Mappings
    {
        public Guid ProductId { get; set; }
        public Guid ProductPhotoId { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("ProductPhoto360Mappings")]
        public virtual Products Product { get; set; }
        [ForeignKey("ProductPhotoId")]
        [InverseProperty("ProductPhoto360Mappings")]
        public virtual ProductPhotos ProductPhoto { get; set; }
    }
}
