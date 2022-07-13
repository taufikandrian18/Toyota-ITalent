using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Talent.Entities.Entities
{
    public class AssesmentQuestionAnswerAnswerRatingRates
    {
        [Key]
        public String GUID {get;set;}
        public String AssesmentAnswerRatingsGUID {get;set;}
        public String Range {get;set;}
        public float Score {get;set;}
        public float Point {get;set;}
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}
         
        [ForeignKey("AssesmentAnswerRatingsGUID")]
        [InverseProperty("AssesmentQuestionAnswerRatingRatesNavigation")]
        public virtual AssesmentQuestionAnswerAnswerRatings AssesmentQuestionAnswerRatingsRatesNavigator { get; set; }
    }
}