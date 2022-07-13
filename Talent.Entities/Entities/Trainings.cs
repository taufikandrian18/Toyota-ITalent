using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class  Trainings
    {
        public Trainings()
        {
            ApprovalToTrainings = new HashSet<ApprovalToTrainings>();
            AssignedLearnings = new HashSet<AssignedLearnings>();
            CoachRatings = new HashSet<CoachRatings>();
            EnrollLearnings = new HashSet<EnrollLearnings>();
            LearningHistories = new HashSet<LearningHistories>();
            TaskAnswers = new HashSet<TaskAnswers>();
            TrainingInvitations = new HashSet<TrainingInvitations>();
            TrainingModuleMappings = new HashSet<TrainingModuleMappings>();
            TrainingOutletMappings = new HashSet<TrainingOutletMappings>();
            TrainingPositionMappings = new HashSet<TrainingPositionMappings>();
            TrainingPositionOnlyViewMappings = new HashSet<TrainingPositionOnlyViewMappings>();
            TrainingRatings = new HashSet<TrainingRatings>();
            EmployeeCertificates = new HashSet<EmployeeCertificates>();
            FinalScores = new HashSet<FinalScores>();
            TrainingCertifications = new HashSet<TrainingCertifications>();
            RelatedTrainingCertifications = new HashSet<TrainingCertifications>();
            LiveAssesmentSkillCheckNavigation = new HashSet<LiveAssesmentSkillChecks>();
        }

        [Key]
        public int TrainingId { get; set; }
        public int CourseId { get; set; }
        public int Batch { get; set; }
        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }
        public int? Quota { get; set; }
        [StringLength(255)]
        public string Location { get; set; }
        public bool? IsAccommodate { get; set; }
        public bool IsParticipantTrainee { get; set; }
        public bool IsParticipantPermanent { get; set; }
        public bool IsCertificate { get; set; }
        public string EnumCertificate { get; set; } 
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(255)]
        public string UpdatedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        [Column("isDeleted")]
        public bool IsDeleted { get; set; }
        public int Top5Course { get; set; }
        public DateTime? RescheduledAt { get; set; }
        public string EnumCeritificationTrigger { get; set; }

        [ForeignKey("CourseId")]
        [InverseProperty("Trainings")]
        public virtual Courses Course { get; set; }
        [InverseProperty("Training")]
        public virtual ICollection<ApprovalToTrainings> ApprovalToTrainings { get; set; }
        [InverseProperty("Training")]
        public virtual ICollection<AssignedLearnings> AssignedLearnings { get; set; }
        [InverseProperty("Training")]
        public virtual ICollection<CoachRatings> CoachRatings { get; set; }
        [InverseProperty("Training")]
        public virtual ICollection<EnrollLearnings> EnrollLearnings { get; set; }
        [InverseProperty("Training")]
        public virtual ICollection<LearningHistories> LearningHistories { get; set; }
        [InverseProperty("Training")]
        public virtual ICollection<TaskAnswers> TaskAnswers { get; set; }
        [InverseProperty("Training")]
        public virtual ICollection<TrainingInvitations> TrainingInvitations { get; set; }
        [InverseProperty("Training")]
        public virtual ICollection<TrainingModuleMappings> TrainingModuleMappings { get; set; }
        [InverseProperty("Training")]
        public virtual ICollection<TrainingOutletMappings> TrainingOutletMappings { get; set; }
        [InverseProperty("Training")]
        public virtual ICollection<TrainingPositionMappings> TrainingPositionMappings { get; set; }
        [InverseProperty("Training")]
        public virtual ICollection<TrainingRatings> TrainingRatings { get; set; }
         [InverseProperty("Training")]
        public virtual ICollection<TrainingPositionOnlyViewMappings> TrainingPositionOnlyViewMappings { get; set; }
        [InverseProperty("Training")]
        public virtual ICollection<EmployeeCertificates> EmployeeCertificates { get; set; }
        [InverseProperty("Training")]
        public virtual ICollection<FinalScores> FinalScores { get; set; }
        [InverseProperty("Training")]

        public virtual ICollection<TrainingCertifications> TrainingCertifications { get; set; }

        [InverseProperty("Training")]
        public virtual ICollection<LiveAssesmentSkillChecks> LiveAssesmentSkillCheckNavigation { get; set; }

        [InverseProperty("RelatedTraining")]

        public virtual ICollection<TrainingCertifications> RelatedTrainingCertifications { get; set; }


    }
}
