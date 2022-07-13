using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalToSurveys
    {
        [Key]
        public int ApprovalId { get; set; }
        public int SurveyId { get; set; }

        [ForeignKey("ApprovalId")]
        [InverseProperty("ApprovalToSurveys")]
        public virtual Approvals Approval { get; set; }
        [ForeignKey("SurveyId")]
        [InverseProperty("ApprovalToSurveys")]
        public virtual Surveys Survey { get; set; }
    }
}
