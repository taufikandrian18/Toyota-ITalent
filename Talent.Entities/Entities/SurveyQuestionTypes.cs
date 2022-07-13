using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SurveyQuestionTypes
    {
        public SurveyQuestionTypes()
        {
            SurveyQuestions = new HashSet<SurveyQuestions>();
        }

        [Key]
        public int SurveyQuestionTypeId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [InverseProperty("SurveyQuestionType")]
        public virtual ICollection<SurveyQuestions> SurveyQuestions { get; set; }
    }
}
