using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class LearningHistories
    {
        [Key]
        public int LearningHistoryId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int? TrainingId { get; set; }
        public int? SetupModuleId { get; set; }
        public int? PopQuizId { get; set; }
        [StringLength(64)]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("LearningHistories")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("PopQuizId")]
        [InverseProperty("LearningHistories")]
        public virtual PopQuizzes PopQuiz { get; set; }
        [ForeignKey("SetupModuleId")]
        [InverseProperty("LearningHistories")]
        public virtual SetupModules SetupModule { get; set; }
        [ForeignKey("TrainingId")]
        [InverseProperty("LearningHistories")]
        public virtual Trainings Training { get; set; }
    }
}
