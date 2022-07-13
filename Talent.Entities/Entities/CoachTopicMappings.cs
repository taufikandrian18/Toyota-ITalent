using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class CoachTopicMappings
    {
        public int CoachId { get; set; }
        public int TopicId { get; set; }

        [ForeignKey("CoachId")]
        [InverseProperty("CoachTopicMappings")]
        public virtual Coaches Coach { get; set; }
        [ForeignKey("TopicId")]
        [InverseProperty("CoachTopicMappings")]
        public virtual Topics Topic { get; set; }
    }
}
