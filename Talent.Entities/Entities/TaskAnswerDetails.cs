using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TaskAnswerDetails
    {
        public TaskAnswerDetails()
        {
            TaskSpecialAnswers = new HashSet<TaskSpecialAnswers>();
        }

        [Key]
        public int TaskAnswerDetailId { get; set; }
        public int TaskAnswerId { get; set; }
        public int TaskId { get; set; }
        public string Answer { get; set; }
        public Guid? AnswerBlobId { get; set; }
        public int? Score { get; set; }
        public int? Point { get; set; }
        public float Attempts { get; set; }
        public bool IsFinished { get; set; }
        public string ScorerGUID { get; set; }
        public string Feedback { get; set; }
        public DateTime? CreatedAt { get; set; }

        [Column("isTrue")]
        public bool IsTrue { get; set; }
        [Column("isChecked")]
        public bool IsChecked { get; set; }

        [ForeignKey("AnswerBlobId")]
        [InverseProperty("TaskAnswerDetails")]
        public virtual Blobs AnswerBlob { get; set; }
        [ForeignKey("TaskId")]
        [InverseProperty("TaskAnswerDetails")]
        public virtual Tasks Task { get; set; }
        [ForeignKey("TaskAnswerId")]
        [InverseProperty("TaskAnswerDetails")]
        public virtual TaskAnswers TaskAnswer { get; set; }
        [InverseProperty("TaskAnswerDetail")]
        public virtual ICollection<TaskSpecialAnswers> TaskSpecialAnswers { get; set; }
        [InverseProperty("TaskAnswerDetails")]
        public virtual ICollection<TaskScores> TaskScoreNavigation { get; set; }
        [ForeignKey("ScorerGUID")]
        [InverseProperty("TaskAnswerDetails")]
        public virtual Employees Employee { get; set; }
    }
}
