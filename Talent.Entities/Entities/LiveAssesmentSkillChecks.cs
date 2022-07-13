using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Talent.Entities.Entities
{
    public class LiveAssesmentSkillChecks
    {

        public LiveAssesmentSkillChecks()
        {
            LiveAssesmentSkillCheckQuestionNavigation = new HashSet<LiveAssesmentSkillCheckQuestions>();
            LiveAssesmentSkillCheckScoreNavigation = new HashSet<LiveAssesmentSkillCheckScores>();
        }

        [Key]
        public string GUID { get; set; }
        public string EmployeeGUID {get;set;}
        public string ScorerGUID {get;set;}
        public string ScorerType {get;set;}
        public string SkillCheckGUID {get;set;}
        public string AssesmentGUID {get;set;}
        public int? TrainingId {get;set;}
        public string Answer {get;set;}
        public string Text {get;set;}
        public float Point {get;set;}   

        public float Score {get;set;}

        public bool IsDraft {get;set;}
        public bool IsScored { get; set; }
        public float Attempts { get; set; }

        public bool IsFinished { get; set; }

        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}
        
        // TO DO : Create Relation In Emplyee Entity 
        [ForeignKey("EmployeeGUID")]
        [InverseProperty("LiveAssesmentSkillCheckNavigation")]
        public virtual Employees EmployeeNavigator { get; set; }

        [ForeignKey("ScorerGUID")]
        [InverseProperty("LiveAssesmentSkillChecScorerkNavigation")]
        public virtual Employees ScorerNavigator { get; set; }

            
        [ForeignKey("SkillCheckGUID")]
        [InverseProperty("LiveAssesmentSkillCheckNavigation")]
        public virtual SkillChecks SkillCheckNavigator { get; set; }

        [ForeignKey("AssesmentGUID")]
        [InverseProperty("LiveAssesmentSkillCheckNavigation")]
        public virtual Assesments Assessments { get; set; }

        [ForeignKey("TrainingId")]
        [InverseProperty("LiveAssesmentSkillCheckNavigation")]
        public virtual Trainings Training { get; set; }

        [InverseProperty("LiveAssesmentSkillCheckNavigator")]
        public virtual ICollection<LiveAssesmentSkillCheckQuestions> LiveAssesmentSkillCheckQuestionNavigation { get; set; }

        [InverseProperty("LiveAssesmentSkillChecksNavigator")]
        public virtual ICollection<LiveAssesmentSkillCheckScores> LiveAssesmentSkillCheckScoreNavigation { get; set; }

    }
}