using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class Assesments
    {
        public Assesments()
        {
            AssesmentSkillChecksNavigation = new HashSet<AssesmentSkillChecks>();
            LiveAssesmentSkillCheckScoreNavigation = new HashSet<LiveAssesmentSkillCheckScores>();
            FinalScores = new HashSet<FinalScores>();
        }

        [Key]
        public String GUID {get;set;}

        public String Name {get;set;}
        public String TrainingTypeID {get; set;}
        public String EnumScoringMethod {get;set;}
        public String EnumRemedialOption {get;set;}
        public float LearningTime {get;set;}
        public float RemedialLimit { get; set; }
        public bool IsByOption { get; set; }
        public float MinimumScore { get; set; }
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

        [InverseProperty("AssesmentsNavigator")]
        public virtual ICollection<AssesmentSkillChecks> AssesmentSkillChecksNavigation { get; set; }

        [InverseProperty("Assesment")]
        public virtual ICollection<SetupLearning> SetupLearning { get; set; }

        [InverseProperty("Assesment")]
        public virtual ICollection<SetupModules> SetupModule { get; set; }

        [InverseProperty("AssesmentNavigator")]
        public virtual ICollection<LiveAssesmentSkillCheckScores> LiveAssesmentSkillCheckScoreNavigation { get; set; }

        [InverseProperty("Assessments")]
        public virtual ICollection<LiveAssesmentSkillChecks> LiveAssesmentSkillCheckNavigation  { get; set; }

        [InverseProperty("Assesment")]
        public virtual ICollection<FinalScores> FinalScores { get; set; }
    }
}