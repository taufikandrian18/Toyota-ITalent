using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EventPositionMappings
    {
        public int EventId { get; set; }
        public int PositionId { get; set; }

        [ForeignKey("EventId")]
        [InverseProperty("EventPositionMappings")]
        public virtual Events Event { get; set; }
        [ForeignKey("PositionId")]
        [InverseProperty("EventPositionMappings")]
        public virtual Positions Position { get; set; }
    }
}
