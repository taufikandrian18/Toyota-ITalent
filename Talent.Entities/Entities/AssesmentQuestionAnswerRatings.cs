using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Talent.Entities.Entities
{
    public class AssesmentQuestionAnswerAnswerRatings
    {

         public AssesmentQuestionAnswerAnswerRatings()
        {
            AssesmentQuestionAnswerRatingRatesNavigation = new HashSet<AssesmentQuestionAnswerAnswerRatingRates>();
            AssesmentQuestionAnswerRatingOptionsNavigation = new HashSet<AssesmentQuestionAnswerAnswerRatingOptions>();
        }

         [Key]
        public String GUID {get;set;}
        public String AssesmentQuestionGUID {get;set;}
        public String Text {get;set;}
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

        [ForeignKey("AssesmentQuestionGUID")]
        [InverseProperty("AssesmentAnswerRatingsNavigation")]
        public virtual AssesmentQuestions QuestionsNavigator { get; set; }

         [InverseProperty("AssesmentQuestionAnswerRatingsRatesNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerAnswerRatingRates> AssesmentQuestionAnswerRatingRatesNavigation { get; set; }

         [InverseProperty("AssesmentQuestionAnswerRatingsOptionsNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerAnswerRatingOptions>AssesmentQuestionAnswerRatingOptionsNavigation { get; set; }
    }
}