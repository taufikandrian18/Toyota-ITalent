using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EventOutletMappings
    {
        public int EventId { get; set; }
        [StringLength(64)]
        public string OutletId { get; set; }

        [ForeignKey("EventId")]
        [InverseProperty("EventOutletMappings")]
        public virtual Events Event { get; set; }
        [ForeignKey("OutletId")]
        [InverseProperty("EventOutletMappings")]
        public virtual Outlets Outlet { get; set; }
    }
}
