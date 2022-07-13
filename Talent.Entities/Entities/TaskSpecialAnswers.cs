using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskSpecialAnswers
    {
        [Key]
        public int TaskSpecialAnswerId { get; set; }
        public int? TaskAnswerDetailId { get; set; }
        public int? Number { get; set; }
        public string Answer { get; set; }
        public int? Score { get; set; }
        public int? Point { get; set; }
        [Column("isTrue")]
        public bool? IsTrue { get; set; }

        [ForeignKey("TaskAnswerDetailId")]
        [InverseProperty("TaskSpecialAnswers")]
        public virtual TaskAnswerDetails TaskAnswerDetail { get; set; }
    }
}
