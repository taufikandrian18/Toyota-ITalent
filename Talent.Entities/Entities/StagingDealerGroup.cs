using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class StagingDealerGroup
    {
        [StringLength(250)]
        public string Code { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(3)]
        public string DealerGroupCode { get; set; }
        [Required]
        [StringLength(250)]
        public string State { get; set; }
        [Column("ID")]
        public int Id { get; set; }
    }
}
