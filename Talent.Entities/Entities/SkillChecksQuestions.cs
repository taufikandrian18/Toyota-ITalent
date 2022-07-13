using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Talent.Entities.Entities
{
    public class SkillChecksQuestions
    {

        public SkillChecksQuestions()
        {
        }

        [Key]
        public String GUID {get;set;}
        public String SkillCheckGUID{get;set;}
        public String AssesmentQuestionGUID {get;set;}
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

        [ForeignKey("SkillCheckGUID")]
        [InverseProperty("SkillChecksQuestionsNavigation")]
        public virtual SkillChecks SkillCheckNavigator { get; set; }

        [ForeignKey("AssesmentQuestionGUID")]
        [InverseProperty("AssesmentsQuestionsNavigation")]
        public virtual AssesmentQuestions AssesmentsQuestionsNavigator { get; set; }
       
    }
}