using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Talent.Entities.Entities
{
    public class AssesmentQuestionAnswerMatrixes
    {       
         public AssesmentQuestionAnswerMatrixes()
        {
            AssesmentQuestionAnswerMatrixXNavigation = new HashSet<AssesmentQuestionAnswerMatrixesX>();
            AssesmentQuestionAnswerMatrixYNavigation = new HashSet<AssesmentQuestionAnswerMatrixesY>();
        }

        [Key]
        public String GUID {get;set;}
        public String AssesmentQuestionGUID {get;set;}
        public String Text {get;set;}
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

         [ForeignKey("AssesmentQuestionGUID")]
        [InverseProperty("AssesmentAnswerMatrixesNavigation")]
        public virtual AssesmentQuestions QuestionsNavigator { get; set; }
    

        [InverseProperty("AssesmentQuestionAnswerMatrixesNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerMatrixesX> AssesmentQuestionAnswerMatrixXNavigation { get; set; }

         [InverseProperty("AssesmentQuestionAnswerMatrixesNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerMatrixesY> AssesmentQuestionAnswerMatrixYNavigation { get; set; }
    }
}