using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TrainingPositionOnlyViewMappings
    {

        [Key]
        public string GUID {get;set;}
        public int TrainingId { get; set; }
        public int PositionId { get; set; }

        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

        // TO DO : ADD IN THIS ANOTHER ENTITIES 
        [ForeignKey("PositionId")]
        [InverseProperty("TrainingPositionOnlyViewMappings")]
        public virtual Positions Position { get; set; }
        [ForeignKey("TrainingId")]
        [InverseProperty("TrainingPositionOnlyViewMappings")]
        public virtual Trainings Training { get; set; }
    }
}
