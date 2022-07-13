using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Talent.Entities.Entities;
using Minio;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.Enums;

using Talent.WebAdmin.UserSide.Services;
using Talent.WebAdmin.UserSide.Models.Configurations;
using Talent.WebAdmin.UserSide.Interfaces;
using Talent.WebAdmin.Helpers;
using StackExchange.Redis;
using TAM.Talent.Commons.Core.Interfaces;
using TAM.Talent.Certificate.Lib;
using TAM.Talent.Excels.Lib.Services;
using Amazon.S3;
using Talent.WebAdmin.Services.TrackingProgressReport;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;

namespace Talent.WebAdmin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureSystemServices(services);
            ConfigureTalentServices(services);
            ConfigureAuthenticationServices(services);

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
        }


        private static void ConfigureAuthenticationServices(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var RedisServiceMan = provider.GetRequiredService<RedisStoreSrevice>();
            services.AddAuthentication(ITalentAuthenticationSchemes.Cookie).AddCookie(ITalentAuthenticationSchemes.Cookie, options =>
            {
                options.LoginPath = "/auth/login";
                options.LogoutPath = "/index";
                options.AccessDeniedPath = "/index";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.SessionStore = RedisServiceMan;
            });
        }


        private void ConfigureSystemServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
          // This lambda determines whether user consent for non-essential cookies is needed for a given request.
          options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(Q =>
            {
                Q.IdleTimeout = TimeSpan.FromHours(12);
                Q.IOTimeout = TimeSpan.FromMinutes(2);
                Q.Cookie.IsEssential = true;
                Q.Cookie.HttpOnly = true;
                Q.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                Q.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContextPool<TalentContext>(options =>
            {
                // https://docs.microsoft.com/en-us/ef/core/querying/client-eval
                options.UseSqlServer
                (
                    Configuration.GetConnectionString("Talent"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(3600)
                )
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
                // For Debug Only
                //.EnableSensitiveDataLogging(true)
                //.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.SensitiveDataLoggingEnabledWarning));
            });

            //redis configs 
            //var redisConfigString = Configuration.GetConnectionString("Redis");
            var redisConfigString = this.Configuration["RedisSettings:IP"];
            //var redisOptions = ConfigurationOptions.Parse(redisConfigString);

            var connect = ConnectionMultiplexer.Connect(redisConfigString);

            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = redisConfigString;
                option.InstanceName = this.Configuration["RedisSettings:InstanceName"];
            });

            services.Configure<AppSettings>(Configuration);
            services.AddSingleton(di => di.GetService<IOptions<AppSettings>>().Value);

            // Configuration SMTP For Send Email.
            services.Configure<SmtpConfiguration>(this.Configuration.GetSection("Smtp"));
            // Configuration SSO For Token.
            services.Configure<SsoConfiguration>(this.Configuration.GetSection("SSOSettings"));

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(config => config.FullName);
                c.SwaggerDoc("v1", new Info { Title = "Talent Back End", Version = "v1" });
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();
        }

        private void ConfigureTalentServices(IServiceCollection services)
        {

            services.AddTransient<IFileStorageService, FileService>();
            services.AddTransient<FileService>();

            // TODO: Ini serius pada scoped? Klo bisa transient, ya transient aja.
            services.AddScoped<RedisStoreSrevice>();

            services.AddScoped<IContextualService, WebContextualService>();
            services.AddScoped<SetupTimePointService>();
            services.AddScoped<BlobService>();
            services.AddScoped<CompetencyMappingService>();
            services.AddScoped<CompetencyService>();
            services.AddScoped<CompetencyJoinService>();
            services.AddScoped<CompetencyTypeService>();
            services.AddScoped<GuideService>();
            services.AddScoped<GuideJoinService>();
            services.AddScoped<GuideTypeService>();
            services.AddScoped<KeyActionService>();
            services.AddScoped<PointTypeService>();
            services.AddScoped<EvaluationTypesService>();
            services.AddScoped<TopicService>();
            services.AddScoped<ModuleService>();
            services.AddScoped<SequenceService>();
            services.AddScoped<ChecklistService>();
            services.AddScoped<SetupPopQuizService>();
            services.AddScoped<SetupCourseService>();
            services.AddScoped<TaskService>();
            services.AddScoped<TaskRatingService>();
            services.AddScoped<TaskTebakGambarService>();
            services.AddTransient<TaskHotSpotService>();
            services.AddTransient<TaskMultipleChoiceService>();
            services.AddScoped<CourseService>();
            services.AddScoped<CourseJoinService>();
            services.AddScoped<ProgramTypeService>();
            services.AddScoped<LevelService>();
            services.AddScoped<CourseCategoryService>();
            services.AddScoped<LearningTypeService>();
            services.AddScoped<ApprovalStatusService>();
            services.AddScoped<DigitalSignatureService>();
            services.AddScoped<TaskEssayTypeService>();
            services.AddScoped<TaskMatchingTypeService>();
            services.AddScoped<TaskMatrixTypeService>();
            services.AddScoped<TaskTrueFalseService>();
            services.AddScoped<CoachesService>();
            services.AddScoped<AuthService>();
            services.AddScoped<TaskShortAnswerService>();
            services.AddScoped<TaskUploadFileService>();
            services.AddScoped<SetupModuleService>();
            services.AddScoped<UserRoleService>();
            services.AddScoped<DropdownService>();
            services.AddScoped<HobbyService>();
            services.AddScoped<EmployeeService>();
            services.AddScoped<SetupLearningService>();
            services.AddScoped<ApprovalMappingService>();
            services.AddScoped<ApprovalContentService>();
            services.AddScoped<BannerService>();
            services.AddScoped<ReleaseTrainingService>();
            services.AddScoped<AssessmentService>();
            services.AddScoped<RewardService>();
            services.AddScoped<EventCategoryService>();
            services.AddScoped<EventService>();
            services.AddScoped<EventJoinService>();
            services.AddScoped<CalendarService>();
            services.AddScoped<SetupTop5CourseService>();
            services.AddScoped<SetupTSLService>();
            services.AddScoped<UserPrivilegeSettingsService>();
            services.AddScoped<PositionMappingService>();
            services.AddScoped<SurveysService>();
            services.AddScoped<InboxService>();
            services.AddScoped<StagingService>();
            services.AddScoped<StagingToTableService>();
            services.AddScoped<DashboardOperationService>();
            services.AddScoped<ApprovalTrainingService>();
            services.AddScoped<TrainingProcessService>();
            services.AddScoped<ReportService>();
            services.AddScoped<ReportNPSService>();
            services.AddScoped<SurveyReportService>();
            services.AddScoped<SurveyReportGenerateExcelService>();
            services.AddScoped<TrainingScoreReportService>();
            services.AddScoped<TrainingScoreReportGenerateExcelService>();
            services.AddScoped<ProductService>();
            services.AddScoped<ProductCategoryService>();
            services.AddScoped<ProductTypeService>();
            services.AddScoped<ProductGalleryService>();
            services.AddScoped<ProductCustomerTypeService>();
            services.AddScoped<ProductCustomerService>();
            services.AddScoped<ProductPhotoService>();
            services.AddScoped<ProductFAQCategoryService>();
            services.AddScoped<ProductFeatureCategoryService>();
            services.AddScoped<ProductFAQService>();
            services.AddScoped<ProductFAQUserService>();
            services.AddScoped<ProductCompetitorMappingService>();
            services.AddScoped<ProductFeatureService>();
            services.AddScoped<ProductFeatureMappingService>();
            services.AddScoped<ProductTipService>();
            services.AddScoped<ProductTipCategoryService>();
            services.AddScoped<ProductProgramCategoryService>();
            services.AddScoped<ProductProgramMappingService>();
            services.AddScoped<ProductSPWACategoryService>();
            services.AddScoped<ProductSPWAMappingService>();
            services.AddScoped<DictionaryService>();
            services.AddScoped<CmsContentService>();
            services.AddScoped<CmsFmbService>();
            services.AddScoped<CmsMenuService>();
            services.AddScoped<CmsOperationService>();
            services.AddScoped<CmsSettingService>();
            services.AddScoped<CmsSubContentService>();
            services.AddScoped<CmsWorkPrincipalService>();
            services.AddScoped<KeyPieceOfMindService>();
            services.AddScoped<KeyPieceOfMindTypeService>();
            services.AddScoped<ServiceTipService>();
            services.AddScoped<ServiceTipTypeService>();
            services.AddScoped<ServiceTipMenuService>();
            services.AddScoped<KodawariBannerService>();
            services.AddScoped<KodawariDownloadService>();
            services.AddScoped<KodawariMenuService>();
            services.AddScoped<KodawariTypeService>();
            services.AddScoped<KodawariService>();
            services.AddScoped<HOGuidelineUploadService>();
            services.AddScoped<UserActivityService>();
            services.AddScoped<SkillCheckService>();
            services.AddScoped<SkillCheckQuestionService>();
            services.AddScoped<SkillChecksScoreConfigs>();
            services.AddScoped<AssesmentService>();
            services.AddScoped<AssesmentSkillCheckService>();
            services.AddScoped<AssesmentQuestionTypeService>();
            services.AddScoped<AssesmentQuestionService>();
            services.AddScoped<ReportTrackingService>();
            services.AddScoped<UserActivityService>();
            services.AddScoped<TrackingReportProgressReportService>();
            services.AddScoped<AreaSpecialistService>();

            //API MOBILE
            services.AddScoped<UserSideTaskService>();
            services.AddScoped<UserSideMyLearningService>();
            services.AddScoped<UserSideTimePointService>();
            services.AddScoped<UserSideTeamMappingService>();
            services.AddScoped<UserSideProfileService>();
            services.AddScoped<UserSideProductService>();
            services.AddScoped<UserSideProductGalleryService>();
            services.AddScoped<UserSideProductCustomerService>();
            services.AddScoped<UserSideProductPhotoService>();
            services.AddScoped<UserSideProductFAQService>();
            services.AddScoped<UserSideCertificateService>();
            services.AddScoped<UserSideTeamService>();
            services.AddScoped<UserSideTeamCourseService>();
            services.AddScoped<UserSideInterestService>();
            services.AddScoped<UserSideHobbyService>();
            services.AddScoped<UserSideScheduleService>();
            services.AddScoped<UserSideBadgesService>();
            services.AddScoped<UserSidePersonalMappingService>();
            services.AddScoped<UserSideLearningHistoryService>();
            services.AddScoped<UserSideTrainingService>();
            services.AddScoped<UserSideRankService>();
            services.AddScoped<UserSideTimeTableService>();
            services.AddScoped<UserSideCoachService>();
            services.AddScoped<UserSideScheduleBookingService>();
            services.AddScoped<UserSideTaskAnswerService>();
            services.AddScoped<UserSideMyEventService>();
            services.AddScoped<UserSideReviewService>();
            services.AddScoped<ClaimHelper>();
            services.AddScoped<UserSideAccessService>();
            services.AddScoped<UserSideMyGuideService>();
            services.AddScoped<UserSidePopQuizService>();
            services.AddScoped<UserSideMyInsightService>();
            services.AddScoped<UserSideMyInsightAnswerService>();
            services.AddScoped<UserSideTrainingProcessService>();
            services.AddScoped<UserSideSearch>();
            services.AddScoped<UserSideSurveyService>();
            services.AddScoped<UserSideLevelService>();
            services.AddScoped<UserSideUserAccessTimesService>();
            services.AddScoped<UserSideGenerateCertificatePDFService>();
            services.AddScoped<PDFCertificateGeneratorService>();
            services.AddScoped<UserSideDictionaryService>();
            services.AddScoped<UserSideBulkCalculatingLearningService>();
            services.AddScoped<UserSideProductComparisonService>();
            services.AddScoped<UserSideProductFeatureService>();
            services.AddScoped<UserSideProductTipService>();
            services.AddScoped<UserSideProductProgramService>();
            services.AddScoped<UserSideProductSPWAService>();
            services.AddScoped<UserSideKeyPieceOfMindService>();
            services.AddScoped<UserSideServiceTipService>();
            services.AddScoped<UserSideKodawariService>();
            services.AddScoped<UserSideLiveAssesmentSkillCheckService>();
            services.AddScoped<UserSideLiveAssesmentSkillCheckQuestionService>();


            // For SSO Token.
            services.AddScoped<UserSideAuthService>();



            // For Send Email
            services.AddTransient<IEmailService, UserSideEmailService>();
            services.AddSingleton<ITemplatingService, UserSideHandlebarsService>();
            services.AddTransient<ApprovalService>();
            services.AddScoped<MasterNewsService>();
            services.AddScoped<UserSideLoginService>();
            services.AddScoped<UserSideFilterListService>();
            services.AddScoped<UserSideMyTeamService>();
            services.AddScoped<UserSideInboxService>();
            services.AddScoped<UserSideBannerService>();
            services.AddScoped<UserSideNewsService>();
            services.AddScoped<UserSideMyPointAndRewardService>();


            // New Features
            services.AddScoped<UserSideGuestLoginService>();
            services.AddScoped<UserSideDealerService>();
            services.AddScoped<UserSideOnBoardingService>();
            services.AddScoped<UserSideAnnouncementService>();
            services.AddScoped<ApprovalUpgradeUserService>();
            services.AddScoped<DashboardGuestAccountService>();
            services.AddScoped<UserFCMService>();
            services.AddScoped<PushNotificationService>();
            services.AddScoped<UserSideUserDownloadService>();
            services.AddScoped<KeyPieceOfMindMenuService>();

            // Inject Scoring Service
            services.AddScoped<UserSideScoringService>();

            services.AddSingleton<MinioClient>(di =>
      {
          var url = this.Configuration["MinIO:EndPoint"];
          var accessKey = this.Configuration["MinIO:AccessKey"];
          var secretKey = this.Configuration["MinIO:SecretKey"];
          return new MinioClient(url, accessKey, secretKey);
      });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                await next();
            });


            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Strict,
                Secure = CookieSecurePolicy.SameAsRequest
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Talent Back End");
                c.DisplayOperationId();
            });

            app.UseSession();
            app.UseAuthentication();
            app.UseMvc();


        }
    }
}
