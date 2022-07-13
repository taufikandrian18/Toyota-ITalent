using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SetupModules
    {
        public SetupModules()
        {
            ApprovalToSetupModules = new HashSet<ApprovalToSetupModules>();
            AssignedLearnings = new HashSet<AssignedLearnings>();
            CalculateLearningQueue = new HashSet<CalculateLearningQueue>();
            CoursePrerequisiteMappings = new HashSet<CoursePrerequisiteMappings>();
            EnrollLearningTimes = new HashSet<EnrollLearningTimes>();
            EnrollLearnings = new HashSet<EnrollLearnings>();
            LearningHistories = new HashSet<LearningHistories>();
            SetupLearning = new HashSet<SetupLearning>();
            SetupTasks = new HashSet<SetupTasks>();
            TaskAnswers = new HashSet<TaskAnswers>();
            TrainingModuleMappings = new HashSet<TrainingModuleMappings>();
            FinalScores = new HashSet<FinalScores>();
        }

        [Key]
        public int SetupModuleId { get; set; }
        public int? CourseId { get; set; }
        public string AssesmentId { get; set; }
        public int? ModuleId { get; set; }
        public int? TrainingTypeId { get; set; }
        public int TimePointId { get; set; }
        public bool IsRecommendedForYou { get; set; }
        public bool IsPopular { get; set; }
        public int? MinimumScore { get; set; }
        public bool? IsForRemedial { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
        public float Order { get; set; }
        public string EnumRemedialOption { get; set; }
        public float RemedialLimit { get; set; }
        public string EnumScoringMethod { get; set; }
        public bool IsByOption { get; set; }
        public string EnumCategoryPreDuringPost { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }

        [ForeignKey("CourseId")]
        [InverseProperty("SetupModules")]
        public virtual Courses Course { get; set; }
        [ForeignKey("ModuleId")]
        [InverseProperty("SetupModules")]
        public virtual Modules Module { get; set; }
        [ForeignKey("TimePointId")]
        [InverseProperty("SetupModules")]
        public virtual TimePoints TimePoint { get; set; }
        [ForeignKey("TrainingTypeId")]
        [InverseProperty("SetupModules")]
        public virtual TrainingTypes TrainingType { get; set; }
        [InverseProperty("SetupModule")]
        public virtual ICollection<ApprovalToSetupModules> ApprovalToSetupModules { get; set; }
        [InverseProperty("SetupModule")]
        public virtual ICollection<AssignedLearnings> AssignedLearnings { get; set; }
        [InverseProperty("SetupModule")]
        public virtual ICollection<CalculateLearningQueue> CalculateLearningQueue { get; set; }
        [InverseProperty("NextSetupModule")]
        public virtual ICollection<CoursePrerequisiteMappings> CoursePrerequisiteMappings { get; set; }
        [InverseProperty("SetupModule")]
        public virtual ICollection<EnrollLearningTimes> EnrollLearningTimes { get; set; }
        [InverseProperty("SetupModule")]
        public virtual ICollection<EnrollLearnings> EnrollLearnings { get; set; }
        [InverseProperty("SetupModule")]
        public virtual ICollection<LearningHistories> LearningHistories { get; set; }
        [InverseProperty("SetupModule")]
        public virtual ICollection<SetupLearning> SetupLearning { get; set; }
        [InverseProperty("SetupModule")]
        public virtual ICollection<SetupTasks> SetupTasks { get; set; }
        [InverseProperty("SetupModule")]
        public virtual ICollection<TaskAnswers> TaskAnswers { get; set; }
        [InverseProperty("SetupModule")]
        public virtual ICollection<TrainingModuleMappings> TrainingModuleMappings { get; set; }
        [InverseProperty("SetupModule")]
        public virtual Assesments Assesment { get; set; }
        [InverseProperty("SetupModule")]
        public virtual ICollection<FinalScores> FinalScores { get; set; }
    }
}
