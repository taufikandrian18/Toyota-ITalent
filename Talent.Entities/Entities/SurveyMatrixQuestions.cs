using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SurveyMatrixQuestions
    {
        [Key]
        public int SurveyMatrixQuestionId { get; set; }
        public int? SurveyQuestionId { get; set; }
        public int? Number { get; set; }
        [Required]
        [StringLength(64)]
        public string Question { get; set; }

        [ForeignKey("SurveyQuestionId")]
        [InverseProperty("SurveyMatrixQuestions")]
        public virtual SurveyQuestions SurveyQuestion { get; set; }
    }
}
