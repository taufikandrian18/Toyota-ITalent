using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Talent.Entities.Entities
{
    public class LiveAssesmentSkillCheckScores
    {

        public LiveAssesmentSkillCheckScores()
        {
          
        }

        [Key]
        public string GUID { get; set; }
        public string EmployeeGUID { get; set; }
        public string LiveAssesmentSkillCheckGUID { get; set; }
        public float Attempts { get; set; }
        public string AssesmentGUID { get; set; }
        public string SkillCheckGUID { get; set; }
        public float AverageScore { get; set; }
        public float HighestScore { get; set; }
        public float LowestScore { get; set; }
        public bool IsPass { get; set; }

        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}
        
        [ForeignKey("EmployeeGUID")]
        [InverseProperty("LiveAssesmentSkillCheckScoreNavigation")]
        public virtual Employees EmployeeNavigator { get; set; }

        [ForeignKey("LiveAssesmentSkillCheckGUID")]
        [InverseProperty("LiveAssesmentSkillCheckScoreNavigation")]
        public virtual LiveAssesmentSkillChecks LiveAssesmentSkillChecksNavigator { get; set; }


        [ForeignKey("AssesmentGUID")]
        [InverseProperty("LiveAssesmentSkillCheckScoreNavigation")]
        public virtual Assesments AssesmentNavigator { get; set; }

        [ForeignKey("SkillCheckGUID")]
        [InverseProperty("LiveAssesmentSkillCheckScoreNavigation")]
        public virtual SkillChecks SkillCheckNavigator { get; set; }




    }
}