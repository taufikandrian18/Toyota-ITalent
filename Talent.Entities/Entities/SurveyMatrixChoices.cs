using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SurveyMatrixChoices
    {
        [Key]
        public int SurveyMatrixChoiceId { get; set; }
        public int? SurveyQuestionId { get; set; }
        [Required]
        [StringLength(64)]
        public string Text { get; set; }
        public int Number { get; set; }

        [ForeignKey("SurveyQuestionId")]
        [InverseProperty("SurveyMatrixChoices")]
        public virtual SurveyQuestions SurveyQuestion { get; set; }
    }
}
