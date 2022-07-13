using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class StagingOutlet
    {
        [StringLength(250)]
        public string Code { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [Column(TypeName = "decimal(38, 0)")]
        public decimal? DealerCompanyId { get; set; }
        [StringLength(5)]
        public string OutletFunctionId { get; set; }
        [StringLength(10)]
        public string OutletCode { get; set; }
        [StringLength(64)]
        public string PhoneNumber { get; set; }
        [Column(TypeName = "decimal(38, 0)")]
        public decimal? KabupatenId { get; set; }
        [Column(TypeName = "decimal(38, 0)")]
        public decimal? TamAreaId { get; set; }
        [Column(TypeName = "decimal(38, 0)")]
        public decimal? TamAreaAfterSalesId { get; set; }
        [Required]
        [StringLength(250)]
        public string State { get; set; }
        [Column("ID")]
        public int Id { get; set; }
        [Column(TypeName = "decimal(38, 0)")]
        public decimal RegionCode { get; set; }
    }
}
