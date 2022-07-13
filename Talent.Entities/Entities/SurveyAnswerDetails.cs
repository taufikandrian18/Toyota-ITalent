using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SurveyAnswerDetails
    {
        public SurveyAnswerDetails()
        {
            SurveySpecialAnswers = new HashSet<SurveySpecialAnswers>();
        }

        [Key]
        public int SurveyAnswerDetailId { get; set; }
        public int SurveyAnswerId { get; set; }
        public int SurveyQuestionId { get; set; }
        [StringLength(255)]
        public string Answer { get; set; }
        public Guid? BlobId { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("SurveyAnswerDetails")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("SurveyAnswerId")]
        [InverseProperty("SurveyAnswerDetails")]
        public virtual SurveyAnswers SurveyAnswer { get; set; }
        [ForeignKey("SurveyQuestionId")]
        [InverseProperty("SurveyAnswerDetails")]
        public virtual SurveyQuestions SurveyQuestion { get; set; }
        [InverseProperty("SurveyAnswerDetail")]
        public virtual ICollection<SurveySpecialAnswers> SurveySpecialAnswers { get; set; }
    }
}
