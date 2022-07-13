using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Talent.Entities.Entities
{
    public partial class TalentContext : DbContext
    {

        public TalentContext(DbContextOptions<TalentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accommodations> Accommodations { get; set; }
        public virtual DbSet<ApprovalHistories> ApprovalHistories { get; set; }
        public virtual DbSet<ApprovalLevels> ApprovalLevels { get; set; }
        public virtual DbSet<ApprovalMappings> ApprovalMappings { get; set; }
        public virtual DbSet<ApprovalPositionMappings> ApprovalPositionMappings { get; set; }
        public virtual DbSet<ApprovalStatus> ApprovalStatus { get; set; }
        public virtual DbSet<ApprovalToBanners> ApprovalToBanners { get; set; }
        public virtual DbSet<ApprovalToCourses> ApprovalToCourses { get; set; }
        public virtual DbSet<ApprovalToEvents> ApprovalToEvents { get; set; }
        public virtual DbSet<ApprovalToGuides> ApprovalToGuides { get; set; }
        public virtual DbSet<ApprovalToModules> ApprovalToModules { get; set; }
        public virtual DbSet<ApprovalToNews> ApprovalToNews { get; set; }
        public virtual DbSet<UpgradeAccountApprovals> UpgradeAccountApprovals { get; set; }
        public virtual DbSet<ApprovalToPopQuizzes> ApprovalToPopQuizzes { get; set; }
        public virtual DbSet<ApprovalToSetupCourses> ApprovalToSetupCourses { get; set; }
        public virtual DbSet<ApprovalToSetupModules> ApprovalToSetupModules { get; set; }
        public virtual DbSet<ApprovalToSurveys> ApprovalToSurveys { get; set; }
        public virtual DbSet<ApprovalToTasks> ApprovalToTasks { get; set; }
        public virtual DbSet<ApprovalToTrainings> ApprovalToTrainings { get; set; }
        public virtual DbSet<Approvals> Approvals { get; set; }
        public virtual DbSet<Areas> Areas { get; set; }
        public virtual DbSet<AreaSpecialists> AreaSpecialists { get; set; }
        public virtual DbSet<Assesments> Assesments { get; set; }
        public virtual DbSet<AssesmentSkillChecks> AssesmentSkillChecks { get; set; }
        public virtual DbSet<SkillChecks> SkillChecks { get; set; }
        public virtual DbSet<SkillChecksQuestions> SkillChecksQuestions { get; set; }
        public virtual DbSet<SkillChecksScoreConfigs> SkillChecksScoreConfigs { get; set; }
        public virtual DbSet<AssesmentQuestionTypes> AssesmentQuestionTypes { get; set; }
        public virtual DbSet<AssesmentQuestions> AssesmentQuestions { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerTrueFalses> AssesmentQuestionAnswerTrueFalses { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerShortAnswers> AssesmentQuestionAnswerShortAnswers { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerMatrixes> AssesmentQuestionAnswerMatrixes { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerMatrixesX> AssesmentQuestionAnswerMatrixesX { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerMatrixesY> AssesmentQuestionAnswerMatrixesY { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerMatrixesXY> AssesmentQuestionAnswerMatrixesXY { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerHotspots> AssesmentQuestionAnswerHotspots { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerFileUploads> AssesmentQuestionAnswerFileUploads { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerEssays> AssesmentQuestionAnswerEssays { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerDropdowns> AssesmentQuestionAnswerDropdowns { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerChecklists> AssesmentQuestionAnswerChecklists { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerAnswerRatings> AssesmentQuestionAnswerAnswerRatings { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerAnswerRatingRates> AssesmentQuestionAnswerAnswerRatingRates { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerAnswerRatingOptions> AssesmentQuestionAnswerAnswerRatingOptions { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerAnswerMultipleChoices> AssesmentQuestionAnswerAnswerMultipleChoices { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerAnswerMathcings> AssesmentQuestionAnswerAnswerMathcings { get; set; }
        public virtual DbSet<AssesmentQuestionAnswerAnswerImages> AssesmentQuestionAnswerAnswerImages { get; set; }
        public virtual DbSet<LiveAssesmentSkillChecks> LiveAssesmentSkillChecks { get; set; }
        public virtual DbSet<LiveAssesmentSkillCheckScores> LiveAssesmentSkillCheckScores { get; set; }

        public virtual DbSet<LiveAssesmentSkillCheckQuestions> LiveAssesmentSkillCheckQuestions { get; set; }
        public virtual DbSet<Assessments> Assessments { get; set; }
        public virtual DbSet<AssignedLearnings> AssignedLearnings { get; set; }
        public virtual DbSet<BannerTypes> BannerTypes { get; set; }
        public virtual DbSet<Banners> Banners { get; set; }
        public virtual DbSet<Blobs> Blobs { get; set; }
        public virtual DbSet<CalculateLearningQueue> CalculateLearningQueue { get; set; }
        public virtual DbSet<Cfams> Cfams { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Cms_Contents> Cms_Contents { get; set; }
        public virtual DbSet<Cms_Fmbs> Cms_Fmbs { get; set; }
        public virtual DbSet<Cms_Menus> Cms_Menus { get; set; }
        public virtual DbSet<Cms_Operations> Cms_Operations { get; set; }
        public virtual DbSet<Cms_Settings> Cms_Settings { get; set; }
        public virtual DbSet<Cms_SubContents> Cms_SubContents { get; set; }
        public virtual DbSet<Cms_WorkPrincipals> Cms_WorkPrincipals { get; set; }
        public virtual DbSet<CoachBookingSchedules> CoachBookingSchedules { get; set; }
        public virtual DbSet<CoachEmployeeMappings> CoachEmployeeMappings { get; set; }
        public virtual DbSet<CoachRatings> CoachRatings { get; set; }
        public virtual DbSet<CoachSchedules> CoachSchedules { get; set; }
        public virtual DbSet<CoachTopicMappings> CoachTopicMappings { get; set; }
        public virtual DbSet<Coaches> Coaches { get; set; }
        public virtual DbSet<Competencies> Competencies { get; set; }
        public virtual DbSet<CompetencyEvaluationMappings> CompetencyEvaluationMappings { get; set; }
        public virtual DbSet<CompetencyKeyActionMappings> CompetencyKeyActionMappings { get; set; }
        public virtual DbSet<CompetencyTypes> CompetencyTypes { get; set; }
        public virtual DbSet<CourseCategories> CourseCategories { get; set; }
        public virtual DbSet<CourseLearningObjectives> CourseLearningObjectives { get; set; }
        public virtual DbSet<CoursePrerequisiteMappings> CoursePrerequisiteMappings { get; set; }
        public virtual DbSet<CourseTrainingTypeMappings> CourseTrainingTypeMappings { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<DealerPeopleCategories> DealerPeopleCategories { get; set; }
        public virtual DbSet<Dealers> Dealers { get; set; }
        public virtual DbSet<DigitalSignatures> DigitalSignatures { get; set; }
        public virtual DbSet<Dictionaries> Dictionaries { get; set; }
        public virtual DbSet<DictionaryMappings> DictionaryMappings { get; set; }
        public virtual DbSet<Ebadges> Ebadges { get; set; }
        public virtual DbSet<EmailTemplates> EmailTemplates { get; set; }
        public virtual DbSet<EmployeeAccessTimes> EmployeeAccessTimes { get; set; }
        public virtual DbSet<EmployeeBadges> EmployeeBadges { get; set; }
        public virtual DbSet<EmployeeCertificates> EmployeeCertificates { get; set; }
        public virtual DbSet<EmployeeEventMappings> EmployeeEventMappings { get; set; }
        public virtual DbSet<EmployeeHobbyMappings> EmployeeHobbyMappings { get; set; }
        public virtual DbSet<EmployeeInterests> EmployeeInterests { get; set; }
        public virtual DbSet<EmployeeLevels> EmployeeLevels { get; set; }
        public virtual DbSet<EmployeePointHistories> EmployeePointHistories { get; set; }
        public virtual DbSet<EmployeePositionMappings> EmployeePositionMappings { get; set; }
        public virtual DbSet<EmployeeRewardMappings> EmployeeRewardMappings { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<UserFcmTokens> UserFcmTokens { get; set; }
        public virtual DbSet<EmployeeDownload> EmployeeDownload { get; set; }

        public virtual DbSet<EnrollLearningTimes> EnrollLearningTimes { get; set; }
        public virtual DbSet<EnrollLearnings> EnrollLearnings { get; set; }
        public virtual DbSet<EvaluationTypes> EvaluationTypes { get; set; }
        public virtual DbSet<EventCategories> EventCategories { get; set; }
        public virtual DbSet<EventOutletMappings> EventOutletMappings { get; set; }
        public virtual DbSet<EventPositionMappings> EventPositionMappings { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<FileContentTypes> FileContentTypes { get; set; }
        public virtual DbSet<GuideTypes> GuideTypes { get; set; }
        public virtual DbSet<Guides> Guides { get; set; }
        public virtual DbSet<HOGuidelines> HOGuidelines { get; set; }
        public virtual DbSet<Hobbies> Hobbies { get; set; }
        public virtual DbSet<Inboxes> Inboxes { get; set; }
        public virtual DbSet<ItalentReports> ItalentReports { get; set; }
        public virtual DbSet<KeyActions> KeyActions { get; set; }
        public virtual DbSet<KeyPieceOfMindMenus> KeyPieceOfMindMenus { get; set; }
        public virtual DbSet<KeyPieceOfMindTypes> KeyPieceOfMindTypes { get; set; }
        public virtual DbSet<KeyPieceOfMinds> KeyPieceOfMinds { get; set; }
        public virtual DbSet<KodawariBanners> KodawariBanners { get; set; }
        public virtual DbSet<KodawariMenus> KodawariMenus { get; set; }
        public virtual DbSet<KodawariTypes> KodawariTypes { get; set; }
        public virtual DbSet<Kodawaris> Kodawaris { get; set; }
        public virtual DbSet<KodawariDownloads> KodawariDownloads { get; set; }
        public virtual DbSet<LearningHistories> LearningHistories { get; set; }
        public virtual DbSet<LearningTypes> LearningTypes { get; set; }
        public virtual DbSet<Levels> Levels { get; set; }
        public virtual DbSet<MaterialTypes> MaterialTypes { get; set; }
        public virtual DbSet<Menus> Menus { get; set; }
        public virtual DbSet<MobileInboxTypes> MobileInboxTypes { get; set; }
        public virtual DbSet<MobileInboxes> MobileInboxes { get; set; }
        public virtual DbSet<MobilePages> MobilePages { get; set; }
        public virtual DbSet<ModuleTopicMappings> ModuleTopicMappings { get; set; }
        public virtual DbSet<Modules> Modules { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsCategories> NewsCategories { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductCategories> ProductCategories { get; set; }
        public virtual DbSet<ProductCustomerTypes> ProductCustomerTypes { get; set; }
        public virtual DbSet<ProductCustomers> ProductCustomers { get; set; }
        public virtual DbSet<ProductTypes> ProductTypes { get; set; }
        public virtual DbSet<ProductGalleries> ProductGalleries { get; set; }
        public virtual DbSet<ProductPhoto360Mappings> ProductPhoto360Mappings { get; set; }
        public virtual DbSet<ProductPhotos> ProductPhotos { get; set; }
        public virtual DbSet<ProductFAQCategories> ProductFAQCategories { get; set; }
        public virtual DbSet<ProductFAQs> ProductFAQs { get; set; }
        public virtual DbSet<ProductFAQUsers> ProductFAQUsers { get; set; }
        public virtual DbSet<ProductCompetitorMappings> ProductCompetitorMappings { get; set; }
        public virtual DbSet<ProductFeatureMappings> ProductFeatureMappings { get; set; }
        public virtual DbSet<ProductFeatures> ProductFeatures { get; set; }
        public virtual DbSet<ProductFeatureCategories> ProductFeatureCategories { get; set; }
        public virtual DbSet<ProductTips> ProductTips { get; set; }
        public virtual DbSet<ProductTipCategories> ProductTipCategories { get; set; }
        public virtual DbSet<ProductProgramCategories> ProductProgramCategories { get; set; }
        public virtual DbSet<ProductProgramMappings> ProductProgramMappings { get; set; }
        public virtual DbSet<ProductSPWACategories> ProductSPWACategories { get; set; }
        public virtual DbSet<ProductSPWAMappings> ProductSPWAMappings { get; set; }
        public virtual DbSet<Outlets> Outlets { get; set; }
        public virtual DbSet<OnBoardings> OnBoardings { get; set; }
        public virtual DbSet<Announcements> Announcements { get; set; }
        public virtual DbSet<Pages> Pages { get; set; }
        public virtual DbSet<PointTransactionTypes> PointTransactionTypes { get; set; }
        public virtual DbSet<PointTypes> PointTypes { get; set; }
        public virtual DbSet<PopQuizzes> PopQuizzes { get; set; }
        public virtual DbSet<PositionCompentencyMappings> PositionCompentencyMappings { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }
        public virtual DbSet<PrivilegePageMappings> PrivilegePageMappings { get; set; }
        public virtual DbSet<ProgramTypes> ProgramTypes { get; set; }
        public virtual DbSet<Provinces> Provinces { get; set; }
        public virtual DbSet<QuestionTypes> QuestionTypes { get; set; }
        public virtual DbSet<Regions> Regions { get; set; }
        public virtual DbSet<RewardPointTypes> RewardPointTypes { get; set; }
        public virtual DbSet<RewardPoints> RewardPoints { get; set; }
        public virtual DbSet<RewardTypes> RewardTypes { get; set; }
        public virtual DbSet<Rewards> Rewards { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<RotateTeamMembers> RotateTeamMembers { get; set; }
        public virtual DbSet<ServiceTipTypes> ServiceTipTypes { get; set; }
        public virtual DbSet<ServiceTips> ServiceTips { get; set; }
        public virtual DbSet<ServiceTipMenus> ServiceTipMenus { get; set; }
        public virtual DbSet<SetupLearning> SetupLearning { get; set; }
        public virtual DbSet<SetupModules> SetupModules { get; set; }
        public virtual DbSet<SetupTaskCodes> SetupTaskCodes { get; set; }
        public virtual DbSet<SetupTasks> SetupTasks { get; set; }
        public virtual DbSet<StagingActualOrganizationStructure> StagingActualOrganizationStructure { get; set; }
        public virtual DbSet<StagingAfterSalesArea> StagingAfterSalesArea { get; set; }
        public virtual DbSet<StagingDealerCompany> StagingDealerCompany { get; set; }
        public virtual DbSet<StagingDealerEmployee> StagingDealerEmployee { get; set; }
        public virtual DbSet<StagingDealerGroup> StagingDealerGroup { get; set; }
        public virtual DbSet<StagingManpowerPositionType> StagingManpowerPositionType { get; set; }
        public virtual DbSet<StagingManpowerType> StagingManpowerType { get; set; }
        public virtual DbSet<StagingOrganizationObject> StagingOrganizationObject { get; set; }
        public virtual DbSet<StagingOutlet> StagingOutlet { get; set; }
        public virtual DbSet<StagingRegion> StagingRegion { get; set; }
        public virtual DbSet<StagingSalesArea> StagingSalesArea { get; set; }
        public virtual DbSet<StagingUser> StagingUser { get; set; }
        public virtual DbSet<SurveyAnswerDetails> SurveyAnswerDetails { get; set; }
        public virtual DbSet<SurveyAnswers> SurveyAnswers { get; set; }
        public virtual DbSet<SurveyChoices> SurveyChoices { get; set; }
        public virtual DbSet<SurveyMatchingChoices> SurveyMatchingChoices { get; set; }
        public virtual DbSet<SurveyMatrixChoices> SurveyMatrixChoices { get; set; }
        public virtual DbSet<SurveyMatrixQuestions> SurveyMatrixQuestions { get; set; }
        public virtual DbSet<SurveyOutletMappings> SurveyOutletMappings { get; set; }
        public virtual DbSet<SurveyPositionMappings> SurveyPositionMappings { get; set; }
        public virtual DbSet<SurveyQuestionTypes> SurveyQuestionTypes { get; set; }
        public virtual DbSet<SurveyQuestions> SurveyQuestions { get; set; }
        public virtual DbSet<SurveySpecialAnswers> SurveySpecialAnswers { get; set; }
        public virtual DbSet<Surveys> Surveys { get; set; }
        public virtual DbSet<TamemployeeStructure> TamemployeeStructure { get; set; }
        public virtual DbSet<TaskAnswerDetails> TaskAnswerDetails { get; set; }
        public virtual DbSet<TaskAnswers> TaskAnswers { get; set; }
        public virtual DbSet<TaskChecklistChoices> TaskChecklistChoices { get; set; }
        public virtual DbSet<TaskChecklistTypes> TaskChecklistTypes { get; set; }
        public virtual DbSet<TaskEssayTypes> TaskEssayTypes { get; set; }
        public virtual DbSet<TaskFileUploadTypes> TaskFileUploadTypes { get; set; }
        public virtual DbSet<TaskHotSpotAnswers> TaskHotSpotAnswers { get; set; }
        public virtual DbSet<TaskHotSpotTypes> TaskHotSpotTypes { get; set; }
        public virtual DbSet<TaskMatchingChoices> TaskMatchingChoices { get; set; }
        public virtual DbSet<TaskMatchingTypes> TaskMatchingTypes { get; set; }
        public virtual DbSet<TaskMatrixChoices> TaskMatrixChoices { get; set; }
        public virtual DbSet<TaskMatrixQuestions> TaskMatrixQuestions { get; set; }
        public virtual DbSet<TaskMatrixTypes> TaskMatrixTypes { get; set; }
        public virtual DbSet<TaskMultipleChoiceChoices> TaskMultipleChoiceChoices { get; set; }
        public virtual DbSet<TaskMultipleChoiceTypes> TaskMultipleChoiceTypes { get; set; }
        public virtual DbSet<TaskRatingChoices> TaskRatingChoices { get; set; }
        public virtual DbSet<TaskRatingTypes> TaskRatingTypes { get; set; }
        public virtual DbSet<TaskSequenceChoices> TaskSequenceChoices { get; set; }
        public virtual DbSet<TaskSequenceTypes> TaskSequenceTypes { get; set; }
        public virtual DbSet<TaskShortAnswerTypes> TaskShortAnswerTypes { get; set; }
        public virtual DbSet<TaskSpecialAnswers> TaskSpecialAnswers { get; set; }
        public virtual DbSet<TaskTebakGambarPictures> TaskTebakGambarPictures { get; set; }
        public virtual DbSet<TaskTebakGambarTypes> TaskTebakGambarTypes { get; set; }
        public virtual DbSet<TaskTrueFalseTypes> TaskTrueFalseTypes { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<TeamDetails> TeamDetails { get; set; }
        public virtual DbSet<TeamMemberRequests> TeamMemberRequests { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<TimePoints> TimePoints { get; set; }
        public virtual DbSet<Topics> Topics { get; set; }
        public virtual DbSet<TrainingInvitations> TrainingInvitations { get; set; }
        public virtual DbSet<TrainingModuleMappings> TrainingModuleMappings { get; set; }
        public virtual DbSet<TrainingOutletMappings> TrainingOutletMappings { get; set; }
        public virtual DbSet<TrainingPositionMappings> TrainingPositionMappings { get; set; }
        public virtual DbSet<TrainingProcesses> TrainingProcesses { get; set; }
        public virtual DbSet<TrainingRatings> TrainingRatings { get; set; }
        public virtual DbSet<TrainingServiceLevels> TrainingServiceLevels { get; set; }
        public virtual DbSet<TrainingTypes> TrainingTypes { get; set; }
        public virtual DbSet<Trainings> Trainings { get; set; }
        public virtual DbSet<UserLogActivities> UserLogActivities { get; set; }
        public virtual DbSet<HomePage_AllPopularTraining> HomePage_AllPopularTraining { get; set; }
        public virtual DbSet<HomePage_AllLatestTraining> HomePage_AllLatestTraining { get; set; }
        public virtual DbSet<HomePage_AllRecommendedTraining> HomePage_AllRecommendedTraining { get; set; }
        public virtual DbSet<TrainingPositionOnlyViewMappings> TrainingPositionOnlyViewMappings { get; set; }
        public virtual DbSet<TrainingCertifications> TrainingCertifications { get; set; }
        public virtual DbSet<PushNotificationCategories> PushNotificationCategories { get; set; }
        public virtual DbSet<PushNotificationRecipients> PushNotificationRecipients { get; set; }
        public virtual DbSet<PushNotifications> PushNotifications { get; set; }
        public virtual DbSet<FinalScores> FinalScores { get; set; }
        public virtual DbSet<GetAllMyCoachReport> GetAllMyCoachReport { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Accommodations>(entity =>
            {
                entity.Property(e => e.AccommodationId).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<ApprovalHistories>(entity =>
            {
                entity.Property(e => e.ActionAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ActionBy).IsUnicode(false);

                entity.Property(e => e.ApprovalStatus).IsUnicode(false);

                entity.HasOne(d => d.Approval)
                    .WithMany(p => p.ApprovalHistories)
                    .HasForeignKey(d => d.ApprovalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalHistories_Approvals");

                entity.HasOne(d => d.ApprovalStatusNavigation)
                    .WithMany(p => p.ApprovalHistories)
                    .HasForeignKey(d => d.ApprovalStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalHistories_ApprovalStatus");
            });

            modelBuilder.Entity<ApprovalLevels>(entity =>
            {
                entity.Property(e => e.ApprovalLevelId).ValueGeneratedNever();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<ApprovalMappings>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.PageId).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.ApprovalLevel)
                    .WithMany(p => p.ApprovalMappings)
                    .HasForeignKey(d => d.ApprovalLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalMappings_ApprovalLevels");

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.ApprovalMappings)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalMappings_Pages");
            });

            modelBuilder.Entity<ApprovalPositionMappings>(entity =>
            {
                entity.Property(e => e.PositionId).ValueGeneratedNever();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.HasOne(d => d.Position)
                    .WithOne(p => p.ApprovalPositionMappings)
                    .HasForeignKey<ApprovalPositionMappings>(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalPositionMappings_Positions");
            });

            modelBuilder.Entity<ApprovalStatus>(entity =>
            {
                entity.Property(e => e.ApprovalName).IsUnicode(false);
            });

            modelBuilder.Entity<ApprovalToBanners>(entity =>
            {
                entity.Property(e => e.ApprovalId).ValueGeneratedNever();

                entity.HasOne(d => d.Approval)
                    .WithOne(p => p.ApprovalToBanners)
                    .HasForeignKey<ApprovalToBanners>(d => d.ApprovalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToBanners_Approvals");

                entity.HasOne(d => d.Banner)
                    .WithMany(p => p.ApprovalToBanners)
                    .HasForeignKey(d => d.BannerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToBanners_Banners");
            });

            modelBuilder.Entity<ApprovalToCourses>(entity =>
            {
                entity.Property(e => e.ApprovalId).ValueGeneratedNever();

                entity.HasOne(d => d.Approval)
                    .WithOne(p => p.ApprovalToCourses)
                    .HasForeignKey<ApprovalToCourses>(d => d.ApprovalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToCourses_Approvals");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.ApprovalToCourses)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToCourses_Courses");
            });

            modelBuilder.Entity<ApprovalToEvents>(entity =>
            {
                entity.Property(e => e.ApprovalId).ValueGeneratedNever();

                entity.HasOne(d => d.Approval)
                    .WithOne(p => p.ApprovalToEvents)
                    .HasForeignKey<ApprovalToEvents>(d => d.ApprovalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToEvents_Approvals");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.ApprovalToEvents)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToEvents_Events");
            });

            modelBuilder.Entity<ApprovalToGuides>(entity =>
            {
                entity.Property(e => e.ApprovalId).ValueGeneratedNever();

                entity.HasOne(d => d.Approval)
                    .WithOne(p => p.ApprovalToGuides)
                    .HasForeignKey<ApprovalToGuides>(d => d.ApprovalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToGuides_Approvals");

                entity.HasOne(d => d.Guide)
                    .WithMany(p => p.ApprovalToGuides)
                    .HasForeignKey(d => d.GuideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToGuides_Guides");
            });

            modelBuilder.Entity<ApprovalToModules>(entity =>
            {
                entity.Property(e => e.ApprovalId).ValueGeneratedNever();

                entity.HasOne(d => d.Approval)
                    .WithOne(p => p.ApprovalToModules)
                    .HasForeignKey<ApprovalToModules>(d => d.ApprovalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToModules_Approvals");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.ApprovalToModules)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToModules_Modules");
            });

            modelBuilder.Entity<ApprovalToNews>(entity =>
            {
                entity.Property(e => e.ApprovalId).ValueGeneratedNever();

                entity.HasOne(d => d.Approval)
                    .WithOne(p => p.ApprovalToNews)
                    .HasForeignKey<ApprovalToNews>(d => d.ApprovalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToNews_Approvals");

                entity.HasOne(d => d.News)
                    .WithMany(p => p.ApprovalToNews)
                    .HasForeignKey(d => d.NewsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToNews_News");
            });

            modelBuilder.Entity<ApprovalToPopQuizzes>(entity =>
            {
                entity.Property(e => e.ApprovalId).ValueGeneratedNever();

                entity.HasOne(d => d.Approval)
                    .WithOne(p => p.ApprovalToPopQuizzes)
                    .HasForeignKey<ApprovalToPopQuizzes>(d => d.ApprovalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToSetupTasks_Approvals");

                entity.HasOne(d => d.PopQuiz)
                    .WithMany(p => p.ApprovalToPopQuizzes)
                    .HasForeignKey(d => d.PopQuizId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToPopQuizzes_PopQuizzes");
            });

            modelBuilder.Entity<ApprovalToSetupCourses>(entity =>
            {
                entity.Property(e => e.ApprovalId).ValueGeneratedNever();

                entity.HasOne(d => d.Approval)
                    .WithOne(p => p.ApprovalToSetupCourses)
                    .HasForeignKey<ApprovalToSetupCourses>(d => d.ApprovalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToSetupCourses_Approvals");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.ApprovalToSetupCourses)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToSetupCourses_Courses");
            });

            modelBuilder.Entity<ApprovalToSetupModules>(entity =>
            {
                entity.Property(e => e.ApprovalId).ValueGeneratedNever();

                entity.HasOne(d => d.Approval)
                    .WithOne(p => p.ApprovalToSetupModules)
                    .HasForeignKey<ApprovalToSetupModules>(d => d.ApprovalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToSetupModules_Approvals");

                entity.HasOne(d => d.SetupModule)
                    .WithMany(p => p.ApprovalToSetupModules)
                    .HasForeignKey(d => d.SetupModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToSetupModules_SetupModules");
            });

            modelBuilder.Entity<ApprovalToSurveys>(entity =>
            {
                entity.Property(e => e.ApprovalId).ValueGeneratedNever();

                entity.HasOne(d => d.Approval)
                    .WithOne(p => p.ApprovalToSurveys)
                    .HasForeignKey<ApprovalToSurveys>(d => d.ApprovalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToSurveys_Approvals");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.ApprovalToSurveys)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToSurveys_Surveys");
            });

            modelBuilder.Entity<ApprovalToTasks>(entity =>
            {
                entity.Property(e => e.ApprovalId).ValueGeneratedNever();

                entity.HasOne(d => d.Approval)
                    .WithOne(p => p.ApprovalToTasks)
                    .HasForeignKey<ApprovalToTasks>(d => d.ApprovalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToTasks_Approvals");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.ApprovalToTasks)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToTasks_Tasks");
            });

            modelBuilder.Entity<ApprovalToTrainings>(entity =>
            {
                entity.Property(e => e.ApprovalId).ValueGeneratedNever();

                entity.HasOne(d => d.Approval)
                    .WithOne(p => p.ApprovalToTrainings)
                    .HasForeignKey<ApprovalToTrainings>(d => d.ApprovalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToTrainings_Approvals");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.ApprovalToTrainings)
                    .HasForeignKey(d => d.TrainingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalToTrainings_Trainings");
            });

            modelBuilder.Entity<Approvals>(entity =>
            {
                entity.Property(e => e.ActionAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ActionBy).IsUnicode(false);

                entity.Property(e => e.ApprovalTo).IsUnicode(false);

                entity.Property(e => e.ContentCategory).IsUnicode(false);

                entity.Property(e => e.ContentName).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.HasOne(d => d.ActionByNavigation)
                    .WithMany(p => p.ApprovalsActionByNavigation)
                    .HasForeignKey(d => d.ActionBy)
                    .HasConstraintName("FK_Approvals_ActionBy");

                entity.HasOne(d => d.ApprovalMapping)
                    .WithMany(p => p.Approvals)
                    .HasForeignKey(d => d.ApprovalMappingId)
                    .HasConstraintName("FK_Approvals_ApprovalMappings");

                entity.HasOne(d => d.ApprovalStatus)
                    .WithMany(p => p.Approvals)
                    .HasForeignKey(d => d.ApprovalStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Approvals_ApprovalStatus");

                entity.HasOne(d => d.ApprovalToNavigation)
                    .WithMany(p => p.ApprovalsApprovalToNavigation)
                    .HasForeignKey(d => d.ApprovalTo)
                    .HasConstraintName("FK_Approvals_ApprovalTo");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ApprovalsCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Approvals_CreatedBy");
            });

            modelBuilder.Entity<Areas>(entity =>
            {
                entity.Property(e => e.AreaId)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<AreaSpecialists>(entity =>
            {
                entity.Property(e => e.AreaSpecialistId)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AreaSpecialistName).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Assessments>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Publisher).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.Assessments)
                    .HasForeignKey(d => d.BlobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assessments_Blobs");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Assessments)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assessments_Employees");
            });

            modelBuilder.Entity<AssignedLearnings>(entity =>
            {
                entity.Property(e => e.AssignedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AssignedBy).IsUnicode(false);

                entity.Property(e => e.AssignedTo).IsUnicode(false);

                entity.HasOne(d => d.AssignedByNavigation)
                    .WithMany(p => p.AssignedLearningsAssignedByNavigation)
                    .HasForeignKey(d => d.AssignedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignedLearnings_AssignedBy");

                entity.HasOne(d => d.AssignedToNavigation)
                    .WithMany(p => p.AssignedLearningsAssignedToNavigation)
                    .HasForeignKey(d => d.AssignedTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignedLearnings_AssignedTo");

                entity.HasOne(d => d.SetupModule)
                    .WithMany(p => p.AssignedLearnings)
                    .HasForeignKey(d => d.SetupModuleId)
                    .HasConstraintName("FK_AssignedLearnings_SetupModules");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.AssignedLearnings)
                    .HasForeignKey(d => d.TrainingId)
                    .HasConstraintName("FK_AssignedLearnings_Trainings");
            });

            modelBuilder.Entity<BannerTypes>(entity =>
            {
                entity.Property(e => e.BannerTypeId).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Banners>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.BannerType)
                    .WithMany(p => p.Banners)
                    .HasForeignKey(d => d.BannerTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Banners_BannerTypes");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.Banners)
                    .HasForeignKey(d => d.BlobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Banners_Blobs");

                entity.HasOne(d => d.MobilePage)
                    .WithMany(p => p.Banners)
                    .HasForeignKey(d => d.MobilePageId)
                    .HasConstraintName("FK_Banners_MobilePages");
            });

            modelBuilder.Entity<Blobs>(entity =>
            {
                entity.Property(e => e.BlobId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Mime).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<CalculateLearningQueue>(entity =>
            {
                entity.Property(e => e.CalculateLearningQueueId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.EnrollType).IsUnicode(false);

                entity.Property(e => e.FinishedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.EnrollLearning)
                    .WithMany(p => p.CalculateLearningQueue)
                    .HasForeignKey(d => d.EnrollLearningId)
                    .HasConstraintName("FK_CalculateLearningQueue_EnrollLearning");

                entity.HasOne(d => d.SetupModule)
                    .WithMany(p => p.CalculateLearningQueue)
                    .HasForeignKey(d => d.SetupModuleId)
                    .HasConstraintName("FK_CalculateLearningQueue_SetupModules");
            });

            modelBuilder.Entity<Cfams>(entity =>
            {
                entity.Property(e => e.Cfamid)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Cfamname).IsUnicode(false);
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.Property(e => e.CityId)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CityName).IsUnicode(false);

                entity.Property(e => e.ParentCode).IsUnicode(false);
            });

            modelBuilder.Entity<Cms_Fmbs>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Cms_FmbDescription).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.CmsFmbBlob)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_CmsFmbs_Blobs");

                entity.HasOne(d => d.FmbFileBlob)
                    .WithMany(p => p.CmsFmbFileBlob)
                    .HasForeignKey(d => d.CmsFmbFileBlobId)
                    .HasConstraintName("FK_CmsFmbs_CmsFmbFileBlobs");
            });

            modelBuilder.Entity<Cms_WorkPrincipals>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Cms_WorkPrincipalDescription).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.CmsWorkPrincipalBlob)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_CmsWorkPrincipals_Blobs");

                entity.HasOne(d => d.WorkPrincipalFileBlob)
                    .WithMany(p => p.CmsWorkPrincipalFileBlob)
                    .HasForeignKey(d => d.Cms_WorkPrincipalFileBlobId)
                    .HasConstraintName("FK_CmsWorkPrincipals_CmsWorkPrincipalFileBlobs");
            });

            modelBuilder.Entity<Cms_Contents>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Cms_ContentName).IsUnicode(false);

                entity.Property(e => e.Cms_ContentDescription).IsUnicode(false);

                entity.Property(e => e.Cms_ContentCategory).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.CmsContentBlob)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_CmsContents_Blobs");

                entity.HasOne(d => d.ContentVideoBlob)
                    .WithMany(p => p.CmsContentVideoBlob)
                    .HasForeignKey(d => d.CmsContentVideoBlobId)
                    .HasConstraintName("FK_CmsContents_CmsContentVideoBlobs");

                entity.HasOne(d => d.ContentFileBlob)
                    .WithMany(p => p.CmsContentFileBlob)
                    .HasForeignKey(d => d.CmsContentFileBlobId)
                    .HasConstraintName("FK_CmsContents_CmsContentFileBlobs");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Cms_Contents)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_CmsContents_Products");
            });

            modelBuilder.Entity<Cms_Operations>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Cms_OperationDescription).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.CmsOperationBlob)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_CmsOperations_Blobs");

                entity.HasOne(d => d.OperationFileBlob)
                    .WithMany(p => p.CmsOperationFileBlob)
                    .HasForeignKey(d => d.Cms_OperationFileBlobId)
                    .HasConstraintName("FK_CmsOperations_CmsOperationFileBlobs");
            });

            modelBuilder.Entity<Cms_Settings>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Cms_SettingDescription).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.CmsSettingBlob)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_CmsSettings_Blobs");

                entity.HasOne(d => d.SettingFileBlob)
                    .WithMany(p => p.CmsSettingFileBlob)
                    .HasForeignKey(d => d.Cms_SettingFileBlobId)
                    .HasConstraintName("FK_CmsSettings_CmsSettingFileBlobs");
            });

            modelBuilder.Entity<Cms_Menus>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Cms_MenuName).IsUnicode(false);

                entity.Property(e => e.Cms_MenuCategory).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");
            });

            modelBuilder.Entity<Cms_SubContents>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Cms_SubContentName).IsUnicode(false);

                entity.Property(e => e.Cms_SubContentDescription).IsUnicode(false);

                entity.Property(e => e.Cms_SubContentCategory).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.CmsSubContentBlob)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_CmsSubContents_Blobs");

                entity.HasOne(d => d.SubContentVideoBlob)
                    .WithMany(p => p.CmsSubContentVideoBlob)
                    .HasForeignKey(d => d.CmsSubContentVideoBlobId)
                    .HasConstraintName("FK_CmsSubContents_CmsSubContentVideoBlobs");

                entity.HasOne(d => d.SubContentFileBlob)
                    .WithMany(p => p.CmsSubContentFileBlob)
                    .HasForeignKey(d => d.CmsSubContentFileBlobId)
                    .HasConstraintName("FK_CmsSubContents_CmsSubContentFileBlobs");
            });

            modelBuilder.Entity<CoachBookingSchedules>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.HasOne(d => d.CoachSchedule)
                    .WithMany(p => p.CoachBookingSchedules)
                    .HasForeignKey(d => d.CoachScheduleId)
                    .HasConstraintName("FK_CoachBookingSchedules_CoachSchedules");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.CoachBookingSchedules)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CoachBookingSchedules_Employees");
            });

            modelBuilder.Entity<CoachEmployeeMappings>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.EmployeeId });

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.CoachEmployeeMappings)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CoachEmployeeMappings_Coaches");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.CoachEmployeeMappings)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_CoachEmployeeMappings_Teams");
            });

            modelBuilder.Entity<CoachRatings>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.Review).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Coach)
                    .WithMany(p => p.CoachRatings)
                    .HasForeignKey(d => d.CoachId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CoachRatings_Coaches");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.CoachRatings)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CoachRatings_Employees");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.CoachRatings)
                    .HasForeignKey(d => d.TrainingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CoachRatings_Trainings");
            });

            modelBuilder.Entity<CoachSchedules>(entity =>
            {
                entity.Property(e => e.EndDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ScheduleName)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Schedule Name')");

                entity.Property(e => e.StartDateTime).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Coach)
                    .WithMany(p => p.CoachSchedules)
                    .HasForeignKey(d => d.CoachId)
                    .HasConstraintName("FK_CoachSchedules_Coaches");
            });

            modelBuilder.Entity<CoachTopicMappings>(entity =>
            {
                entity.HasKey(e => new { e.CoachId, e.TopicId });

                entity.HasOne(d => d.Coach)
                    .WithMany(p => p.CoachTopicMappings)
                    .HasForeignKey(d => d.CoachId)
                    .HasConstraintName("FK_CoachTopicMappings_Coaches");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.CoachTopicMappings)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK_CoachTopicMappings_Topics");
            });

            modelBuilder.Entity<Coaches>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Coaches)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Coaches_Employees");
            });

            modelBuilder.Entity<Competencies>(entity =>
            {
                entity.Property(e => e.CompetencyDescription).IsUnicode(false);

                entity.Property(e => e.CompetencyName).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.PrefixCode).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.CompetencyType)
                    .WithMany(p => p.Competencies)
                    .HasForeignKey(d => d.CompetencyTypeId)
                    .HasConstraintName("FK_Competencies_CompetencyTypes");
            });

            modelBuilder.Entity<CompetencyEvaluationMappings>(entity =>
            {
                entity.HasKey(e => new { e.CompetencyId, e.EvaluationTypeId });

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Competency)
                    .WithMany(p => p.CompetencyEvaluationMappings)
                    .HasForeignKey(d => d.CompetencyId)
                    .HasConstraintName("FK_CompetencyEvaluationMappings_Competencies");

                entity.HasOne(d => d.EvaluationType)
                    .WithMany(p => p.CompetencyEvaluationMappings)
                    .HasForeignKey(d => d.EvaluationTypeId)
                    .HasConstraintName("FK_CompetencyEvaluationMappings_EvaluationTypes");
            });

            modelBuilder.Entity<CompetencyKeyActionMappings>(entity =>
            {
                entity.HasKey(e => new { e.CompetencyId, e.KeyActionId });

                entity.HasOne(d => d.Competency)
                    .WithMany(p => p.CompetencyKeyActionMappings)
                    .HasForeignKey(d => d.CompetencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompetencyKeyActionMappings_Competencies");

                entity.HasOne(d => d.KeyAction)
                    .WithMany(p => p.CompetencyKeyActionMappings)
                    .HasForeignKey(d => d.KeyActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompetencyKeyActionMappings_KeyActions");
            });

            modelBuilder.Entity<CompetencyTypes>(entity =>
            {
                entity.Property(e => e.CompetencyTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<CourseCategories>(entity =>
            {
                entity.Property(e => e.CourseCategoryName).IsUnicode(false);
            });

            modelBuilder.Entity<CourseLearningObjectives>(entity =>
            {
                entity.HasKey(e => e.LearningObjectiveId)
                    .HasName("PK_LearningObjectives");

                entity.Property(e => e.LearningObjectiveName).IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseLearningObjectives)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseLearningObjectives_Courses");
            });

            modelBuilder.Entity<CoursePrerequisiteMappings>(entity =>
            {
                entity.HasOne(d => d.NextCourse)
                    .WithMany(p => p.CoursePrerequisiteMappingsNextCourse)
                    .HasForeignKey(d => d.NextCourseId)
                    .HasConstraintName("FK_CoursePrerequisiteMappings_NextCourses");

                entity.HasOne(d => d.NextSetupModule)
                    .WithMany(p => p.CoursePrerequisiteMappings)
                    .HasForeignKey(d => d.NextSetupModuleId)
                    .HasConstraintName("FK_CoursePrerequisiteMappings_NextSetupModules");

                entity.HasOne(d => d.PrevCourse)
                    .WithMany(p => p.CoursePrerequisiteMappingsPrevCourse)
                    .HasForeignKey(d => d.PrevCourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CoursePrerequisiteMappings_PrevCourses");
            });

            modelBuilder.Entity<CourseTrainingTypeMappings>(entity =>
            {
                entity.HasKey(e => new { e.CourseId, e.TrainingTypeId });

                entity.Property(e => e.WorkloadRequirement).IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseTrainingTypeMappings)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseTrainingTypeMappings_Courses");

                entity.HasOne(d => d.TrainingType)
                    .WithMany(p => p.CourseTrainingTypeMappings)
                    .HasForeignKey(d => d.TrainingTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseTrainingTypeMappings_TrainingTypes");
            });

            modelBuilder.Entity<Courses>(entity =>
            {
                entity.Property(e => e.CourseDescription).IsUnicode(false);

                entity.Property(e => e.CourseName).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Others).IsUnicode(false);

                entity.Property(e => e.SetupCourseCreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SetupCourseCreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.SetupCourseUpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SetupCourseUpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_Courses_Blobs");

                entity.HasOne(d => d.CourseCategory)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CourseCategoryId)
                    .HasConstraintName("FK_Courses_CourseCategories");

                entity.HasOne(d => d.LearningType)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.LearningTypeId)
                    .HasConstraintName("FK_Courses_LearningTypes");

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.LevelId)
                    .HasConstraintName("FK_Courses_Levels");

                entity.HasOne(d => d.ProgramType)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.ProgramTypeId)
                    .HasConstraintName("FK_Courses_ProgramTypes");
            });

            modelBuilder.Entity<DealerPeopleCategories>(entity =>
            {
                entity.Property(e => e.DealerPeopleCategoryCode)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Dealers>(entity =>
            {
                entity.Property(e => e.DealerId)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.DealerName).IsUnicode(false);
            });

            modelBuilder.Entity<DigitalSignatures>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.DigitalSignatures)
                    .HasForeignKey(d => d.BlobId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_DigitalSignatures_Blobs");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.DigitalSignatures)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_DigitalSignatures_Employees");
            });

            modelBuilder.Entity<Dictionaries>(entity =>
            {
                entity.Property(e => e.DictionaryName).IsUnicode(false);

                entity.Property(e => e.DictionaryManfaat).IsUnicode(false);

                entity.Property(e => e.DictionaryArti).IsUnicode(false);

                entity.Property(e => e.DictionaryFakta).IsUnicode(false);

                entity.Property(e => e.DictionaryBasicOperation).IsUnicode(false);

                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.DictionaryStatus).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.Dictionaries)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_Dictionaries_Blobs");
            });

            modelBuilder.Entity<DictionaryMappings>(entity =>
            {
                entity.HasKey(e => new { e.DictionaryId, e.EmployeeId });

                entity.HasOne(d => d.Dictionary)
                    .WithMany(p => p.DictionaryMappings)
                    .HasForeignKey(d => d.DictionaryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DictionaryMappings_Dictionaries");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.DictionaryMappings)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_DictionaryMappings_Employees");
            });

            modelBuilder.Entity<Ebadges>(entity =>
            {
                entity.Property(e => e.EbadgeName).IsUnicode(false);
            });

            modelBuilder.Entity<EmailTemplates>(entity =>
            {
                entity.Property(e => e.EmailTemplateId).ValueGeneratedNever();

                entity.Property(e => e.Template).IsUnicode(false);

                entity.Property(e => e.Title).IsUnicode(false);
            });

            modelBuilder.Entity<EmployeeAccessTimes>(entity =>
            {
                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeAccessTimes)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeAccessTimes_Employees");
            });

            modelBuilder.Entity<EmployeeBadges>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.HasOne(d => d.Ebadge)
                    .WithMany(p => p.EmployeeBadges)
                    .HasForeignKey(d => d.EbadgeId)
                    .HasConstraintName("FK_EmployeeBadges_EBadges");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeBadges)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeBadges_Employees");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.EmployeeBadges)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK_EmployeeBadges_Topics");
            });

            modelBuilder.Entity<EmployeeCertificates>(entity =>
            {
                entity.HasKey(e => e.EmployeeCertificateId)
                    .HasName("PK__Employee__38D88A304AE974A1");

                entity.Property(e => e.CertificateNumber).IsUnicode(false);

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.Host).IsUnicode(false);

                entity.Property(e => e.Title).IsUnicode(false);

                entity.Property(e => e.TrainingName).IsUnicode(false);

                entity.Property(e => e.Type).IsUnicode(false);

                entity.Property(e => e.Venue).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.EmployeeCertificates)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_EmployeeCertificates_Blobs");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.EmployeeCertificates)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_EmployeeCertificates_CourseId");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeCertificates)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeCertificates_Employees");
            });

            modelBuilder.Entity<EmployeeEventMappings>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.EventId });

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.IsJoined).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSaved).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeEventMappings)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeEventMappings_Employees");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EmployeeEventMappings)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeEventMappings_Events");
            });

            modelBuilder.Entity<EmployeeHobbyMappings>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.HobbyId });

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeHobbyMappings)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeHobbyMappings_Employees");

                entity.HasOne(d => d.Hobby)
                    .WithMany(p => p.EmployeeHobbyMappings)
                    .HasForeignKey(d => d.HobbyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeHobbyMappings_Hobbies");
            });

            modelBuilder.Entity<EmployeeInterests>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.TopicId });

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeInterests)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeInterests_Employees");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.EmployeeInterests)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeInterests_Topics");
            });

            modelBuilder.Entity<EmployeeLevels>(entity =>
            {
                entity.Property(e => e.EmployeeLevelId).ValueGeneratedNever();
            });

            modelBuilder.Entity<EmployeePointHistories>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeePointHistories)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeePointHistories_Employees");

                entity.HasOne(d => d.PointTransactionType)
                    .WithMany(p => p.EmployeePointHistories)
                    .HasForeignKey(d => d.PointTransactionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeePointHistories_PointTransactionTypes");

                entity.HasOne(d => d.RewardPointType)
                    .WithMany(p => p.EmployeePointHistories)
                    .HasForeignKey(d => d.RewardPointTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeePointHistories_RewardPointTypes");
            });

            modelBuilder.Entity<EmployeePositionMappings>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.PositionId });

                entity.Property(e => e.EmployeeId).IsUnicode(false);
                entity.Property(e => e.Information).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeePositionMappings)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_EmployeePositionMappings_Employees");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.EmployeePositionMappings)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK_EmployeePositionMappings_Positions");
            });

            modelBuilder.Entity<EmployeeRewardMappings>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.RewardId });

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.RedeemedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeRewardMappings)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeRewardMappings_Employees");

                entity.HasOne(d => d.Reward)
                    .WithMany(p => p.EmployeeRewardMappings)
                    .HasForeignKey(d => d.RewardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeRewardMappings_Rewards");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.Property(e => e.EmployeeId)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.DealerPeopleCategoryCode).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.EmployeeEmail).IsUnicode(false);

                entity.Property(e => e.EmployeeName).IsUnicode(false);

                entity.Property(e => e.EmployeePhone).IsUnicode(false);

                entity.Property(e => e.EmployeeUsername).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Gender).IsUnicode(false);

                entity.Property(e => e.Ktp).IsUnicode(false);

                entity.Property(e => e.LastSeenAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ManpowerStatusType).IsUnicode(false);

                entity.Property(e => e.Nickname).IsUnicode(false);

                entity.Property(e => e.OutletId).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_Employees_Blobs");

                entity.HasOne(d => d.DealerPeopleCategoryCodeNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DealerPeopleCategoryCode)
                    .HasConstraintName("FK_Employees_DealerPeopleCategories");

                entity.HasOne(d => d.Outlet)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.OutletId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Employees_Outlets");
            });

            modelBuilder.Entity<PushNotifications>(entity =>
            {
                entity.Property(e => e.Guid)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.PushNotifications)
                    .HasForeignKey(d => d.SenderEmployeeId)
                    .HasConstraintName("FK_Push_Employee_Sender");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.PushNotifications)
                    .HasForeignKey(d => d.NotificationCategoryId)
                    .HasConstraintName("FK_Push_Category");
            });

            modelBuilder.Entity<PushNotificationCategories>(entity =>
            {
                entity.Property(e => e.Guid)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).IsUnicode(false);
            });

            modelBuilder.Entity<PushNotificationRecipients>(entity =>
            {
                entity.Property(e => e.Guid)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).IsUnicode(false);

                entity.HasOne(d => d.Notification)
                  .WithMany(p => p.PushNotificationRecipients)
                  .HasForeignKey(d => d.NotificationId)
                  .HasConstraintName("FK_Push");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.PushNotificationRecipients)
                    .HasForeignKey(d => d.RecipientEmployeeId)
                    .HasConstraintName("FK_Push_Employee_Receiver");
            });

            modelBuilder.Entity<EnrollLearningTimes>(entity =>
            {
                entity.HasOne(d => d.EnrollLearning)
                    .WithMany(p => p.EnrollLearningTimes)
                    .HasForeignKey(d => d.EnrollLearningId)
                    .HasConstraintName("FK_EnrollLearningTimes_EnrollLearnings");

                entity.HasOne(d => d.SetupModule)
                    .WithMany(p => p.EnrollLearningTimes)
                    .HasForeignKey(d => d.SetupModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EnrollLearningTimes_SetupModules");
            });

            modelBuilder.Entity<EnrollLearnings>(entity =>
            {
                entity.HasKey(e => e.EnrollLearningId)
                    .HasName("PK_EnrollLearningId");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EnrollLearnings)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_EnrollLearnings_Employees");

                entity.HasOne(d => d.SetupModule)
                    .WithMany(p => p.EnrollLearnings)
                    .HasForeignKey(d => d.SetupModuleId)
                    .HasConstraintName("FK_EnrollLearnings_SetupModules");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.EnrollLearnings)
                    .HasForeignKey(d => d.TrainingId)
                    .HasConstraintName("FK_EnrollLearnings_Trainings");
            });

            modelBuilder.Entity<EvaluationTypes>(entity =>
            {
                entity.Property(e => e.EvaluationTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<EventCategories>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");
            });

            modelBuilder.Entity<EventOutletMappings>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.OutletId });

                entity.Property(e => e.OutletId).IsUnicode(false);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventOutletMappings)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_EventOutletMappings_Events");

                entity.HasOne(d => d.Outlet)
                    .WithMany(p => p.EventOutletMappings)
                    .HasForeignKey(d => d.OutletId)
                    .HasConstraintName("FK_EventOutletMappings_Outlets");
            });

            modelBuilder.Entity<EventPositionMappings>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.PositionId });

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventPositionMappings)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_EventPositionMappings_Events");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.EventPositionMappings)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK_EventPositionMappings_Positions");
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.EndDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.HostName).IsUnicode(false);

                entity.Property(e => e.Location).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Source).IsUnicode(false);

                entity.Property(e => e.StartDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TotalView).HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_Events_Blobs");

                entity.HasOne(d => d.EventCategory)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.EventCategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Events_EventCategories");
            });

            modelBuilder.Entity<FileContentTypes>(entity =>
            {
                entity.HasKey(e => new { e.ContentType, e.Mime });

                entity.Property(e => e.ContentType).IsUnicode(false);

                entity.Property(e => e.Mime).IsUnicode(false);
            });

            modelBuilder.Entity<FinalScores>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<GuideTypes>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Guides>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.Guides)
                    .HasForeignKey(d => d.BlobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Guides_Blobs");

                entity.HasOne(d => d.GuideType)
                    .WithMany(p => p.Guides)
                    .HasForeignKey(d => d.GuideTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Guides_GuideTypes");
            });

            modelBuilder.Entity<HOGuidelines>(entity =>
            {
                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.HOGuidelineTitle).IsUnicode(false);

                entity.Property(e => e.HOGuidelineComment).IsUnicode(false);

                entity.Property(e => e.HOGuideLineStatus).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsApproved).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.HOGuidelines)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_HOGuidelines_Blobs");
            });

            modelBuilder.Entity<Hobbies>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");
            });

            modelBuilder.Entity<Inboxes>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.LinkToPage).IsUnicode(false);

                entity.Property(e => e.Message).IsUnicode(false);

                entity.Property(e => e.Title).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Approval)
                    .WithMany(p => p.Inboxes)
                    .HasForeignKey(d => d.ApprovalId)
                    .HasConstraintName("FK_Inboxes_Approvals");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Inboxes)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inboxes_Employees");
            });

            modelBuilder.Entity<ItalentReports>(entity =>
            {
                entity.HasKey(e => e.ItalentReportId)
                    .HasName("PK_ITalentReportId");

                entity.Property(e => e.ItalentReportId).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Url).IsUnicode(false);
            });

            modelBuilder.Entity<KeyActions>(entity =>
            {
                entity.HasIndex(e => e.KeyActionCode)
                    .HasName("UQ_KeyActions_KeyActionCode")
                    .IsUnique();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.KeyActionCode).IsUnicode(false);

                entity.Property(e => e.KeyActionDescription).IsUnicode(false);

                entity.Property(e => e.KeyActionName).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");
            });

            modelBuilder.Entity<KeyPieceOfMindMenus>(entity =>
            {
                entity.Property(e => e.KeyPieceOfMindMenuName).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<KeyPieceOfMindTypes>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.KeyPieceOfMindTypeName).IsUnicode(false);

                entity.Property(e => e.KeyPieceOfMindTypeDescription).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.KeyPieceOfMindTypes)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_KeyPieceOfMindTypes_Blobs");
            });

            modelBuilder.Entity<KeyPieceOfMinds>(entity =>
            {
                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Cms_Menu)
                    .WithMany(p => p.KeyPieceOfMinds)
                    .HasForeignKey(d => d.Cms_MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KeyPieceOfMinds_CmsMenus");

                entity.HasOne(d => d.Cms_Content)
                    .WithMany(p => p.KeyPieceOfMinds)
                    .HasForeignKey(d => d.Cms_ContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KeyPieceOfMinds_CmsContents");

                entity.HasOne(d => d.KeyPieceOfMindType)
                    .WithMany(p => p.KeyPieceOfMinds)
                    .HasForeignKey(d => d.KeyPieceOfMindTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KeyPieceOfMinds_KeyPieceOfMindTypes");

                entity.HasOne(d => d.KeyPieceOfMindMenu)
                    .WithMany(p => p.KeyPieceOfMinds)
                    .HasForeignKey(d => d.KeyPieceOfMindMenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KeyPieceOfMinds_KeyPieceOfMindMenus");

                entity.HasOne(d => d.Cms_SubContent)
                    .WithMany(p => p.KeyPieceOfMinds)
                    .HasForeignKey(d => d.Cms_SubContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KeyPieceOfMinds_Cms_SubContents");
            });

            modelBuilder.Entity<Kodawaris>(entity =>
            {
                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Cms_Menu)
                    .WithMany(p => p.Kodawaris)
                    .HasForeignKey(d => d.Cms_MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Kodawaris_CmsMenus");

                entity.HasOne(d => d.Cms_Content)
                    .WithMany(p => p.Kodawaris)
                    .HasForeignKey(d => d.Cms_ContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Kodawaris_CmsContents");

                entity.HasOne(d => d.KodawariType)
                    .WithMany(p => p.Kodawaris)
                    .HasForeignKey(d => d.KodawariTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Kodawaris_KodawariTypes");

                entity.HasOne(d => d.KodawariMenu)
                    .WithMany(p => p.Kodawaris)
                    .HasForeignKey(d => d.KodawariMenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Kodawaris_KodawariMenus");

                entity.HasOne(d => d.Cms_SubContent)
                    .WithMany(p => p.Kodawaris)
                    .HasForeignKey(d => d.Cms_SubContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Kodawaris_Cms_SubContents");
            });

            modelBuilder.Entity<KodawariMenus>(entity =>
            {
                entity.Property(e => e.KodawariMenuName).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<KodawariTypes>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.KodawariTypeName).IsUnicode(false);

                entity.Property(e => e.KodawariTypeDescription).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.KodawariTypes)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_KodawariTypes_Blobs");
            });

            modelBuilder.Entity<KodawariBanners>(entity =>
            {
                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.KodawariBannerTitle).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.KodawariBanners)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_KodawariBanners_Blobs");
            });

            modelBuilder.Entity<KodawariDownloads>(entity =>
            {
                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDownloadable).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.KodawariDownloadTitle).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.KodawariDownloads)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_KodawariDownloads_Blobs");

                entity.HasOne(d => d.KodawariMenu)
                    .WithMany(p => p.KodawariDownloads)
                    .HasForeignKey(d => d.KodawariMenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KodawariDownloads_KodawariMenus");
            });

            modelBuilder.Entity<LearningHistories>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.LearningHistories)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LearningHistories_Employees");

                entity.HasOne(d => d.PopQuiz)
                    .WithMany(p => p.LearningHistories)
                    .HasForeignKey(d => d.PopQuizId)
                    .HasConstraintName("FK_LearningHistories_PopQuizzes");

                entity.HasOne(d => d.SetupModule)
                    .WithMany(p => p.LearningHistories)
                    .HasForeignKey(d => d.SetupModuleId)
                    .HasConstraintName("FK_LearningHistories_SetupModules");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.LearningHistories)
                    .HasForeignKey(d => d.TrainingId)
                    .HasConstraintName("FK_LearningHistories_Trainings");
            });

            modelBuilder.Entity<LearningTypes>(entity =>
            {
                entity.Property(e => e.LearningName).IsUnicode(false);
            });

            modelBuilder.Entity<Levels>(entity =>
            {
                entity.Property(e => e.LevelName).IsUnicode(false);
            });

            modelBuilder.Entity<MaterialTypes>(entity =>
            {
                entity.Property(e => e.MaterialTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<Menus>(entity =>
            {
                entity.Property(e => e.MenuId)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<MobileInboxTypes>(entity =>
            {
                entity.Property(e => e.MobileInboxTypeId).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<MobileInboxes>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.Message).IsUnicode(false);

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.ResignEmployeeId).IsUnicode(false);

                entity.Property(e => e.Title).IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.MobileInboxesEmployee)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MobileInboxes_Employees");

                entity.HasOne(d => d.MobileInboxType)
                    .WithMany(p => p.MobileInboxes)
                    .HasForeignKey(d => d.MobileInboxTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MobileInboxes_MobileInboxTypes");

                entity.HasOne(d => d.ResignEmployee)
                    .WithMany(p => p.MobileInboxesResignEmployee)
                    .HasForeignKey(d => d.ResignEmployeeId)
                    .HasConstraintName("FK_MobileInboxes_ResignEmployees");

                entity.HasOne(d => d.RotateTeamMember)
                    .WithMany(p => p.MobileInboxes)
                    .HasForeignKey(d => d.RotateTeamMemberId)
                    .HasConstraintName("FK_MobileInboxes_RotateTeams");

                entity.HasOne(d => d.TeamMemberRequest)
                    .WithMany(p => p.MobileInboxes)
                    .HasForeignKey(d => d.TeamMemberRequestId)
                    .HasConstraintName("FK_MobileInboxes_TeamMemberRequests");
            });

            modelBuilder.Entity<MobilePages>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Route).IsUnicode(false);
            });

            modelBuilder.Entity<ModuleTopicMappings>(entity =>
            {
                entity.HasKey(e => new { e.ModuleId, e.TopicId });

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.ModuleTopicMappings)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuleTopicMappings_Modules");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.ModuleTopicMappings)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuleTopicMappings_Topics");
            });

            modelBuilder.Entity<Modules>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.MaterialLink).IsUnicode(false);

                entity.Property(e => e.ModuleDescription).IsUnicode(false);

                entity.Property(e => e.ModuleName).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.ModulesBlob)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_Modules_Blobs");

                entity.HasOne(d => d.MaterialBlob)
                    .WithMany(p => p.ModulesMaterialBlob)
                    .HasForeignKey(d => d.MaterialBlobId)
                    .HasConstraintName("FK_Modules_MaterialBlobs");

                entity.HasOne(d => d.MaterialType)
                    .WithMany(p => p.Modules)
                    .HasForeignKey(d => d.MaterialTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Modules_MaterialTypes");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.Author).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Link).IsUnicode(false);

                entity.Property(e => e.Title).IsUnicode(false);

                entity.Property(e => e.TotalView).HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.FileBlob)
                    .WithMany(p => p.NewsFileBlob)
                    .HasForeignKey(d => d.FileBlobId)
                    .HasConstraintName("FK__News__FileBlobId__63D8CE75");

                entity.HasOne(d => d.NewsCategory)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.NewsCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__News__NewsCatego__61F08603");

                entity.HasOne(d => d.ThumbnailBlob)
                    .WithMany(p => p.NewsThumbnailBlob)
                    .HasForeignKey(d => d.ThumbnailBlobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__News__ThumbnailB__62E4AA3C");
            });

            modelBuilder.Entity<NewsCategories>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.Property(e => e.ProductName).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsCompetitor).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_Products_Blobs");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK_Products_ProductCategories");
            });

            modelBuilder.Entity<ProductCategories>(entity =>
            {
                entity.Property(e => e.ProductCategoryName).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<ProductTypes>(entity =>
            {
                entity.Property(e => e.ProductTypeName).IsUnicode(false);

                entity.Property(e => e.ProductTypeSalesTalk).IsUnicode(false);

                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductTypes)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductTypes_Products");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.ProductTypes)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_ProductTypes_Blobs");
            });

            modelBuilder.Entity<ProductTipCategories>(entity =>
            {
                entity.Property(e => e.ProductTipCategoryName).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductTipCategories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductTipCategories_Products");
            });

            modelBuilder.Entity<ProductFAQCategories>(entity =>
            {
                entity.Property(e => e.ProductFaqCategoryName).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductFAQCategories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFAQCategories_Products");
            });

            modelBuilder.Entity<ProductFAQUsers>(entity =>
            {
                entity.Property(e => e.ProductFAQUserQuestion).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductFaqUsers)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFAQUsers_Products");

                entity.HasOne(d => d.ProductFAQCategory)
                    .WithMany(p => p.ProductFaqUsers)
                    .HasForeignKey(d => d.ProductFaqCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFAQUsers_ProductFAQCategories");
            });

            modelBuilder.Entity<ProductTips>(entity =>
            {
                entity.Property(e => e.ProductTipTitle).IsUnicode(false);

                entity.Property(e => e.ProductTipDescription).IsUnicode(false);

                entity.Property(e => e.IsApproved).IsUnicode(false);

                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductTips)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductTips_Products");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.ProductTips)
                    .HasForeignKey(d => d.BlobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductTips_Blobs");

                entity.HasOne(d => d.ProductTipCategory)
                    .WithMany(p => p.ProductTips)
                    .HasForeignKey(d => d.ProductTipCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductTips_ProductTipCategories");

                entity.HasOne(d => d.Outlet)
                    .WithMany(p => p.ProductTips)
                    .HasForeignKey(d => d.OutletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductTips_Outlets");
            });

            modelBuilder.Entity<ProductFAQs>(entity =>
            {
                entity.Property(e => e.ProductFaqTitle).IsUnicode(false);

                entity.Property(e => e.ProductFaqDescription).IsUnicode(false);

                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductFaqs)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFAQs_Products");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.ProductFaqs)
                    .HasForeignKey(d => d.BlobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFAQs_Blobs");

                entity.HasOne(d => d.ProductFAQCategory)
                    .WithMany(p => p.ProductFaqs)
                    .HasForeignKey(d => d.ProductFaqCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFAQs_ProductFAQCategories");
            });

            modelBuilder.Entity<ProductCustomerTypes>(entity =>
            {
                entity.Property(e => e.ProductCustomerTypeName).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<ProductCustomers>(entity =>
            {
                entity.Property(e => e.ProductCustomerStatus).IsUnicode(false);

                entity.Property(e => e.ProductCustomerPenghasilan).IsUnicode(false);

                entity.Property(e => e.ProductCustomerKebutuhan).IsUnicode(false);

                entity.Property(e => e.ProductCustomerUmur).IsUnicode(false);

                entity.Property(e => e.ProductCustomerPekerjaan).IsUnicode(false);

                entity.Property(e => e.ProductCustomerPrevVehicle).IsUnicode(false);

                entity.Property(e => e.ProductCustomerProspectSource).IsUnicode(false);

                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductCustomers)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCustomers_Products");

                entity.HasOne(d => d.ProductCustomerType)
                    .WithMany(p => p.ProductCustomers)
                    .HasForeignKey(d => d.ProductCustomerTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCustomers_ProductCustomerTypes");

            });

            modelBuilder.Entity<ProductGalleries>(entity =>
            {
                entity.Property(e => e.ProductGalleryColorCode).IsUnicode(false);

                entity.Property(e => e.ProductGalleryColorName).IsUnicode(false);

                entity.Property(e => e.IsApproved).IsUnicode(false);

                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsColor).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductGalleries)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductGalleries_Products");

                entity.HasOne(d => d.ProductTypes)
                    .WithMany(p => p.ProductGalleries)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductGalleries_ProductTypes");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.ProductGalleries)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_ProductGalleries_Blobs");
            });

            modelBuilder.Entity<ProductPhoto360Mappings>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ProductPhotoId });

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPhoto360Mappings)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductPhoto360Mappings_Products");

                entity.HasOne(d => d.ProductPhoto)
                    .WithMany(p => p.ProductPhoto360Mappings)
                    .HasForeignKey(d => d.ProductPhotoId)
                    .HasConstraintName("FK_ProductPhoto360Mappings_ProductPhotos");
            });

            modelBuilder.Entity<ProductCompetitorMappings>(entity =>
            {
                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductMappings)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCompetitorMappings_Products");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.ProductTypeMappings)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCompetitorMappings_ProductTypes");

                entity.HasOne(d => d.ProductCompetitor)
                    .WithMany(p => p.ProductCompetitorMappings)
                    .HasForeignKey(d => d.ProductCompetitorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCompetitorMappings_ProductCompetitors");

                entity.HasOne(d => d.ProductCompetitorType)
                    .WithMany(p => p.ProductTypeCompetitorMappings)
                    .HasForeignKey(d => d.ProductCompetitorTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCompetitorMappings_ProductCompetitorTypes");

            });

            modelBuilder.Entity<ProductPhotos>(entity =>
            {
                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Is360Photo).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.ProductPhotos)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_ProductPhotos_Blobs");
            });

            modelBuilder.Entity<ProductFeatureCategories>(entity =>
            {
                entity.Property(e => e.ProductFeatureCategoryName).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<ProductFeatures>(entity =>
            {
                entity.Property(e => e.ProductFeatureName).IsUnicode(false);

                entity.Property(e => e.IsApproved).IsUnicode(false);

                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.ProductFeatures)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_ProductFeatures_Blobs");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductFeatures)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFeatures_Products");
            });

            modelBuilder.Entity<ProductFeatureMappings>(entity =>
            {
                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductFeatureMappingApprovalStatus).IsUnicode(false);

                entity.Property(e => e.EnumContributorFlagging).IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductFeatureMappings)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFeatureMappings_Products");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.ProductFeatureMappings)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFeatureMappings_ProductTypes");

                entity.HasOne(d => d.ProductFeature)
                    .WithMany(p => p.ProductFeatureMappings)
                    .HasForeignKey(d => d.ProductFeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFeatureMappings_ProductFeatures");

                entity.HasOne(d => d.ProductFeatureCategory)
                    .WithMany(p => p.ProductFeatureMappings)
                    .HasForeignKey(d => d.ProductFeatureCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFeatureMappings_ProductFeatureCategories");

                entity.HasOne(d => d.Cms_Fmb)
                    .WithMany(p => p.ProductFeatureMappings)
                    .HasForeignKey(d => d.Cms_FmbId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFeatureMappings_CmsFmbs");

                entity.HasOne(d => d.Cms_WorkPrincipal)
                    .WithMany(p => p.ProductFeatureMappings)
                    .HasForeignKey(d => d.Cms_WorkPrincipalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFeatureMappings_CmsWorkPrincipals");

                entity.HasOne(d => d.Cms_Content)
                    .WithMany(p => p.ProductFeatureMappings)
                    .HasForeignKey(d => d.Cms_ContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFeatureMappings_CmsContents");

                entity.HasOne(d => d.Cms_Operation)
                    .WithMany(p => p.ProductFeatureMappings)
                    .HasForeignKey(d => d.Cms_OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFeatureMappings_CmsOperations");

                entity.HasOne(d => d.Cms_Setting)
                    .WithMany(p => p.ProductFeatureMappings)
                    .HasForeignKey(d => d.Cms_SettingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductFeatureMappings_CmsSettings");
            });

            modelBuilder.Entity<ProductProgramCategories>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductProgramCategoryName).IsUnicode(false);

                entity.Property(e => e.ProductProgramCategoryDescription).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.ProductProgramCategories)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_ProductProgramCategories_Blobs");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductProgramCategories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductProgramCategories_Products");
            });

            modelBuilder.Entity<ProductProgramMappings>(entity =>
            {
                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Cms_Menu)
                    .WithMany(p => p.ProductProgramMappings)
                    .HasForeignKey(d => d.Cms_MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductProgramMappings_CmsMenus");

                entity.HasOne(d => d.Cms_Content)
                    .WithMany(p => p.ProductProgramMappings)
                    .HasForeignKey(d => d.Cms_ContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductProgramMappings_CmsContents");

                entity.HasOne(d => d.ProductProgramCategory)
                    .WithMany(p => p.ProductProgramMappings)
                    .HasForeignKey(d => d.ProductProgramCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductProgramMappings_ProductProgramCategories");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductProgramMappings)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductProgramMappings_Products");
            });

            modelBuilder.Entity<ProductSPWACategories>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductSPWACategoryName).IsUnicode(false);

                entity.Property(e => e.ProductSPWACategoryDescription).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.ProductSPWACategories)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_ProductSPWACategories_Blobs");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductSPWACategories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductSPWACategories_Products");
            });

            modelBuilder.Entity<ProductSPWAMappings>(entity =>
            {
                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Cms_Menu)
                    .WithMany(p => p.ProductSPWAMappings)
                    .HasForeignKey(d => d.Cms_MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductSPWAMappings_CmsMenus");

                entity.HasOne(d => d.Cms_Content)
                    .WithMany(p => p.ProductSPWAMappings)
                    .HasForeignKey(d => d.Cms_ContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductSPWAMappings_CmsContents");

                entity.HasOne(d => d.ProductSPWACategory)
                    .WithMany(p => p.ProductSPWAMappings)
                    .HasForeignKey(d => d.ProductSPWACategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductSPWAMappings_ProductSPWACategories");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductSPWAMappings)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductSPWAMappings_Products");
            });

            modelBuilder.Entity<Outlets>(entity =>
            {
                entity.Property(e => e.OutletId)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AreaId).IsUnicode(false);

                entity.Property(e => e.Cfamid).IsUnicode(false);

                entity.Property(e => e.CityId).IsUnicode(false);

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.DealerId).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.OutletCode).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.ProvinceId).IsUnicode(false);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Outlets)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK_Outlets_Areas");

                entity.HasOne(d => d.AreaSpecialist)
                    .WithMany(p => p.Outlets)
                    .HasForeignKey(d => d.AreaSpecialistId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Outlets_AreaSpecialists");

                entity.HasOne(d => d.Cfam)
                    .WithMany(p => p.Outlets)
                    .HasForeignKey(d => d.Cfamid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Outlets_CFAMs");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Outlets)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Outlets_Cities");

                entity.HasOne(d => d.Dealer)
                    .WithMany(p => p.Outlets)
                    .HasForeignKey(d => d.DealerId)
                    .HasConstraintName("FK_Outlets_Dealers");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Outlets)
                    .HasForeignKey(d => d.ProvinceId)
                    .HasConstraintName("FK_Outlets_Provinces");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Outlets)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Outlets_Region");
            });

            modelBuilder.Entity<OnBoardings>(entity =>
            {
                entity.Property(e => e.OnBoardingID)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).IsUnicode(false);
                entity.Property(e => e.SectionNumber).IsUnicode(false);
                entity.Property(e => e.OnBoardingFileURL).IsUnicode(false);
            });


            modelBuilder.Entity<Announcements>(entity =>
            {
                entity.Property(e => e.AnnouncementID)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).IsUnicode(false);
                entity.Property(e => e.Title).IsUnicode(false);
                entity.Property(e => e.AnnouncementFileID).IsUnicode(false);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");
            });

            modelBuilder.Entity<Pages>(entity =>
            {
                entity.Property(e => e.PageId)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.MenuId).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.Pages)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_Pages_Menus");
            });

            modelBuilder.Entity<PointTransactionTypes>(entity =>
            {
                entity.Property(e => e.PointTransactionTypeId).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<PointTypes>(entity =>
            {
                entity.Property(e => e.PointTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<PopQuizzes>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.PopQuizName).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");
            });

            modelBuilder.Entity<PositionCompentencyMappings>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Priority).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Competency)
                    .WithMany(p => p.PositionCompentencyMappings)
                    .HasForeignKey(d => d.CompetencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PositionCompentencyMappings_Competencies");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.PositionCompentencyMappings)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PositionCompentencyMappings_Positions");
            });

            modelBuilder.Entity<Positions>(entity =>
            {
                entity.Property(e => e.PositionCode)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.PositionDescription).IsUnicode(false);

                entity.Property(e => e.PositionName).IsUnicode(false);
            });

            modelBuilder.Entity<PrivilegePageMappings>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.PageId).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.PrivilegePageMappings)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrivilegePageMappings_Pages");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PrivilegePageMappings)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrivilegePageMappings_Roles");
            });

            modelBuilder.Entity<ProgramTypes>(entity =>
            {
                entity.Property(e => e.ProgramTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<Provinces>(entity =>
            {
                entity.Property(e => e.ProvinceId)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ParentCode).IsUnicode(false);

                entity.Property(e => e.ProvinceName).IsUnicode(false);
            });

            modelBuilder.Entity<QuestionTypes>(entity =>
            {
                entity.Property(e => e.QuestionTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<Regions>(entity =>
            {
                entity.Property(e => e.RegionName).IsUnicode(false);
            });

            modelBuilder.Entity<RewardPointTypes>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<RewardPoints>(entity =>
            {
                entity.HasOne(d => d.Reward)
                    .WithMany(p => p.RewardPoints)
                    .HasForeignKey(d => d.RewardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RewardPoints_Rewards");

                entity.HasOne(d => d.RewardPointType)
                    .WithMany(p => p.RewardPoints)
                    .HasForeignKey(d => d.RewardPointTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RewardPoints_RewardPointTypes");
            });

            modelBuilder.Entity<RewardTypes>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Rewards>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.HowToUse).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.TermsAndConditions).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Coach)
                    .WithMany(p => p.Rewards)
                    .HasForeignKey(d => d.CoachId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Rewards_Coaches");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Rewards)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_Rewards_Events");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.Rewards)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Rewards_Modules");

                entity.HasOne(d => d.RewardType)
                    .WithMany(p => p.Rewards)
                    .HasForeignKey(d => d.RewardTypeId)
                    .HasConstraintName("FK_Rewards_RewardTypes");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.DealerPeopleCategoryCode).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.DealerPeopleCategoryCodeNavigation)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.DealerPeopleCategoryCode)
                    .HasConstraintName("FK_Roles_DealerPeopleCategories");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Roles_Positions");
            });

            modelBuilder.Entity<RotateTeamMembers>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ApprovalStatus)
                    .WithMany(p => p.RotateTeamMembers)
                    .HasForeignKey(d => d.ApprovalStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RotateTeamMembers_ApprovalStatus");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.RotateTeamMembers)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RotateTeamMembers_Employees");

                entity.HasOne(d => d.NextTeam)
                    .WithMany(p => p.RotateTeamMembersNextTeam)
                    .HasForeignKey(d => d.NextTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RotateTeamMembers_NextTeams");

                entity.HasOne(d => d.PreviousTeam)
                    .WithMany(p => p.RotateTeamMembersPreviousTeam)
                    .HasForeignKey(d => d.PreviousTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RotateTeamMembers_PrevTeams");
            });

            modelBuilder.Entity<ServiceTipTypes>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.ServiceTipTypeName).IsUnicode(false);

                entity.Property(e => e.ServiceTipTypeDescription).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.ServiceTipTypes)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_ServiceTipTypes_Blobs");
            });

            modelBuilder.Entity<ServiceTips>(entity =>
            {
                entity.Property(e => e.ApprovedAt).ValueGeneratedNever().HasDefaultValue(null);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Cms_Menu)
                    .WithMany(p => p.ServiceTips)
                    .HasForeignKey(d => d.Cms_MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceTips_CmsMenus");

                entity.HasOne(d => d.Cms_Content)
                    .WithMany(p => p.ServiceTips)
                    .HasForeignKey(d => d.Cms_ContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceTips_CmsContents");

                entity.HasOne(d => d.ServiceTipType)
                    .WithMany(p => p.ServiceTips)
                    .HasForeignKey(d => d.ServiceTipTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceTips_ServiceTipTypes");

                entity.HasOne(d => d.Cms_SubContent)
                    .WithMany(p => p.ServiceTips)
                    .HasForeignKey(d => d.Cms_SubContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceTips_Cms_SubContents");
            });

            modelBuilder.Entity<ServiceTipMenus>(entity =>
            {
                entity.Property(e => e.ServiceTipMenuName).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<SetupLearning>(entity =>
            {
                entity.Property(e => e.ApprovalStatus).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LearningCategoryName).IsUnicode(false);

                entity.Property(e => e.LearningName).IsUnicode(false);

                entity.Property(e => e.ProgramTypeName).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.SetupLearning)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__SetupLear__Cours__220B0B18");

                entity.HasOne(d => d.PopQuiz)
                    .WithMany(p => p.SetupLearning)
                    .HasForeignKey(d => d.PopQuizId)
                    .HasConstraintName("FK__SetupLear__PopQu__2116E6DF");

                entity.HasOne(d => d.SetupModule)
                    .WithMany(p => p.SetupLearning)
                    .HasForeignKey(d => d.SetupModuleId)
                    .HasConstraintName("FK_SetupLearning_SetupModules");
            });

            modelBuilder.Entity<SetupModules>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.SetupModules)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SetupModules_Courses");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.SetupModules)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SetupModules_Modules");

                entity.HasOne(d => d.TimePoint)
                    .WithMany(p => p.SetupModules)
                    .HasForeignKey(d => d.TimePointId)
                    .HasConstraintName("FK_SetupModules_TimePoints");

                entity.HasOne(d => d.TrainingType)
                    .WithMany(p => p.SetupModules)
                    .HasForeignKey(d => d.TrainingTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SetupModules_TrainingTypes");
            });

            modelBuilder.Entity<SetupTaskCodes>(entity =>
            {
                entity.HasKey(e => new { e.SetupTaskId, e.TaskId });

                entity.HasOne(d => d.SetupTask)
                    .WithMany(p => p.SetupTaskCodes)
                    .HasForeignKey(d => d.SetupTaskId)
                    .HasConstraintName("FK_SetupTaskCodes_SetupTasks");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.SetupTaskCodes)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SetupTaskCodes_Tasks");
            });

            modelBuilder.Entity<SetupTasks>(entity =>
            {
                entity.HasOne(d => d.Competency)
                    .WithMany(p => p.SetupTasks)
                    .HasForeignKey(d => d.CompetencyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SetupTasks_Competencies");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.SetupTasks)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SetupTasks_Modules");

                entity.HasOne(d => d.PopQuiz)
                    .WithMany(p => p.SetupTasks)
                    .HasForeignKey(d => d.PopQuizId)
                    .HasConstraintName("FK_SetupTasks_PopQuizzes");

                entity.HasOne(d => d.SetupModule)
                    .WithMany(p => p.SetupTasks)
                    .HasForeignKey(d => d.SetupModuleId)
                    .HasConstraintName("FK_SetupTasks_SetupModules");
            });

            modelBuilder.Entity<StagingActualOrganizationStructure>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.EmployeeGroup).IsUnicode(false);

                entity.Property(e => e.EmployeeGroupText).IsUnicode(false);

                entity.Property(e => e.EmployeeSubgroup).IsUnicode(false);

                entity.Property(e => e.EmployeeSubgroupText).IsUnicode(false);

                entity.Property(e => e.EnterUserName).IsUnicode(false);

                entity.Property(e => e.JobCode).IsUnicode(false);

                entity.Property(e => e.JobName).IsUnicode(false);

                entity.Property(e => e.LastChgUserName).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.NoReg).IsUnicode(false);

                entity.Property(e => e.ObjectDescription).IsUnicode(false);

                entity.Property(e => e.OrgCode).IsUnicode(false);

                entity.Property(e => e.OrgName).IsUnicode(false);

                entity.Property(e => e.ParentOrgCode).IsUnicode(false);

                entity.Property(e => e.PersonalArea).IsUnicode(false);

                entity.Property(e => e.PersonalSubarea).IsUnicode(false);

                entity.Property(e => e.PostCode).IsUnicode(false);

                entity.Property(e => e.PostName).IsUnicode(false);

                entity.Property(e => e.Service).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);

                entity.Property(e => e.Structure).IsUnicode(false);

                entity.Property(e => e.ValidationStatus).IsUnicode(false);

                entity.Property(e => e.VersionFlag).IsUnicode(false);

                entity.Property(e => e.VersionName).IsUnicode(false);

                entity.Property(e => e.WorkContract).IsUnicode(false);

                entity.Property(e => e.WorkContractText).IsUnicode(false);
            });

            modelBuilder.Entity<StagingAfterSalesArea>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__StagingA__A25C5AA63B389B6B");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<StagingDealerCompany>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__StagingD__A25C5AA60D6ECC5A");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<StagingDealerEmployee>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__StagingD__A25C5AA63850C09D");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.Ktp).IsUnicode(false);

                entity.Property(e => e.LastManpowerPositionTypeId).IsUnicode(false);

                entity.Property(e => e.ManpowerStatusTypeId).IsUnicode(false);

                entity.Property(e => e.ManpowerTypeId).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.Sex).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<StagingDealerGroup>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__StagingD__A25C5AA6992F47B1");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.DealerGroupCode).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<StagingManpowerPositionType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__StagingM__A25C5AA6F362A55F");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<StagingManpowerType>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<StagingOrganizationObject>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Abbreviation).IsUnicode(false);

                entity.Property(e => e.ObjectDescription).IsUnicode(false);

                entity.Property(e => e.ObjectId).IsUnicode(false);

                entity.Property(e => e.ObjectText).IsUnicode(false);

                entity.Property(e => e.ObjectType).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<StagingOutlet>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__StagingO__A25C5AA6DBBC51C9");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.OutletCode).IsUnicode(false);

                entity.Property(e => e.OutletFunctionId).IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<StagingRegion>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__StagingR__A25C5AA6E838ACC4");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RegionType).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<StagingSalesArea>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__StagingS__A25C5AA66C62D791");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<StagingUser>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.NoReg).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<SurveyAnswerDetails>(entity =>
            {
                entity.Property(e => e.Answer).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.SurveyAnswerDetails)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_SurveyAnswerDetails_Blobs");

                entity.HasOne(d => d.SurveyAnswer)
                    .WithMany(p => p.SurveyAnswerDetails)
                    .HasForeignKey(d => d.SurveyAnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyAnswerDetails_SurveyAnswers");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyAnswerDetails)
                    .HasForeignKey(d => d.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyAnswerDetails_SurveyQuestions");
            });

            modelBuilder.Entity<SurveyAnswers>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.SurveyAnswers)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyAnswers_Employees");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyAnswers)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyAnswers_Surveys");
            });

            modelBuilder.Entity<SurveyChoices>(entity =>
            {
                entity.Property(e => e.Choice).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.SurveyChoices)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_SurveyChoices_Blobs");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyChoices)
                    .HasForeignKey(d => d.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyChoices_SurveyQuestions");
            });

            modelBuilder.Entity<SurveyMatchingChoices>(entity =>
            {
                entity.Property(e => e.SurveyMatchingChoiceCode).IsUnicode(false);

                entity.Property(e => e.Text).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.SurveyMatchingChoices)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_SurveyMatchingChoices_Blobs");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyMatchingChoices)
                    .HasForeignKey(d => d.SurveyQuestionId)
                    .HasConstraintName("FK_SurveyMatchingChoices_SurveyQuestions");
            });

            modelBuilder.Entity<SurveyMatrixChoices>(entity =>
            {
                entity.Property(e => e.Text).IsUnicode(false);

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyMatrixChoices)
                    .HasForeignKey(d => d.SurveyQuestionId)
                    .HasConstraintName("FK_SurveyMatrixChoices_SurveyQuestions");
            });

            modelBuilder.Entity<SurveyMatrixQuestions>(entity =>
            {
                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyMatrixQuestions)
                    .HasForeignKey(d => d.SurveyQuestionId)
                    .HasConstraintName("FK_SurveyMatrixQuestions_SurveyQuestions");
            });

            modelBuilder.Entity<SurveyOutletMappings>(entity =>
            {
                entity.HasKey(e => new { e.SurveyId, e.OutletId });

                entity.Property(e => e.OutletId).IsUnicode(false);

                entity.HasOne(d => d.Outlet)
                    .WithMany(p => p.SurveyOutletMappings)
                    .HasForeignKey(d => d.OutletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyOutletMappings_Outlets");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyOutletMappings)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyOutletMappings_Surveys");
            });

            modelBuilder.Entity<SurveyPositionMappings>(entity =>
            {
                entity.HasKey(e => new { e.SurveyId, e.PositionId });

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.SurveyPositionMappings)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyPositionMappings_Positions");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyPositionMappings)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyPositionMappings_Surveys");
            });

            modelBuilder.Entity<SurveyQuestionTypes>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<SurveyQuestions>(entity =>
            {
                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.SurveyQuestions)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_SurveyQuestions_Blobs");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyQuestions)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyQuestions_Surveys");

                entity.HasOne(d => d.SurveyQuestionType)
                    .WithMany(p => p.SurveyQuestions)
                    .HasForeignKey(d => d.SurveyQuestionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyQuestions_SurveyQuestionTypes");
            });

            modelBuilder.Entity<SurveySpecialAnswers>(entity =>
            {
                entity.Property(e => e.Answer).IsUnicode(false);

                entity.HasOne(d => d.SurveyAnswerDetail)
                    .WithMany(p => p.SurveySpecialAnswers)
                    .HasForeignKey(d => d.SurveyAnswerDetailId)
                    .HasConstraintName("FK_SurveySpecialAnswers_SurveyAnswerDetails");
            });

            modelBuilder.Entity<Surveys>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.Title).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");
            });

            modelBuilder.Entity<TamemployeeStructure>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.EmployeeGroup).IsUnicode(false);

                entity.Property(e => e.EmployeeGroupText).IsUnicode(false);

                entity.Property(e => e.EmployeeSubgroup).IsUnicode(false);

                entity.Property(e => e.EmployeeSubgroupText).IsUnicode(false);

                entity.Property(e => e.JobCode).IsUnicode(false);

                entity.Property(e => e.JobName).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.NoReg).IsUnicode(false);

                entity.Property(e => e.OrgCode).IsUnicode(false);

                entity.Property(e => e.OrgName).IsUnicode(false);

                entity.Property(e => e.ParentOrgCode).IsUnicode(false);

                entity.Property(e => e.PersonalArea).IsUnicode(false);

                entity.Property(e => e.PersonalSubarea).IsUnicode(false);

                entity.Property(e => e.PostCode).IsUnicode(false);

                entity.Property(e => e.PostName).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);

                entity.Property(e => e.Structure).IsUnicode(false);

                entity.Property(e => e.WorkContract).IsUnicode(false);

                entity.Property(e => e.WorkContractText).IsUnicode(false);
            });

            modelBuilder.Entity<TaskAnswerDetails>(entity =>
            {
                entity.Property(e => e.Answer).IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AnswerBlob)
                    .WithMany(p => p.TaskAnswerDetails)
                    .HasForeignKey(d => d.AnswerBlobId)
                    .HasConstraintName("FK_TaskAnswerDetails_Blobs");

                entity.HasOne(d => d.TaskAnswer)
                    .WithMany(p => p.TaskAnswerDetails)
                    .HasForeignKey(d => d.TaskAnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaskAnswerDetails_TaskAnswers");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskAnswerDetails)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaskAnswerDetails_Tasks");
            });

            modelBuilder.Entity<TaskAnswers>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TaskAnswers)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaskAnswers_Employees");

                entity.HasOne(d => d.PopQuiz)
                    .WithMany(p => p.TaskAnswers)
                    .HasForeignKey(d => d.PopQuizId)
                    .HasConstraintName("FK_TaskAnswers_PopQuizzes");

                entity.HasOne(d => d.SetupModule)
                    .WithMany(p => p.TaskAnswers)
                    .HasForeignKey(d => d.SetupModuleId)
                    .HasConstraintName("FK_TaskAnswers_SetupModules");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.TaskAnswers)
                    .HasForeignKey(d => d.TrainingId)
                    .HasConstraintName("FK_TaskAnswers_Trainings");
            });

            modelBuilder.Entity<TaskChecklistChoices>(entity =>
            {
                entity.Property(e => e.Text).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskChecklistChoices)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_TaskChecklistChoices_Tasks");
            });

            modelBuilder.Entity<TaskChecklistTypes>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();

                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithOne(p => p.TaskChecklistTypes)
                    .HasForeignKey<TaskChecklistTypes>(d => d.TaskId)
                    .HasConstraintName("FK_TaskChecklistTypes_Tasks");
            });

            modelBuilder.Entity<TaskEssayTypes>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();

                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithOne(p => p.TaskEssayTypes)
                    .HasForeignKey<TaskEssayTypes>(d => d.TaskId)
                    .HasConstraintName("FK_TaskEssayTypes_Tasks");
            });

            modelBuilder.Entity<TaskFileUploadTypes>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();

                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithOne(p => p.TaskFileUploadTypes)
                    .HasForeignKey<TaskFileUploadTypes>(d => d.TaskId)
                    .HasConstraintName("FK_TaskFileUploadTypes_Tasks");
            });

            modelBuilder.Entity<TaskHotSpotAnswers>(entity =>
            {
                entity.Property(e => e.Answer).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskHotSpotAnswers)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_TaskHotSpotAnswers_Tasks");
            });

            modelBuilder.Entity<TaskHotSpotTypes>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();

                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.TaskHotSpotTypes)
                    .HasForeignKey(d => d.BlobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaskHotSpotTypes_Blobs");

                entity.HasOne(d => d.Task)
                    .WithOne(p => p.TaskHotSpotTypes)
                    .HasForeignKey<TaskHotSpotTypes>(d => d.TaskId)
                    .HasConstraintName("FK_TaskHotSpotTypes_Tasks");
            });

            modelBuilder.Entity<TaskMatchingChoices>(entity =>
            {
                entity.Property(e => e.TaskMatchingChoiceCode).IsUnicode(false);

                entity.Property(e => e.Text).IsUnicode(false);

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.TaskMatchingChoices)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_TaskMatchingChoices_Blobs");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskMatchingChoices)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_TaskMatchingChoices_Tasks");
            });

            modelBuilder.Entity<UpgradeAccountApprovals>(entity =>
            {
                entity.HasKey(e => e.Guid);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.ApprovalDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.UpgradeAccountApprovals)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Upgrade_Approvals_Employees");
            });

            modelBuilder.Entity<UserFcmTokens>(entity =>
            {
                entity.HasKey(e => e.Guid);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.UserFcmTokens)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_User_Fcm_Employees");
            });

            modelBuilder.Entity<HomePage_AllPopularTraining>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProgramTypeName).IsUnicode(false);
                entity.Property(e => e.CourseName).IsUnicode(false);
                entity.Property(e => e.ModuleName).IsUnicode(false);
                entity.Property(e => e.Mime).IsUnicode(false);
                entity.Property(e => e.MaterialTypeName).IsUnicode(false);
                entity.Property(e => e.OutletId).IsUnicode(false);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.ApprovedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<HomePage_AllLatestTraining>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProgramTypeName).IsUnicode(false);
                entity.Property(e => e.CourseName).IsUnicode(false);
                entity.Property(e => e.ModuleName).IsUnicode(false);
                entity.Property(e => e.Mime).IsUnicode(false);
                entity.Property(e => e.MaterialTypeName).IsUnicode(false);
                entity.Property(e => e.OutletId).IsUnicode(false);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.ApprovedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<HomePage_AllRecommendedTraining>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProgramTypeName).IsUnicode(false);
                entity.Property(e => e.CourseName).IsUnicode(false);
                entity.Property(e => e.ModuleName).IsUnicode(false);
                entity.Property(e => e.MIME).IsUnicode(false);
                entity.Property(e => e.MaterialTypeName).IsUnicode(false);
                entity.Property(e => e.OutletId).IsUnicode(false);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.ApprovedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<EmployeeDownload>(entity =>
            {
                entity.HasKey(e => e.Guid);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeDownloads)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_User_Downloads");
            });

            modelBuilder.Entity<TaskMatchingTypes>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();

                entity.Property(e => e.Answer).IsUnicode(false);

                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithOne(p => p.TaskMatchingTypes)
                    .HasForeignKey<TaskMatchingTypes>(d => d.TaskId)
                    .HasConstraintName("FK_TaskMatchingTypes_Tasks");
            });

            modelBuilder.Entity<TaskMatrixChoices>(entity =>
            {
                entity.Property(e => e.Text).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskMatrixChoices)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_TaskMatrixChoices_Tasks");
            });

            modelBuilder.Entity<TaskMatrixQuestions>(entity =>
            {
                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskMatrixQuestions)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_TaskMatrixQuestions_Tasks");
            });

            modelBuilder.Entity<TaskMatrixTypes>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();

                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithOne(p => p.TaskMatrixTypes)
                    .HasForeignKey<TaskMatrixTypes>(d => d.TaskId)
                    .HasConstraintName("FK_TaskMatrixTypes_Tasks");
            });

            modelBuilder.Entity<TaskMultipleChoiceChoices>(entity =>
            {
                entity.Property(e => e.Text).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskMultipleChoiceChoices)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_TaskMultipleChoiceChoices_Tasks");
            });

            modelBuilder.Entity<TaskMultipleChoiceTypes>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();

                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithOne(p => p.TaskMultipleChoiceTypes)
                    .HasForeignKey<TaskMultipleChoiceTypes>(d => d.TaskId)
                    .HasConstraintName("FK_TaskMultipleChoiceTypes_Tasks");
            });

            modelBuilder.Entity<TaskRatingChoices>(entity =>
            {
                entity.Property(e => e.Text).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskRatingChoices)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_TaskRatingChoices_Tasks");
            });

            modelBuilder.Entity<TaskRatingTypes>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();

                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithOne(p => p.TaskRatingTypes)
                    .HasForeignKey<TaskRatingTypes>(d => d.TaskId)
                    .HasConstraintName("FK_TaskRatingTypes_Tasks");
            });

            modelBuilder.Entity<TaskSequenceChoices>(entity =>
            {
                entity.Property(e => e.Text).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskSequenceChoices)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_TaskSequenceChoices_Tasks");
            });

            modelBuilder.Entity<TaskSequenceTypes>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();

                entity.Property(e => e.Answer).IsUnicode(false);

                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithOne(p => p.TaskSequenceTypes)
                    .HasForeignKey<TaskSequenceTypes>(d => d.TaskId)
                    .HasConstraintName("FK_TaskSequenceTypes_Tasks");
            });

            modelBuilder.Entity<TaskShortAnswerTypes>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();

                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithOne(p => p.TaskShortAnswerTypes)
                    .HasForeignKey<TaskShortAnswerTypes>(d => d.TaskId)
                    .HasConstraintName("FK_TaskShortAnswerTypes_Tasks");
            });

            modelBuilder.Entity<TaskSpecialAnswers>(entity =>
            {
                entity.Property(e => e.Answer).IsUnicode(false);

                entity.HasOne(d => d.TaskAnswerDetail)
                    .WithMany(p => p.TaskSpecialAnswers)
                    .HasForeignKey(d => d.TaskAnswerDetailId)
                    .HasConstraintName("FK_TaskSpecialAnswers_TaskAnswerDetails");
            });

            modelBuilder.Entity<TaskTebakGambarPictures>(entity =>
            {
                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.TaskTebakGambarPictures)
                    .HasForeignKey(d => d.BlobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaskTebakGambarPictures_Blobs");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskTebakGambarPictures)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_TaskTebakGambarPictures_Tasks");
            });

            modelBuilder.Entity<TaskTebakGambarTypes>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();

                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithOne(p => p.TaskTebakGambarTypes)
                    .HasForeignKey<TaskTebakGambarTypes>(d => d.TaskId)
                    .HasConstraintName("FK_TaskTebakGambarTypes_Tasks");
            });

            modelBuilder.Entity<TaskTrueFalseTypes>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();

                entity.Property(e => e.Question).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithOne(p => p.TaskTrueFalseTypes)
                    .HasForeignKey<TaskTrueFalseTypes>(d => d.TaskId)
                    .HasConstraintName("FK_TaskTrueFalseTypes_Tasks");
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.StoryLineDescription).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.BlobId)
                    .HasConstraintName("FK_Tasks_Blobs");

                entity.HasOne(d => d.Competency)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.CompetencyId)
                    .HasConstraintName("FK_Tasks_Competencies");

                entity.HasOne(d => d.EvaluationType)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.EvaluationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tasks_EvaluationTypes");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("FK_Tasks_Modules");

                entity.HasOne(d => d.QuestionType)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.QuestionTypeId)
                    .HasConstraintName("FK_Tasks_QuestionTypes");
            });

            modelBuilder.Entity<TeamDetails>(entity =>
            {
                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TeamDetails)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamDetails_Employees");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamDetails)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamDetails_Teams");
            });

            modelBuilder.Entity<TeamMemberRequests>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ApprovalStatus)
                    .WithMany(p => p.TeamMemberRequests)
                    .HasForeignKey(d => d.ApprovalStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamMemberRequests_ApprovalStatus");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TeamMemberRequests)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamMemberRequests_Employees");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamMemberRequests)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamMemberRequests_Teams");
            });

            modelBuilder.Entity<Teams>(entity =>
            {
                entity.Property(e => e.TeamLeaderId).IsUnicode(false);

                entity.HasOne(d => d.TeamLeader)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.TeamLeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Teams_Employees");
            });

            modelBuilder.Entity<TimePoints>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.PointType)
                    .WithMany(p => p.TimePoints)
                    .HasForeignKey(d => d.PointTypeId)
                    .HasConstraintName("FK_TimePoints_PointTypes");
            });

            modelBuilder.Entity<Topics>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.TopicDescription).IsUnicode(false);

                entity.Property(e => e.TopicName).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.Topics)
                    .HasForeignKey(d => d.BlobId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Topics_Blobs");

                entity.HasOne(d => d.Ebadge)
                    .WithMany(p => p.Topics)
                    .HasForeignKey(d => d.EbadgeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Topics_EBadges");
            });

            modelBuilder.Entity<TrainingInvitations>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.ApprovalStatus)
                    .WithMany(p => p.TrainingInvitations)
                    .HasForeignKey(d => d.ApprovalStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainingInvitations_ApprovalStatus");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TrainingInvitations)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainingInvitations_Employees");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.TrainingInvitations)
                    .HasForeignKey(d => d.TrainingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainingInvitations_Trainings");
            });

            modelBuilder.Entity<TrainingModuleMappings>(entity =>
            {
                entity.HasOne(d => d.Coach)
                    .WithMany(p => p.TrainingModuleMappings)
                    .HasForeignKey(d => d.CoachId)
                    .HasConstraintName("FK_TrainingModuleMappings_Coaches");

                entity.HasOne(d => d.SetupModule)
                    .WithMany(p => p.TrainingModuleMappings)
                    .HasForeignKey(d => d.SetupModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainingModuleMappings_SetupModules");

                entity.HasOne(d => d.TimePoint)
                    .WithMany(p => p.TrainingModuleMappings)
                    .HasForeignKey(d => d.TimePointId)
                    .HasConstraintName("FK_TrainingModuleMappings_TimePoints");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.TrainingModuleMappings)
                    .HasForeignKey(d => d.TrainingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainingModuleMappings_Trainings");
            });

            modelBuilder.Entity<TrainingOutletMappings>(entity =>
            {
                entity.HasKey(e => new { e.TrainingId, e.OutletId });

                entity.Property(e => e.OutletId).IsUnicode(false);

                entity.HasOne(d => d.Outlet)
                    .WithMany(p => p.TrainingOutletMappings)
                    .HasForeignKey(d => d.OutletId)
                    .HasConstraintName("FK_TrainingOutletMappings_Outlets");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.TrainingOutletMappings)
                    .HasForeignKey(d => d.TrainingId)
                    .HasConstraintName("FK_TrainingOutletMappings_Trainings");
            });

            modelBuilder.Entity<TrainingPositionMappings>(entity =>
            {
                entity.HasKey(e => new { e.TrainingId, e.PositionId });

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.TrainingPositionMappings)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK_TrainingPositionMappings_Positions");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.TrainingPositionMappings)
                    .HasForeignKey(d => d.TrainingId)
                    .HasConstraintName("FK_TrainingPositionMappings_Trainings");
            });

            modelBuilder.Entity<TrainingProcesses>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Accomodation)
                    .WithMany(p => p.TrainingProcesses)
                    .HasForeignKey(d => d.AccomodationId)
                    .HasConstraintName("FK_TrainingProcesses_Accommodation");

                entity.HasOne(d => d.TrainingInvitation)
                    .WithMany(p => p.TrainingProcesses)
                    .HasForeignKey(d => d.TrainingInvitationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainingProcesses_TrainingInvitation");
            });

            modelBuilder.Entity<TrainingRatings>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.EmployeeId).IsUnicode(false);

                entity.Property(e => e.Review).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.TrainingRatings)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_TrainingRating_Courses");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TrainingRatings)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainingRatings_Employees");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.TrainingRatings)
                    .HasForeignKey(d => d.TrainingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainingRating_Trainings");
            });

            modelBuilder.Entity<TrainingServiceLevels>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");
            });

            modelBuilder.Entity<TrainingTypes>(entity =>
            {
                entity.Property(e => e.TrainingTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<Trainings>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Location).IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('SYSTEM')");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Trainings)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Trainings__Cours__5A846E65");
            });

            modelBuilder.Entity<Assesments>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestions>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionTypes>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentSkillChecks>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<SkillChecksQuestions>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<SkillChecks>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<SkillChecksScoreConfigs>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<LiveAssesmentSkillChecks>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<LiveAssesmentSkillCheckQuestions>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TrainingPositionOnlyViewMappings>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerAnswerImages>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerAnswerMathcings>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerAnswerMultipleChoices>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerChecklists>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerDropdowns>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerEssays>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerHotspots>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerFileUploads>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerShortAnswers>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerTrueFalses>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerAnswerRatings>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerAnswerRatingRates>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerAnswerRatingOptions>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerMatrixes>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerMatrixesX>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerMatrixesY>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AssesmentQuestionAnswerMatrixesXY>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });


            modelBuilder.Entity<GetAllMyCoachReport>(entity =>
            {
                entity.Property(e => e.assesmentid).IsUnicode(false);

                entity.Property(e => e.modulename).IsUnicode(false);

                entity.Property(e => e.enumscoringmethod).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.guid).IsUnicode(false);

                entity.Property(e => e.employeeid).IsUnicode(false);

                entity.Property(e => e.returnValue).IsUnicode(false);

                entity.Property(e => e.createdAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.endtime).HasDefaultValueSql("(getdate())");
            });




        }
    }
}
