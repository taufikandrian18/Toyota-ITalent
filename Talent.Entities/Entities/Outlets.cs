using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Outlets
    {
        public Outlets()
        {
            Employees = new HashSet<Employees>();
            EventOutletMappings = new HashSet<EventOutletMappings>();
            SurveyOutletMappings = new HashSet<SurveyOutletMappings>();
            TrainingOutletMappings = new HashSet<TrainingOutletMappings>();
            ProductTips = new HashSet<ProductTips>();
        }

        [Key]
        [StringLength(64)]
        public string OutletId { get; set; }
        [Required]
        [StringLength(64)]
        public string DealerId { get; set; }
        [Column("CFAMId")]
        [StringLength(32)]
        public string Cfamid { get; set; }
        [Required]
        [StringLength(32)]
        public string AreaId { get; set; }
        [StringLength(32)]
        public string AreaSpecialistId { get; set; }
        [Required]
        [StringLength(32)]
        public string ProvinceId { get; set; }
        [Required]
        [StringLength(32)]
        public string CityId { get; set; }
        public int? RegionId { get; set; }
        [Required]
        [StringLength(64)]
        public string OutletCode { get; set; }
        [Required]
        public string Name { get; set; }
        [StringLength(255)]
        public string Phone { get; set; }
        [Column("IsBP")]
        public bool IsBp { get; set; }
        [Column("IsGR")]
        public bool IsGr { get; set; }
        public bool IsSales { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }

        [ForeignKey("AreaId")]
        [InverseProperty("Outlets")]
        public virtual Areas Area { get; set; }
        [ForeignKey("AreaSpecialistId")]
        [InverseProperty("Outlets")]
        public virtual AreaSpecialists AreaSpecialist { get; set; }
        [ForeignKey("Cfamid")]
        [InverseProperty("Outlets")]
        public virtual Cfams Cfam { get; set; }
        [ForeignKey("CityId")]
        [InverseProperty("Outlets")]
        public virtual Cities City { get; set; }
        [ForeignKey("DealerId")]
        [InverseProperty("Outlets")]
        public virtual Dealers Dealer { get; set; }
        [ForeignKey("ProvinceId")]
        [InverseProperty("Outlets")]
        public virtual Provinces Province { get; set; }
        [ForeignKey("RegionId")]
        [InverseProperty("Outlets")]
        public virtual Regions Region { get; set; }
        [InverseProperty("Outlet")]
        public virtual ICollection<Employees> Employees { get; set; }
        [InverseProperty("Outlet")]
        public virtual ICollection<EventOutletMappings> EventOutletMappings { get; set; }
        [InverseProperty("Outlet")]
        public virtual ICollection<SurveyOutletMappings> SurveyOutletMappings { get; set; }
        [InverseProperty("Outlet")]
        public virtual ICollection<TrainingOutletMappings> TrainingOutletMappings { get; set; }
        [InverseProperty("Outlet")]
        public virtual ICollection<ProductTips> ProductTips { get; set; }
    }
}
