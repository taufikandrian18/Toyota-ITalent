using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class AssesmentQuestionTypes
    {
        public AssesmentQuestionTypes()
        {
            QuestionsNavigation = new HashSet<AssesmentQuestions>();
        }

        
        [Key]
        public String GUIDQuestionType {get; set;}
        [StringLength(128)]
        public string TypeName {get;set;}
        [StringLength(128)]
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

        [InverseProperty("QuestionsTypeNavigator")]
        public virtual ICollection<AssesmentQuestions> QuestionsNavigation { get; set; }
    }
}