using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SurveyAnswers
    {
        public SurveyAnswers()
        {
            SurveyAnswerDetails = new HashSet<SurveyAnswerDetails>();
        }

        [Key]
        public int SurveyAnswerId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int SurveyId { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("SurveyAnswers")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("SurveyId")]
        [InverseProperty("SurveyAnswers")]
        public virtual Surveys Survey { get; set; }
        [InverseProperty("SurveyAnswer")]
        public virtual ICollection<SurveyAnswerDetails> SurveyAnswerDetails { get; set; }
    }
}
