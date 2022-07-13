using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SurveyChoices
    {
        [Key]
        public int SurveyChoiceId { get; set; }
        public int SurveyQuestionId { get; set; }
        [StringLength(512)]
        public string Choice { get; set; }
        public Guid? BlobId { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("SurveyChoices")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("SurveyQuestionId")]
        [InverseProperty("SurveyChoices")]
        public virtual SurveyQuestions SurveyQuestion { get; set; }
    }
}
