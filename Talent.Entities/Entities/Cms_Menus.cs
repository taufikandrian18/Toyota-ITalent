using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class Cms_Menus
    {
        public Cms_Menus()
        {
            KeyPieceOfMinds = new HashSet<KeyPieceOfMinds>();
            Kodawaris = new HashSet<Kodawaris>();
            ProductProgramMappings = new HashSet<ProductProgramMappings>();
            ProductSPWAMappings = new HashSet<ProductSPWAMappings>();
            ServiceTips = new HashSet<ServiceTips>();
        }
        [Key]
        public Guid Cms_MenuId { get; set; }
        [Required]
        [StringLength(50)]
        public string Cms_MenuName { get; set; }
        [Required]
        [StringLength(50)]
        public string Cms_MenuCategory { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }


        [InverseProperty("Cms_Menu")]
        public virtual ICollection<KeyPieceOfMinds> KeyPieceOfMinds { get; set; }
        [InverseProperty("Cms_Menu")]
        public virtual ICollection<Kodawaris> Kodawaris { get; set; }
        [InverseProperty("Cms_Menu")]
        public virtual ICollection<ProductProgramMappings> ProductProgramMappings { get; set; }
        [InverseProperty("Cms_Menu")]
        public virtual ICollection<ProductSPWAMappings> ProductSPWAMappings { get; set; }
        [InverseProperty("Cms_Menu")]
        public virtual ICollection<ServiceTips> ServiceTips { get; set; }
    }
}
