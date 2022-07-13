using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SurveyQuestions
    {
        public SurveyQuestions()
        {
            SurveyAnswerDetails = new HashSet<SurveyAnswerDetails>();
            SurveyChoices = new HashSet<SurveyChoices>();
            SurveyMatchingChoices = new HashSet<SurveyMatchingChoices>();
            SurveyMatrixChoices = new HashSet<SurveyMatrixChoices>();
            SurveyMatrixQuestions = new HashSet<SurveyMatrixQuestions>();
        }

        [Key]
        public int SurveyQuestionId { get; set; }
        public int SurveyId { get; set; }
        public int SurveyQuestionTypeId { get; set; }
        public int QuestionNumber { get; set; }
        [StringLength(512)]
        public string Question { get; set; }
        public Guid? BlobId { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("SurveyQuestions")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("SurveyId")]
        [InverseProperty("SurveyQuestions")]
        public virtual Surveys Survey { get; set; }
        [ForeignKey("SurveyQuestionTypeId")]
        [InverseProperty("SurveyQuestions")]
        public virtual SurveyQuestionTypes SurveyQuestionType { get; set; }
        [InverseProperty("SurveyQuestion")]
        public virtual ICollection<SurveyAnswerDetails> SurveyAnswerDetails { get; set; }
        [InverseProperty("SurveyQuestion")]
        public virtual ICollection<SurveyChoices> SurveyChoices { get; set; }
        [InverseProperty("SurveyQuestion")]
        public virtual ICollection<SurveyMatchingChoices> SurveyMatchingChoices { get; set; }
        [InverseProperty("SurveyQuestion")]
        public virtual ICollection<SurveyMatrixChoices> SurveyMatrixChoices { get; set; }
        [InverseProperty("SurveyQuestion")]
        public virtual ICollection<SurveyMatrixQuestions> SurveyMatrixQuestions { get; set; }
    }
}
