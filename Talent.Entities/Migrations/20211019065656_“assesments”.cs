using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class assesments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentCode",
                table: "StagingDealerEmployee",
                maxLength: 250,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionTypes",
                columns: table => new
                {
                    GUIDQuestionType = table.Column<string>(nullable: false),
                    TypeName = table.Column<string>( nullable: true),
                    CreatedAt = table.Column<DateTime>(maxLength: 128, nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionTypes", x => x.GUIDQuestionType);
                });

            migrationBuilder.CreateTable(
                name: "Assesments",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TrainingTypeID = table.Column<string>(nullable: true),
                    EnumScoringMethod = table.Column<string>(nullable: true),
                    EnumRemedialOption = table.Column<string>(nullable: true),
                    LearningTime = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assesments", x => x.GUID);
                });

            migrationBuilder.CreateTable(
                name: "SkillChecks",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IsQuestion = table.Column<bool>(nullable: false),
                    MinimumScore = table.Column<float>(nullable: false),
                    EnumFeedbackScore = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MediaBlobId = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillChecks", x => x.GUID);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPositionOnlyViewMappings",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    TrainingId = table.Column<int>(nullable: false),
                    PositionId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPositionOnlyViewMappings", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_TrainingPositionOnlyViewMappings_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingPositionOnlyViewMappings_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Trainings",
                        principalColumn: "TrainingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestions",
                columns: table => new
                {
                    GUID = table.Column<string>( nullable: false),
                    GUIDQuestionType = table.Column<string>( nullable: true),
                    QuestionCode = table.Column<string>(nullable: true),
                    Question = table.Column<string>(nullable: true),
                    BlobId = table.Column<string>(nullable: true),
                    EnumStoryLine = table.Column<string>(nullable: true),
                    TotalScore = table.Column<float>(nullable: false),
                    TotalPoint = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestions", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestions_AssesmentQuestionTypes_GUIDQuestionType",
                        column: x => x.GUIDQuestionType,
                        principalTable: "AssesmentQuestionTypes",
                        principalColumn: "GUIDQuestionType",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentSkillChecks",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    SkillCheckGUID = table.Column<string>(nullable: true),
                    AssesmentGUID = table.Column<string>(nullable: true),
                    Order = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentSkillChecks", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentSkillChecks_Assesments_AssesmentGUID",
                        column: x => x.AssesmentGUID,
                        principalTable: "Assesments",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssesmentSkillChecks_SkillChecks_SkillCheckGUID",
                        column: x => x.SkillCheckGUID,
                        principalTable: "SkillChecks",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LiveAssesmentSkillChecks",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    EmployeeGUID = table.Column<string>(nullable: true),
                    ScorerGUID = table.Column<string>(nullable: true),
                    ScorerType = table.Column<string>(nullable: true),
                    SkillCheckGUID = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Point = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    IsDraft = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveAssesmentSkillChecks", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_LiveAssesmentSkillChecks_Employees_EmployeeGUID",
                        column: x => x.EmployeeGUID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LiveAssesmentSkillChecks_Employees_ScorerGUID",
                        column: x => x.ScorerGUID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LiveAssesmentSkillChecks_SkillChecks_SkillCheckGUID",
                        column: x => x.SkillCheckGUID,
                        principalTable: "SkillChecks",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkillChecksScoreConfigs",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    SkillCheckGUID = table.Column<string>(nullable: true),
                    Point = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillChecksScoreConfigs", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_SkillChecksScoreConfigs_SkillChecks_SkillCheckGUID",
                        column: x => x.SkillCheckGUID,
                        principalTable: "SkillChecks",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerAnswerImages",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    KeyAnswer = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    BlobId = table.Column<string>(nullable: true),
                    Point = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerAnswerImages", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerAnswerImages_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerAnswerMathcings",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    KeyAnswer = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Point = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerAnswerMathcings", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerAnswerMathcings_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerAnswerMultipleChoices",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    KeyAnswer = table.Column<string>(nullable: true),
                    Order = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Point = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerAnswerMultipleChoices", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerAnswerMultipleChoices_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerAnswerRatings",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerAnswerRatings", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerAnswerRatings_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerChecklists",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    KeyAnswer = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Point = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerChecklists", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerChecklists_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerDropdowns",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    KeyAnswer = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Point = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerDropdowns", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerDropdowns_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerEssays",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Point = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerEssays", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerEssays_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerFileUploads",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    KeyAnswer = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Point = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerFileUploads", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerFileUploads_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerHotspots",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    KeyAnswer = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Point = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerHotspots", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerHotspots_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerMatrixes",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerMatrixes", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerMatrixes_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerShortAnswers",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Point = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerShortAnswers", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerShortAnswers_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerTrueFalses",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    KeyAnswer = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Point = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerTrueFalses", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerTrueFalses_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkillChecksQuestions",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    SkillCheckGUID = table.Column<string>(nullable: true),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillChecksQuestions", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_SkillChecksQuestions_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SkillChecksQuestions_SkillChecks_SkillCheckGUID",
                        column: x => x.SkillCheckGUID,
                        principalTable: "SkillChecks",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LiveAssesmentSkillCheckQuestions",
                columns: table => new
                {
                    GUID = table.Column<string>(nullable: false),
                    LiveAssesmentGUID = table.Column<string>(nullable: true),
                    AssesmentQuestionGUID = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Point = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveAssesmentSkillCheckQuestions", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_LiveAssesmentSkillCheckQuestions_AssesmentQuestions_AssesmentQuestionGUID",
                        column: x => x.AssesmentQuestionGUID,
                        principalTable: "AssesmentQuestions",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LiveAssesmentSkillCheckQuestions_LiveAssesmentSkillChecks_LiveAssesmentGUID",
                        column: x => x.LiveAssesmentGUID,
                        principalTable: "LiveAssesmentSkillChecks",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerAnswerRatingOptions",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentAnswerRatingsGUID = table.Column<string>(nullable: true),
                    Option = table.Column<string>(nullable: true),
                    Score = table.Column<float>(nullable: false),
                    Point = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerAnswerRatingOptions", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerAnswerRatingOptions_AssesmentQuestionAnswerAnswerRatings_AssesmentAnswerRatingsGUID",
                        column: x => x.AssesmentAnswerRatingsGUID,
                        principalTable: "AssesmentQuestionAnswerAnswerRatings",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerAnswerRatingRates",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentAnswerRatingsGUID = table.Column<string>(nullable: true),
                    Range = table.Column<string>(nullable: true),
                    Score = table.Column<float>(nullable: false),
                    Point = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerAnswerRatingRates", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerAnswerRatingRates_AssesmentQuestionAnswerAnswerRatings_AssesmentAnswerRatingsGUID",
                        column: x => x.AssesmentAnswerRatingsGUID,
                        principalTable: "AssesmentQuestionAnswerAnswerRatings",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerMatrixesX",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionAnswerMatrixGUID = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerMatrixesX", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerMatrixesX_AssesmentQuestionAnswerMatrixes_AssesmentQuestionAnswerMatrixGUID",
                        column: x => x.AssesmentQuestionAnswerMatrixGUID,
                        principalTable: "AssesmentQuestionAnswerMatrixes",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerMatrixesY",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionAnswerMatrixGUID = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerMatrixesY", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerMatrixesY_AssesmentQuestionAnswerMatrixes_AssesmentQuestionAnswerMatrixGUID",
                        column: x => x.AssesmentQuestionAnswerMatrixGUID,
                        principalTable: "AssesmentQuestionAnswerMatrixes",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentQuestionAnswerMatrixesXY",
                columns: table => new
                {
                     GUID = table.Column<string>( nullable: false),
                    AssesmentQuestionAnswerMatrixXGUID = table.Column<string>(nullable: true),
                    AssesmentQuestionAnswerMatrixYGUID = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Score = table.Column<float>(nullable: false),
                    Point = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentQuestionAnswerMatrixesXY", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerMatrixesXY_AssesmentQuestionAnswerMatrixesX_AssesmentQuestionAnswerMatrixXGUID",
                        column: x => x.AssesmentQuestionAnswerMatrixXGUID,
                        principalTable: "AssesmentQuestionAnswerMatrixesX",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssesmentQuestionAnswerMatrixesXY_AssesmentQuestionAnswerMatrixesY_AssesmentQuestionAnswerMatrixYGUID",
                        column: x => x.AssesmentQuestionAnswerMatrixYGUID,
                        principalTable: "AssesmentQuestionAnswerMatrixesY",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerAnswerImages_AssesmentQuestionGUID",
                table: "AssesmentQuestionAnswerAnswerImages",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerAnswerMathcings_AssesmentQuestionGUID",
                table: "AssesmentQuestionAnswerAnswerMathcings",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerAnswerMultipleChoices_AssesmentQuestionGUID",
                table: "AssesmentQuestionAnswerAnswerMultipleChoices",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerAnswerRatingOptions_AssesmentAnswerRatingsGUID",
                table: "AssesmentQuestionAnswerAnswerRatingOptions",
                column: "AssesmentAnswerRatingsGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerAnswerRatingRates_AssesmentAnswerRatingsGUID",
                table: "AssesmentQuestionAnswerAnswerRatingRates",
                column: "AssesmentAnswerRatingsGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerAnswerRatings_AssesmentQuestionGUID",
                table: "AssesmentQuestionAnswerAnswerRatings",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerChecklists_AssesmentQuestionGUID",
                table: "AssesmentQuestionAnswerChecklists",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerDropdowns_AssesmentQuestionGUID",
                table: "AssesmentQuestionAnswerDropdowns",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerEssays_AssesmentQuestionGUID",
                table: "AssesmentQuestionAnswerEssays",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerFileUploads_AssesmentQuestionGUID",
                table: "AssesmentQuestionAnswerFileUploads",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerHotspots_AssesmentQuestionGUID",
                table: "AssesmentQuestionAnswerHotspots",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerMatrixes_AssesmentQuestionGUID",
                table: "AssesmentQuestionAnswerMatrixes",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerMatrixesX_AssesmentQuestionAnswerMatrixGUID",
                table: "AssesmentQuestionAnswerMatrixesX",
                column: "AssesmentQuestionAnswerMatrixGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerMatrixesXY_AssesmentQuestionAnswerMatrixXGUID",
                table: "AssesmentQuestionAnswerMatrixesXY",
                column: "AssesmentQuestionAnswerMatrixXGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerMatrixesXY_AssesmentQuestionAnswerMatrixYGUID",
                table: "AssesmentQuestionAnswerMatrixesXY",
                column: "AssesmentQuestionAnswerMatrixYGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerMatrixesY_AssesmentQuestionAnswerMatrixGUID",
                table: "AssesmentQuestionAnswerMatrixesY",
                column: "AssesmentQuestionAnswerMatrixGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerShortAnswers_AssesmentQuestionGUID",
                table: "AssesmentQuestionAnswerShortAnswers",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestionAnswerTrueFalses_AssesmentQuestionGUID",
                table: "AssesmentQuestionAnswerTrueFalses",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentQuestions_GUIDQuestionType",
                table: "AssesmentQuestions",
                column: "GUIDQuestionType");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentSkillChecks_AssesmentGUID",
                table: "AssesmentSkillChecks",
                column: "AssesmentGUID");

            migrationBuilder.CreateIndex(
                name: "IX_AssesmentSkillChecks_SkillCheckGUID",
                table: "AssesmentSkillChecks",
                column: "SkillCheckGUID");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssesmentSkillCheckQuestions_AssesmentQuestionGUID",
                table: "LiveAssesmentSkillCheckQuestions",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssesmentSkillCheckQuestions_LiveAssesmentGUID",
                table: "LiveAssesmentSkillCheckQuestions",
                column: "LiveAssesmentGUID");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssesmentSkillChecks_EmployeeGUID",
                table: "LiveAssesmentSkillChecks",
                column: "EmployeeGUID");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssesmentSkillChecks_ScorerGUID",
                table: "LiveAssesmentSkillChecks",
                column: "ScorerGUID");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssesmentSkillChecks_SkillCheckGUID",
                table: "LiveAssesmentSkillChecks",
                column: "SkillCheckGUID");

            migrationBuilder.CreateIndex(
                name: "IX_SkillChecksQuestions_AssesmentQuestionGUID",
                table: "SkillChecksQuestions",
                column: "AssesmentQuestionGUID");

            migrationBuilder.CreateIndex(
                name: "IX_SkillChecksQuestions_SkillCheckGUID",
                table: "SkillChecksQuestions",
                column: "SkillCheckGUID");

            migrationBuilder.CreateIndex(
                name: "IX_SkillChecksScoreConfigs_SkillCheckGUID",
                table: "SkillChecksScoreConfigs",
                column: "SkillCheckGUID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPositionOnlyViewMappings_PositionId",
                table: "TrainingPositionOnlyViewMappings",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPositionOnlyViewMappings_TrainingId",
                table: "TrainingPositionOnlyViewMappings",
                column: "TrainingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerAnswerImages");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerAnswerMathcings");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerAnswerMultipleChoices");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerAnswerRatingOptions");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerAnswerRatingRates");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerChecklists");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerDropdowns");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerEssays");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerFileUploads");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerHotspots");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerMatrixesXY");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerShortAnswers");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerTrueFalses");

            migrationBuilder.DropTable(
                name: "AssesmentSkillChecks");

            migrationBuilder.DropTable(
                name: "LiveAssesmentSkillCheckQuestions");

            migrationBuilder.DropTable(
                name: "SkillChecksQuestions");

            migrationBuilder.DropTable(
                name: "SkillChecksScoreConfigs");

            migrationBuilder.DropTable(
                name: "TrainingPositionOnlyViewMappings");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerAnswerRatings");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerMatrixesX");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerMatrixesY");

            migrationBuilder.DropTable(
                name: "Assesments");

            migrationBuilder.DropTable(
                name: "LiveAssesmentSkillChecks");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionAnswerMatrixes");

            migrationBuilder.DropTable(
                name: "SkillChecks");

            migrationBuilder.DropTable(
                name: "AssesmentQuestions");

            migrationBuilder.DropTable(
                name: "AssesmentQuestionTypes");

            migrationBuilder.DropColumn(
                name: "ParentCode",
                table: "StagingDealerEmployee");
        }
    }
}
