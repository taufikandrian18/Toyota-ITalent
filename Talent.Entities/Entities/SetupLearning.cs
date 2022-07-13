using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SetupLearning
    {
        public int SetupLearningId { get; set; }
        public int? SetupModuleId { get; set; }
        public int? PopQuizId { get; set; }
        public int? CourseId { get; set; }
        public string AssesmentId { get; set; }
        [Required]
        [StringLength(64)]
        public string LearningName { get; set; }
        [StringLength(64)]
        public string ProgramTypeName { get; set; }
        [Required]
        [StringLength(64)]
        public string LearningCategoryName { get; set; }
        [Required]
        [StringLength(64)]
        public string ApprovalStatus { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }

        [ForeignKey("CourseId")]
        [InverseProperty("SetupLearning")]
        public virtual Courses Course { get; set; }
        [ForeignKey("PopQuizId")]
        [InverseProperty("SetupLearning")]
        public virtual PopQuizzes PopQuiz { get; set; }
        [ForeignKey("SetupModuleId")]
        [InverseProperty("SetupLearning")]
        public virtual SetupModules SetupModule { get; set; }
        [ForeignKey("AssesmentId")]
        [InverseProperty("SetupLearning")]
        public virtual Assesments Assesment { get; set; }
    }
}
