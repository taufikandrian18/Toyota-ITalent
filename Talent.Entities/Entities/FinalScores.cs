using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class FinalScores
    {
        public FinalScores()
        {
        }
        [Key]
        public Guid FinalScoreId { get; set; }
        public string EmployeeId { get; set; }
        public string SkillCheckGuid { get; set; }
        public int? SetupModuleId { get; set; }
        public string AssesmentId { get; set; }
        public int? ModuleId { get; set; }
        public int? TrainingId { get; set; }
        public int? CourseId { get; set; }
        public float FinalScore { get; set; }
        public bool PassedStatus { get; set; }
        public bool? CertificationStatus {get;set;}
        public DateTime CreatedAt { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("FinalScores")]
        public virtual Employees Employee { get; set; }

        [ForeignKey("SkillCheckGuid")]
        [InverseProperty("FinalScores")]
        public virtual SkillChecks SkillCheck { get; set; }

        [ForeignKey("SetupModuleId")]
        [InverseProperty("FinalScores")]
        public virtual SetupModules SetupModule { get; set; }

        [ForeignKey("AssesmentId")]
        [InverseProperty("FinalScores")]
        public virtual Assesments Assesment { get; set; }

        [ForeignKey("ModuleId")]
        [InverseProperty("FinalScores")]
        public virtual Modules Module { get; set; }

        [ForeignKey("TrainingId")]
        [InverseProperty("FinalScores")]
        public virtual Trainings Training { get; set; }

        [ForeignKey("CourseId")]
        [InverseProperty("FinalScores")]
        public virtual Courses Course { get; set; }
    }
}
