using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskAnswers
    {
        public TaskAnswers()
        {
            TaskAnswerDetails = new HashSet<TaskAnswerDetails>();
        }

        [Key]
        public int TaskAnswerId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int? TrainingId { get; set; }
        public int? SetupModuleId { get; set; }
        public int? PopQuizId { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("TaskAnswers")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("PopQuizId")]
        [InverseProperty("TaskAnswers")]
        public virtual PopQuizzes PopQuiz { get; set; }
        [ForeignKey("SetupModuleId")]
        [InverseProperty("TaskAnswers")]
        public virtual SetupModules SetupModule { get; set; }
        [ForeignKey("TrainingId")]
        [InverseProperty("TaskAnswers")]
        public virtual Trainings Training { get; set; }
        [InverseProperty("TaskAnswer")]
        public virtual ICollection<TaskAnswerDetails> TaskAnswerDetails { get; set; }

        [InverseProperty("TaskAnswer")]
        public virtual ICollection<TaskScores> TaskScoreNavigation { get; set; }
    }
}
