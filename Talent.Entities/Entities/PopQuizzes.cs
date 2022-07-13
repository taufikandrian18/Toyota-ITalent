using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class PopQuizzes
    {
        public PopQuizzes()
        {
            ApprovalToPopQuizzes = new HashSet<ApprovalToPopQuizzes>();
            LearningHistories = new HashSet<LearningHistories>();
            SetupLearning = new HashSet<SetupLearning>();
            SetupTasks = new HashSet<SetupTasks>();
            TaskAnswers = new HashSet<TaskAnswers>();
        }

        [Key]
        public int PopQuizId { get; set; }
        [Required]
        [StringLength(64)]
        public string PopQuizName { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty("PopQuiz")]
        public virtual ICollection<ApprovalToPopQuizzes> ApprovalToPopQuizzes { get; set; }
        [InverseProperty("PopQuiz")]
        public virtual ICollection<LearningHistories> LearningHistories { get; set; }
        [InverseProperty("PopQuiz")]
        public virtual ICollection<SetupLearning> SetupLearning { get; set; }
        [InverseProperty("PopQuiz")]
        public virtual ICollection<SetupTasks> SetupTasks { get; set; }
        [InverseProperty("PopQuiz")]
        public virtual ICollection<TaskAnswers> TaskAnswers { get; set; }
    }
}
