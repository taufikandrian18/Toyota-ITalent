using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SurveySpecialAnswers
    {
        [Key]
        public int SurveySpecialAnswerId { get; set; }
        public int? SurveyAnswerDetailId { get; set; }
        public int? Number { get; set; }
        public string Answer { get; set; }

        [ForeignKey("SurveyAnswerDetailId")]
        [InverseProperty("SurveySpecialAnswers")]
        public virtual SurveyAnswerDetails SurveyAnswerDetail { get; set; }
    }
}
