using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Tasks
    {
        public Tasks()
        {
            ApprovalToTasks = new HashSet<ApprovalToTasks>();
            SetupTaskCodes = new HashSet<SetupTaskCodes>();
            TaskAnswerDetails = new HashSet<TaskAnswerDetails>();
            TaskChecklistChoices = new HashSet<TaskChecklistChoices>();
            TaskHotSpotAnswers = new HashSet<TaskHotSpotAnswers>();
            TaskMatchingChoices = new HashSet<TaskMatchingChoices>();
            TaskMatrixChoices = new HashSet<TaskMatrixChoices>();
            TaskMatrixQuestions = new HashSet<TaskMatrixQuestions>();
            TaskMultipleChoiceChoices = new HashSet<TaskMultipleChoiceChoices>();
            TaskRatingChoices = new HashSet<TaskRatingChoices>();
            TaskSequenceChoices = new HashSet<TaskSequenceChoices>();
            TaskTebakGambarPictures = new HashSet<TaskTebakGambarPictures>();
        }

        [Key]
        public int TaskId { get; set; }
        public int CompetencyId { get; set; }
        public int QuestionTypeId { get; set; }
        public int ModuleId { get; set; }
        public Guid? BlobId { get; set; }
        public int EvaluationTypeId { get; set; }
        public int TaskNumber { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(255)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("Tasks")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("CompetencyId")]
        [InverseProperty("Tasks")]
        public virtual Competencies Competency { get; set; }
        [ForeignKey("EvaluationTypeId")]
        [InverseProperty("Tasks")]
        public virtual EvaluationTypes EvaluationType { get; set; }
        [ForeignKey("ModuleId")]
        [InverseProperty("Tasks")]
        public virtual Modules Module { get; set; }
        [ForeignKey("QuestionTypeId")]
        [InverseProperty("Tasks")]
        public virtual QuestionTypes QuestionType { get; set; }
        [InverseProperty("Task")]
        public virtual TaskChecklistTypes TaskChecklistTypes { get; set; }
        [InverseProperty("Task")]
        public virtual TaskEssayTypes TaskEssayTypes { get; set; }
        [InverseProperty("Task")]
        public virtual TaskFileUploadTypes TaskFileUploadTypes { get; set; }
        [InverseProperty("Task")]
        public virtual TaskHotSpotTypes TaskHotSpotTypes { get; set; }
        [InverseProperty("Task")]
        public virtual TaskMatchingTypes TaskMatchingTypes { get; set; }
        [InverseProperty("Task")]
        public virtual TaskMatrixTypes TaskMatrixTypes { get; set; }
        [InverseProperty("Task")]
        public virtual TaskMultipleChoiceTypes TaskMultipleChoiceTypes { get; set; }
        [InverseProperty("Task")]
        public virtual TaskRatingTypes TaskRatingTypes { get; set; }
        [InverseProperty("Task")]
        public virtual TaskSequenceTypes TaskSequenceTypes { get; set; }
        [InverseProperty("Task")]
        public virtual TaskShortAnswerTypes TaskShortAnswerTypes { get; set; }
        [InverseProperty("Task")]
        public virtual TaskTebakGambarTypes TaskTebakGambarTypes { get; set; }
        [InverseProperty("Task")]
        public virtual TaskTrueFalseTypes TaskTrueFalseTypes { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<ApprovalToTasks> ApprovalToTasks { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<SetupTaskCodes> SetupTaskCodes { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<TaskAnswerDetails> TaskAnswerDetails { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<TaskChecklistChoices> TaskChecklistChoices { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<TaskHotSpotAnswers> TaskHotSpotAnswers { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<TaskMatchingChoices> TaskMatchingChoices { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<TaskMatrixChoices> TaskMatrixChoices { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<TaskMatrixQuestions> TaskMatrixQuestions { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<TaskMultipleChoiceChoices> TaskMultipleChoiceChoices { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<TaskRatingChoices> TaskRatingChoices { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<TaskSequenceChoices> TaskSequenceChoices { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<TaskTebakGambarPictures> TaskTebakGambarPictures { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<TaskScores> TaskScoreNavigation { get; set; }
    }
}
