using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class StagingDealerCompany
    {
        [StringLength(250)]
        public string Code { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [Column(TypeName = "decimal(38, 0)")]
        public decimal? DealerGroupId { get; set; }
        [Required]
        [StringLength(250)]
        public string State { get; set; }
        [Column("ID")]
        public int Id { get; set; }
    }
}
