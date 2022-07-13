using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ModuleTopicMappings
    {
        public int ModuleId { get; set; }
        public int TopicId { get; set; }

        [ForeignKey("ModuleId")]
        [InverseProperty("ModuleTopicMappings")]
        public virtual Modules Module { get; set; }
        [ForeignKey("TopicId")]
        [InverseProperty("ModuleTopicMappings")]
        public virtual Topics Topic { get; set; }
    }
}
