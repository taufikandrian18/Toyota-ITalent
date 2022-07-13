using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SurveyMatchingChoices
    {
        [Key]
        public int SurveyMatchingChoiceId { get; set; }
        public int SurveyQuestionId { get; set; }
        public Guid? BlobId { get; set; }
        [Required]
        [StringLength(3)]
        public string SurveyMatchingChoiceCode { get; set; }
        [StringLength(64)]
        public string Text { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("SurveyMatchingChoices")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("SurveyQuestionId")]
        [InverseProperty("SurveyMatchingChoices")]
        public virtual SurveyQuestions SurveyQuestion { get; set; }
    }
}
