using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Talent.Entities.Entities
{
    public class AssesmentQuestionAnswerDropdowns
    {
        [Key]
        public String GUID {get;set;}
        public String AssesmentQuestionGUID {get;set;}
        public String KeyAnswer {get;set;}
        public String Text {get;set;}
        public float Point {get;set;}
        public float Score {get;set;}
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

         [ForeignKey("AssesmentQuestionGUID")]
        [InverseProperty("AssesmentAnswersDropdownsNavigation")]
        public virtual AssesmentQuestions QuestionsNavigator { get; set; }
    }
} 