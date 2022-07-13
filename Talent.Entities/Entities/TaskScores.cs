using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Talent.Entities.Entities
{
    public class TaskScores
    {

        public TaskScores()
        {
          
        }

        [Key]
        public string GUID { get; set; }
        public string EmployeeGUID { get; set; }
        public float Attempts { get; set; }
        public int? TaskID { get; set; }
        public int? ModuleID { get; set; }
        public float AverageScore { get; set; }
        public float HighestScore { get; set; }
        public float LowestScore { get; set; }
        public bool IsPass { get; set; }
        public int? TaskAnswerID { get; set; }
        public int? TaskAnswerDetailID{ get; set; }

        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

        [ForeignKey("EmployeeGUID")]
        [InverseProperty("TaskScoreNavigation")]
        public virtual Employees EmployeeNavigator { get; set; }



        [ForeignKey("TaskID")]
        [InverseProperty("TaskScoreNavigation")]
        public virtual Tasks Task { get; set; }

        [ForeignKey("TaskAnswerID")]
        [InverseProperty("TaskScoreNavigation")]
        public virtual TaskAnswers TaskAnswer { get; set; }

        [ForeignKey("TaskAnswerDetailID")]
        [InverseProperty("TaskScoreNavigation")]
        public virtual TaskAnswerDetails TaskAnswerDetails { get; set; }

        [ForeignKey("ModuleID")]
        [InverseProperty("TaskScoreNavigation")]
        public virtual Modules Module { get; set; }




    }
}