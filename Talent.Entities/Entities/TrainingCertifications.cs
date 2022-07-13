using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TrainingCertifications
    {
        public TrainingCertifications()
        {

        }

        [Key]
        public string GUID { get; set; }
        public int? TrainingId { get; set; }
        public int? RelatedTrainingId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        [ForeignKey("TrainingId")]
        [InverseProperty("TrainingCertifications")]
        public virtual Trainings Training { get; set; }

        [ForeignKey("RelatedTrainingId")]
        [InverseProperty("RelatedTrainingCertifications")]
        public virtual Trainings RelatedTraining { get; set; }
    }
}
