using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalToTrainings
    {
        [Key]
        public int ApprovalId { get; set; }
        public int TrainingId { get; set; }

        [ForeignKey("ApprovalId")]
        [InverseProperty("ApprovalToTrainings")]
        public virtual Approvals Approval { get; set; }
        [ForeignKey("TrainingId")]
        [InverseProperty("ApprovalToTrainings")]
        public virtual Trainings Training { get; set; }
    }
}
