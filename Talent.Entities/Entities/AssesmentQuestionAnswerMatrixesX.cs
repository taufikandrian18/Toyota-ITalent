using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Talent.Entities.Entities
{
    public class AssesmentQuestionAnswerMatrixesX
    {
         public AssesmentQuestionAnswerMatrixesX()
        {
            AssesmentQuestionAnswerMatrixXNavigation = new HashSet<AssesmentQuestionAnswerMatrixesXY>();
         
        }
             
        [Key]
        public String GUID {get;set;}
        public String AssesmentQuestionAnswerMatrixGUID {get;set;}
        public String Text {get;set;}
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

        [ForeignKey("AssesmentQuestionAnswerMatrixGUID")]
        [InverseProperty("AssesmentQuestionAnswerMatrixXNavigation")]
        public virtual AssesmentQuestionAnswerMatrixes AssesmentQuestionAnswerMatrixesNavigator { get; set; }

        [InverseProperty("AnswerMatrixesXYXNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerMatrixesXY> AssesmentQuestionAnswerMatrixXNavigation { get; set; }
    }
}