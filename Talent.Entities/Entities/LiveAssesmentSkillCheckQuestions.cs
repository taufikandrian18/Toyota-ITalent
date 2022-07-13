using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class LiveAssesmentSkillCheckQuestions
    {
        [Key]
        public string GUID {get;set;}

        public string LiveAssesmentGUID {get;set;}
        public string AssesmentQuestionGUID {get;set;}
        public string Answer {get;set;}
        public string Text {get;set;}
        public float Point {get;set;}
        public float Score {get;set;}
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

      
           
        [ForeignKey("AssesmentQuestionGUID")]
        [InverseProperty("LiveAssesmentSkillCheckQuestionNavigation")]
        public virtual AssesmentQuestions AssesmentQuestionNavigator { get; set; }

            
        [ForeignKey("LiveAssesmentGUID")]
        [InverseProperty("LiveAssesmentSkillCheckQuestionNavigation")]
        public virtual LiveAssesmentSkillChecks LiveAssesmentSkillCheckNavigator { get; set; }
        
    }
}