using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TrainingOutletMappings
    {
        public int TrainingId { get; set; }
        [StringLength(64)]
        public string OutletId { get; set; }

        [ForeignKey("OutletId")]
        [InverseProperty("TrainingOutletMappings")]
        public virtual Outlets Outlet { get; set; }
        [ForeignKey("TrainingId")]
        [InverseProperty("TrainingOutletMappings")]
        public virtual Trainings Training { get; set; }
    }
}
