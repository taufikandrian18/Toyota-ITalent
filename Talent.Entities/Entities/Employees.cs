using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Employees
    {
        public Employees()
        {
            ApprovalsActionByNavigation = new HashSet<Approvals>();
            ApprovalsApprovalToNavigation = new HashSet<Approvals>();
            ApprovalsCreatedByNavigation = new HashSet<Approvals>();
            Assessments = new HashSet<Assessments>();
            AssignedLearningsAssignedByNavigation = new HashSet<AssignedLearnings>();
            AssignedLearningsAssignedToNavigation = new HashSet<AssignedLearnings>();
            CoachBookingSchedules = new HashSet<CoachBookingSchedules>();
            CoachRatings = new HashSet<CoachRatings>();
            Coaches = new HashSet<Coaches>();
            DigitalSignatures = new HashSet<DigitalSignatures>();
            DictionaryMappings = new HashSet<DictionaryMappings>();
            EmployeeAccessTimes = new HashSet<EmployeeAccessTimes>();
            EmployeeBadges = new HashSet<EmployeeBadges>();
            EmployeeCertificates = new HashSet<EmployeeCertificates>();
            EmployeeEventMappings = new HashSet<EmployeeEventMappings>();
            EmployeeHobbyMappings = new HashSet<EmployeeHobbyMappings>();
            EmployeeInterests = new HashSet<EmployeeInterests>();
            EmployeePointHistories = new HashSet<EmployeePointHistories>();
            EmployeePositionMappings = new HashSet<EmployeePositionMappings>();
            EmployeeRewardMappings = new HashSet<EmployeeRewardMappings>();
            EnrollLearnings = new HashSet<EnrollLearnings>();
            Inboxes = new HashSet<Inboxes>();
            LearningHistories = new HashSet<LearningHistories>();
            MobileInboxesEmployee = new HashSet<MobileInboxes>();
            MobileInboxesResignEmployee = new HashSet<MobileInboxes>();
            RotateTeamMembers = new HashSet<RotateTeamMembers>();
            SurveyAnswers = new HashSet<SurveyAnswers>();
            TaskAnswers = new HashSet<TaskAnswers>();
            TaskAnswerDetails = new HashSet<TaskAnswerDetails>();
            TeamDetails = new HashSet<TeamDetails>();
            TeamMemberRequests = new HashSet<TeamMemberRequests>();
            Teams = new HashSet<Teams>();
            TrainingInvitations = new HashSet<TrainingInvitations>();
            TrainingRatings = new HashSet<TrainingRatings>();
            UpgradeAccountApprovals = new HashSet<UpgradeAccountApprovals>();
            UserFcmTokens = new HashSet<UserFcmTokens>();
            EmployeeDownloads = new HashSet<EmployeeDownload>();
            PushNotifications = new HashSet<PushNotifications>();
            PushNotificationRecipients = new HashSet<PushNotificationRecipients>();
            LiveAssesmentSkillCheckScoreNavigation = new HashSet<LiveAssesmentSkillCheckScores>();
            FinalScores = new HashSet<FinalScores>();
        }

        [Key]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        [StringLength(64)]
        public string NIP { get; set; }
        [StringLength(64)]
        public string EmployeeMDMCode { get; set; }
        [StringLength(64)]
        public string EmployeeMDMUsername { get; set; }
        [StringLength(64)]
        public string OutletId { get; set; }
        [Required]
        [StringLength(255)]
        public string EmployeeUsername { get; set; }
        [StringLength(255)]
        public string Password { get; set; }

        public string EmployeeName { get; set; }
        public int EmployeeExperience { get; set; }
        public int LearningPoint { get; set; }
        public int TeachingPoint { get; set; }
        [StringLength(255)]
        public string EmployeeEmail { get; set; }
        [StringLength(50)]
        public string EmployeePhone { get; set; }
        public bool IsCoach { get; set; }
        public string Religion { get; set; }
        public string Addresses { get; set; }

        public string PoistionNote { get; set; }

        public bool IsTeamLeader { get; set; }
        public DateTime? LastUpdatedPasswordDate { get; set; }
        public DateTime? LastSeenAt { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? BlobId { get; set; }
        [StringLength(64)]
        public string Nickname { get; set; }
        public string ManpowerCode { get; set; }
        public string Description { get; set; }

        public bool IsSuspended { get; set; }
        public bool IsRequestUpgrade { get; set; }
        public bool IsDataValidation { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? AccountValid { get; set; }
        [StringLength(64)]
        public string Gender { get; set; }
        [StringLength(32)]
        public string ManpowerStatusType { get; set; }
        [StringLength(250)]
        public string DealerPeopleCategoryCode { get; set; }
        [Column("KTP")]
        [StringLength(64)]
        public string Ktp { get; set; }
        public string Status { get; set; }
        public int? CounterLogin { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("Employees")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("DealerPeopleCategoryCode")]
        [InverseProperty("Employees")]
        public virtual DealerPeopleCategories DealerPeopleCategoryCodeNavigation { get; set; }
        [ForeignKey("OutletId")]
        [InverseProperty("Employees")]
        public virtual Outlets Outlet { get; set; }
        [InverseProperty("ActionByNavigation")]
        public virtual ICollection<Approvals> ApprovalsActionByNavigation { get; set; }
        [InverseProperty("ApprovalToNavigation")]
        public virtual ICollection<Approvals> ApprovalsApprovalToNavigation { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<Approvals> ApprovalsCreatedByNavigation { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<Assessments> Assessments { get; set; }
        [InverseProperty("AssignedByNavigation")]
        public virtual ICollection<AssignedLearnings> AssignedLearningsAssignedByNavigation { get; set; }
        [InverseProperty("AssignedToNavigation")]
        public virtual ICollection<AssignedLearnings> AssignedLearningsAssignedToNavigation { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<CoachBookingSchedules> CoachBookingSchedules { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<CoachRatings> CoachRatings { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<Coaches> Coaches { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<DigitalSignatures> DigitalSignatures { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<DictionaryMappings> DictionaryMappings { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<EmployeeAccessTimes> EmployeeAccessTimes { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<EmployeeBadges> EmployeeBadges { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<EmployeeCertificates> EmployeeCertificates { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<EmployeeEventMappings> EmployeeEventMappings { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<EmployeeHobbyMappings> EmployeeHobbyMappings { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<EmployeeInterests> EmployeeInterests { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<EmployeePointHistories> EmployeePointHistories { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<EmployeePositionMappings> EmployeePositionMappings { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<EmployeeRewardMappings> EmployeeRewardMappings { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<EnrollLearnings> EnrollLearnings { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<Inboxes> Inboxes { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<LearningHistories> LearningHistories { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<MobileInboxes> MobileInboxesEmployee { get; set; }
        [InverseProperty("ResignEmployee")]
        public virtual ICollection<MobileInboxes> MobileInboxesResignEmployee { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<RotateTeamMembers> RotateTeamMembers { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<SurveyAnswers> SurveyAnswers { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<TaskAnswers> TaskAnswers { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<TaskAnswerDetails> TaskAnswerDetails { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<TeamDetails> TeamDetails { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<TeamMemberRequests> TeamMemberRequests { get; set; }
        [InverseProperty("TeamLeader")]
        public virtual ICollection<Teams> Teams { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<TrainingInvitations> TrainingInvitations { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<TrainingRatings> TrainingRatings { get; set; }

        [InverseProperty("EmployeeNavigator")]
        public virtual ICollection<LiveAssesmentSkillChecks> LiveAssesmentSkillCheckNavigation { get; set; }

        [InverseProperty("ScorerNavigator")]
        public virtual ICollection<LiveAssesmentSkillChecks> LiveAssesmentSkillChecScorerkNavigation { get; set; }

        public virtual ICollection<UpgradeAccountApprovals> UpgradeAccountApprovals { get; set; }
        public virtual ICollection<PushNotifications> PushNotifications { get; set; }

        public virtual ICollection<EmployeeDownload> EmployeeDownloads { get; set; }
        public virtual ICollection<PushNotificationRecipients> PushNotificationRecipients { get; set; }
        public virtual ICollection<UserFcmTokens> UserFcmTokens { get; set; }

        [InverseProperty("EmployeeNavigator")]
        public virtual ICollection<LiveAssesmentSkillCheckScores> LiveAssesmentSkillCheckScoreNavigation { get; set; }

        [InverseProperty("EmployeeNavigator")]
        public virtual ICollection<TaskScores> TaskScoreNavigation { get; set; }

        [InverseProperty("Employee")]
        public virtual ICollection<FinalScores> FinalScores { get; set; }
    }
}
