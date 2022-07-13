using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Talent.Entities.Entities
{
    public class AssesmentQuestionAnswerAnswerRatingOptions
    {
         
        [Key]
        public String GUID {get;set;}
        public String AssesmentAnswerRatingsGUID {get;set;}
        public String Option {get;set;}
        public float Score {get;set;}
        public float Point {get;set;}
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

        [ForeignKey("AssesmentAnswerRatingsGUID")]
        [InverseProperty("AssesmentQuestionAnswerRatingOptionsNavigation")]
        public virtual AssesmentQuestionAnswerAnswerRatings AssesmentQuestionAnswerRatingsOptionsNavigator { get; set; }
    
    }
}