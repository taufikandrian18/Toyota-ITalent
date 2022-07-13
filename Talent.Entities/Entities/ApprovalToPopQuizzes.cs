using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalToPopQuizzes
    {
        [Key]
        public int ApprovalId { get; set; }
        public int PopQuizId { get; set; }

        [ForeignKey("ApprovalId")]
        [InverseProperty("ApprovalToPopQuizzes")]
        public virtual Approvals Approval { get; set; }
        [ForeignKey("PopQuizId")]
        [InverseProperty("ApprovalToPopQuizzes")]
        public virtual PopQuizzes PopQuiz { get; set; }
    }
}
