using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Talent.Entities.Entities
{
    public class AssesmentQuestionAnswerMatrixesXY
    {
           [Key]
        public String GUID {get;set;}
        public String AssesmentQuestionAnswerMatrixXGUID {get;set;}
        public String AssesmentQuestionAnswerMatrixYGUID {get;set;}
        public String Text {get;set;}
        public float Score {get;set;}
        public float Point{get;set;}
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

        [ForeignKey("AssesmentQuestionAnswerMatrixXGUID")]
        [InverseProperty("AssesmentQuestionAnswerMatrixXNavigation")]
        public virtual AssesmentQuestionAnswerMatrixesX AnswerMatrixesXYXNavigator { get; set; }

        [ForeignKey("AssesmentQuestionAnswerMatrixYGUID")]
        [InverseProperty("AssesmentQuestionAnswerMatrixYNavigation")]
        public virtual AssesmentQuestionAnswerMatrixesY AnswerMatrixesXYYNavigator { get; set; }
    }
}