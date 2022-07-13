CREATE DATABASE ITALENT
GO
USE ITALENT
GO

-- General
CREATE TABLE Blobs (
	BlobId uniqueidentifier PRIMARY KEY DEFAULT NEWID(),
	[Name] varchar(64) NOT NULL,
	MIME varchar(max) NOT NULL
)
GO

CREATE TABLE Dealers (
	DealerId varchar(64) PRIMARY KEY,
	DealerName varchar(128) NOT NULL,
	CreatedAt datetime2(7) NOT NULL,
	CreatedBy varchar(64) NOT NULL
)
GO

CREATE TABLE Datis (
	DatiId varchar(32) PRIMARY KEY,
	DatiName varchar(255) NOT NULL
)
GO

CREATE TABLE CFAMs (
	CFAMId varchar(32) PRIMARY KEY,
	CFAMName varchar(255) NOT NULL
)
GO

CREATE TABLE Provinces (
	ProvinceId varchar(32) PRIMARY KEY,
	ProvinceName varchar(255) NOT NULL
)
GO

CREATE TABLE Cities (
	CityId varchar(32) PRIMARY KEY,
	CityName varchar(255) NOT NULL
)
GO

CREATE TABLE Outlets (
	OutletId varchar(64) PRIMARY KEY,
	DealerId varchar(64) NOT NULL FOREIGN KEY REFERENCES Dealers(DealerId) ON UPDATE CASCADE ON DELETE CASCADE,
	DatilId varchar(32) NOT NULL FOREIGN KEY REFERENCES Datis(DatiId) ON UPDATE CASCADE ON DELETE CASCADE,
	CFAMId varchar(32) NOT NULL FOREIGN KEY REFERENCES CFAMs(CFAMId) ON UPDATE CASCADE ON DELETE CASCADE,
	ProvinceId varchar(32) NOT NULL FOREIGN KEY REFERENCES Provinces(ProvinceId) ON UPDATE CASCADE ON DELETE CASCADE,
	CityId varchar(32) NOT NULL FOREIGN KEY REFERENCES Cities(CityId) ON UPDATE CASCADE ON DELETE CASCADE,
	OutletCode varchar(64) NOT NULL,
	[Name] varchar(max) NOT NULL,
	Phone varchar(255) NULL,
	IsBP bit NOT NULL,
	IsGR bit NOT NULL,
	IsSales bit NOT NULL,
	CreatedAt datetime2(7) NOT NULL,
	CreatedBy varchar(64) NOT NULL
)
GO

CREATE TABLE Employees (
	EmployeeId varchar(64) PRIMARY KEY,
	OutletId varchar(64) FOREIGN KEY REFERENCES Outlets(OutletId) ON DELETE CASCADE ON UPDATE CASCADE,
	EmployeeUsername varchar(255) NOT NULL,
	EmployeeName varchar(255) NOT NULL,
	EmployeeExperience int NOT NULL DEFAULT 0,
	EmployeePoint int NOT NULL DEFAULT 0,
	EmployeeEmail varchar(255) NULL,
	EmployeePhone varchar(16) NULL,
	LastSeenAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	CreatedBy varchar(64) NOT NULL,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdatedBy varchar(64) NOT NULL,
	IsDeleted bit NOT NULL
)
GO

CREATE TABLE Positions (
    PositionId int PRIMARY KEY IDENTITY,
	PositionName varchar(64) NOT NULL,
	PositionDescription varchar(1024) NOT NULL,
)
GO

CREATE TABLE EmployeePositionMappings (
	EmployeeId varchar(64) NOT NULL FOREIGN KEY REFERENCES Employees(EmployeeId) ON DELETE CASCADE ON UPDATE CASCADE,
    PositionId int NOT NULL FOREIGN KEY REFERENCES Positions(PositionId) ON DELETE CASCADE ON UPDATE CASCADE,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY(EmployeeId, PositionId)
)
GO

CREATE TABLE Regions (
    RegionId int PRIMARY KEY IDENTITY,
	RegionName varchar(64) NOT NULL,
)
GO

CREATE TABLE Areas
(
	AreaId int PRIMARY KEY IDENTITY,
	AreaName VARCHAR(128) NOT NULL,
)
GO
​
-- SPRINT I
-- Key Action
CREATE TABLE KeyActions (
	KeyActionId int PRIMARY KEY IDENTITY,
	KeyActionCode varchar(16) NOT NULL UNIQUE,
	KeyActionName varchar(32) NOT NULL,
	KeyActionDescription varchar(256) NULL,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP
)
GO

-- Competency
CREATE TABLE CompetencyTypes (
	CompetencyTypeId int PRIMARY KEY IDENTITY,
	CompetencyTypeName varchar(32) NOT NULL
)
GO
INSERT INTO CompetencyTypes(CompetencyTypeName) VALUES
	('Hard'),
	('Soft')
GO

CREATE TABLE Competencies (
	CompetencyId int PRIMARY KEY IDENTITY,
	CompetencyTypeId int NOT NULL FOREIGN KEY REFERENCES CompetencyTypes(CompetencyTypeId) ON UPDATE CASCADE ON DELETE CASCADE,
	KeyActionId int NOT NULL FOREIGN KEY REFERENCES KeyActions(KeyActionId) ON UPDATE CASCADE ON DELETE CASCADE,
	PrefixCode varchar(16) NOT NULL,
	CompetencyName varchar(64) NOT NULL,
	CompetencyDescription varchar(256) NULL,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP
)
GO

-- Topic
CREATE TABLE EBadges (
	EBadgeId int PRIMARY KEY IDENTITY,
	EBadgeName varchar(64) NOT NULL
)
GO
INSERT INTO EBadges(EBadgeName) VALUES
	('Bronze'),
	('Silver'),
	('Gold')
GO

CREATE TABLE Topics (
	TopicId int PRIMARY KEY IDENTITY,
	EBadgeId int FOREIGN KEY REFERENCES EBadges(EBadgeId) ON UPDATE CASCADE ON DELETE CASCADE,
	BlobId uniqueidentifier FOREIGN KEY REFERENCES Blobs(BlobId) ON UPDATE CASCADE ON DELETE CASCADE,
	TopicName varchar(64) NOT NULL,
	TopicMinimumPoints int NOT NULL,
	TopicDescription varchar(1024) NULL,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP
)
GO

-- Competency Mapping
CREATE TABLE EmployeeCompetencyMappings (
	EmployeeId varchar(64) NOT NULL FOREIGN KEY REFERENCES Employees(EmployeeId) ON UPDATE CASCADE ON DELETE CASCADE,
	CompetencyId int NOT NULL FOREIGN KEY REFERENCES Competencies(CompetencyId) ON UPDATE CASCADE ON DELETE CASCADE,
	PRIMARY KEY(EmployeeId, CompetencyId)
)
GO

CREATE TABLE EvaluationTypes (
	EvaluationTypeId int PRIMARY KEY IDENTITY,
	EvaluationTypeName varchar(64) NOT NULL
)
GO

INSERT INTO EvaluationTypes(EvaluationTypeName) VALUES
	('Knowledge'),
	('Skill'),
	('Behavior')
GO

CREATE TABLE CompetencyEvaluationMappings (
	CompetencyId int NOT NULL FOREIGN KEY REFERENCES Competencies(CompetencyId) ON UPDATE CASCADE ON DELETE CASCADE,
	EvaluationTypeId int NOT NULL FOREIGN KEY REFERENCES EvaluationTypes(EvaluationTypeId) ON UPDATE CASCADE ON DELETE CASCADE,
	BonusScoreLT50 int NOT NULL,
	BonusScoreMTE50 int NOT NULL,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY(CompetencyId, EvaluationTypeId)
)
GO

-- Coach
CREATE TABLE Coaches(
	CoachId int NOT NULL PRIMARY KEY IDENTITY,
	EmployeeId varchar(64) NOT NULL FOREIGN KEY REFERENCES Employees(EmployeeId),
	CoachName varchar (64) NOT NULL,
	CoachIsActive BIT NOT NULL,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
)
GO

CREATE TABLE CoachTopicMappings(
	CoachId int NOT NULL FOREIGN KEY REFERENCES Coaches(CoachId) ON UPDATE CASCADE ON DELETE CASCADE,
	TopicId int NOT NULL FOREIGN KEY REFERENCES Topics(TopicId) ON UPDATE CASCADE ON DELETE CASCADE,
	PRIMARY KEY(CoachId, TopicId)
)
GO

CREATE TABLE CoachSchedules (
	CoachScheduleId int NOT NULL PRIMARY KEY IDENTITY,
	CoachId int NOT NULL,
	StartDateTime datetime2 (7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	EndDateTime datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (CoachId) REFERENCES Coaches(CoachId) ON UPDATE CASCADE ON DELETE CASCADE
)
GO

-- Digital Signature​
CREATE TABLE DigitalSignatures (
	DigitalSignatureId int PRIMARY KEY IDENTITY,
	EmployeeId varchar(64) NOT NULL FOREIGN KEY REFERENCES Employees(EmployeeId) ON DELETE CASCADE ON UPDATE CASCADE,
	BlobId uniqueidentifier FOREIGN KEY REFERENCES Blobs(BlobId) ON UPDATE CASCADE ON DELETE CASCADE,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	CreatedBy varchar(64) NOT NULL,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	IsDeleted bit NOT NULL
)
GO

-- Course
CREATE TABLE ProgramTypes
(
	ProgramTypeId int PRIMARY KEY IDENTITY,
	ProgramTypeName varchar(128) NOT NULL
)
GO

INSERT INTO ProgramTypes(ProgramTypeName) VALUES
	('Entry Development Training Program'),
	('Mandatory Training Program'),
	('Thematic Training Program'),
	('Instructor Development Program')
GO

CREATE TABLE Levels
(
	LevelId int PRIMARY KEY IDENTITY,
	LevelName varchar(128) not NULL
)
GO

INSERT INTO Levels(LevelName) VALUES
	('On Boarding'),
	('Development Program'),
	('Instructor Development Program'),
	('Instructor Retention Program'),
	('Level 1'),
	('Level 2'),
	('Level 3'),
	('Level 4'),
	('Level 5'),
	('N/A')
GO

CREATE TABLE LearningTypes
(
	LearningTypeId int PRIMARY KEY IDENTITY,
	LearningName varchar(32) NOT NULL
)
GO

INSERT INTO LearningTypes(LearningName) VALUES
	('Online'),
	('Offline'),
	('Online/Offline')
GO

CREATE TABLE CourseCategories
(
	CourseCategoryId int PRIMARY KEY IDENTITY,
	CourseCategoryName varchar(64) NOT NULL
)
GO

INSERT INTO CourseCategories(CourseCategoryName) VALUES
	('Position'),
	('Product'),
	('Non-Product'),
	('Others')
GO

CREATE TABLE ApprovalStatuses(
	ApprovalId int PRIMARY KEY IDENTITY,
	ApprovalName varchar(64) NOT NULL
)
GO

INSERT INTO ApprovalStatuses(ApprovalName) VALUES
	('Approved'),
	('Waiting Approval'),
	('Draft')
GO

CREATE TABLE Courses
(
	CourseId int PRIMARY KEY IDENTITY,
	ProgramTypeId int NOT NULL FOREIGN KEY REFERENCES ProgramTypes(ProgramTypeId) ON DELETE CASCADE ON UPDATE CASCADE,
	LevelId int NOT NULL FOREIGN KEY REFERENCES Levels(LevelId) ON DELETE CASCADE ON UPDATE CASCADE,
	CourseCategoryId int NOT NULL FOREIGN KEY REFERENCES CourseCategories(CourseCategoryId) ON DELETE CASCADE ON UPDATE CASCADE,
	LearningTypeId int NOT NULL FOREIGN KEY REFERENCES LearningTypes(LearningTypeId) ON DELETE CASCADE ON UPDATE CASCADE,
	BlobId uniqueidentifier NOT NULL FOREIGN KEY  REFERENCES Blobs(BlobId) ON DELETE CASCADE ON UPDATE CASCADE,
	ApprovalId int NULL FOREIGN KEY REFERENCES ApprovalStatuses(ApprovalId) ON DELETE CASCADE ON UPDATE CASCADE,
	CourseName varchar(64) NOT NULL,
	CoursePrice int NULL,
	CourseDescription varchar(1024) NULL,
	Others varchar(64) NULL,
	IsRecommendedForYou BIT NULL,
	IsPopular BIT NULL,
	IsPublished BIT NULL DEFAULT 0,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	CreatedBy varchar(64) NOT NULL,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	IsDeleted bit DEFAULT 0
)
GO


CREATE TABLE [CourseLearningObjectives](
	[LearningObjectiveId] int PRIMARY KEY IDENTITY,
	CourseId int FOREIGN KEY REFERENCES Courses(CourseId) NOT NULL,
	[LearningObjectiveName] varchar(64) NOT NULL
)
GO


CREATE TABLE [dbo].[CoursePrerequisiteMappings](
	[CourseId]  int FOREIGN KEY REFERENCES Courses(CourseId) NOT NULL,
	 ModuleId int NOT NULL FOREIGN KEY REFERENCES Modules(ModuleId) NOT NULL,
	 PRIMARY KEY([CourseId], ModuleId)
) 
GO


-- Module
CREATE TABLE MaterialTypes(
	MaterialTypeId int PRIMARY KEY IDENTITY,
	MaterialTypeName varchar(64) NOT NULL
)
GO

INSERT INTO MaterialTypes(MaterialTypeName) VALUES
	('Podcast'),
	('Video Learning'),
	('Journal Game')
GO

CREATE TABLE Materials(
	MaterialId int PRIMARY KEY IDENTITY,
	MaterialTypeId int NOT NULL FOREIGN KEY REFERENCES MaterialTypes(MaterialTypeId) ON DELETE CASCADE ON UPDATE CASCADE,
	BlobId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES Blobs(BlobId) ON DELETE CASCADE ON UPDATE CASCADE,
	MaterialLink varchar(1024),
	MaterialDownloadable bit NOT NULL,
)
GO

CREATE TABLE Modules(
	ModuleId int PRIMARY KEY IDENTITY,
	BlobId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES Blobs(BlobId) ON UPDATE CASCADE ON DELETE CASCADE,
	MaterialId int NOT NULL FOREIGN KEY REFERENCES Materials(MaterialId) ON UPDATE NO ACTION ON DELETE NO ACTION,
	ApprovalId int NOT NULL FOREIGN KEY REFERENCES ApprovalStatuses(ApprovalId) ON UPDATE CASCADE ON DELETE CASCADE,
	ModuleName varchar(64) NOT NULL,
	ModuleDescription varchar(1024) NOT NULL,
	IsPublished bit NOT NULL,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP
)
GO

CREATE TABLE MaterialTopicMappings (
	MaterialId int FOREIGN KEY REFERENCES Materials(MaterialId),
	TopicId int FOREIGN KEY REFERENCES Topics(TopicId),
	PRIMARY KEY(MaterialId, TopicId)
)
GO

CREATE TABLE TrainingTypes
(
	TrainingTypeId int PRIMARY KEY IDENTITY,
	TrainingTypeName VARCHAR(64) NOT NULL,
)
GO

INSERT INTO TrainingTypes(TrainingTypeName) VALUES
	('Pre'),
	('During'),
	('Post')
GO

-- Time Point
CREATE TABLE PointTypes (
	PointTypeId int PRIMARY KEY IDENTITY,
	PointTypeName varchar(64) NOT NULL
)
GO
INSERT INTO PointTypes(PointTypeName) VALUES
	('Learning Time'), 
	('Teaching Time'), 
	('Coaching Time'), 
	('NPS'), 
	('Task')
GO

CREATE TABLE TimePoints (
	TimePointId int PRIMARY KEY IDENTITY,
	PointTypeId int NOT NULL FOREIGN KEY REFERENCES PointTypes(PointTypeId) ON DELETE CASCADE ON UPDATE CASCADE,
	"Time" int NULL,
	Score int NULL,
	Points int NOT NULL,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP
)
GO

-- Task
CREATE TABLE QuestionTypes(
    QuestionTypeId int NOT NULL PRIMARY KEY IDENTITY,
    QuestionTypeName varchar(64) NOT NULL
)
GO

INSERT INTO QuestionTypes(QuestionTypeName) VALUES
	('True/False'),
	('Matching'),
	('Sequence'),
	('Tebak Gambar'),
	('Hot Spot'),
	('Short Answer'),
	('Open Question/Essay'),
	('Checklist'),
	('Rating'),
	('Multiple Choice'),
	('File Upload'),
	('Matrix')
GO

CREATE TABLE Tasks(
    TaskId int NOT NULL PRIMARY KEY IDENTITY,
    CompetencyId int NOT NULL FOREIGN KEY REFERENCES Competencies(CompetencyId) ON DELETE CASCADE ON UPDATE CASCADE ,
    QuestionTypeId int NOT NULL FOREIGN KEY REFERENCES QuestionTypes(QuestionTypeId) ON UPDATE CASCADE ON DELETE CASCADE,
    ModuleId int NOT NULL FOREIGN KEY REFERENCES Modules(ModuleId) ON UPDATE CASCADE ON DELETE CASCADE,
	BlobId uniqueidentifier NULL FOREIGN KEY REFERENCES Blobs(BlobId) ON UPDATE NO ACTION ON DELETE NO ACTION,
    TaskNumber int NOT NULL,
	LayoutTeype int NOT NULL,
	StoryLineDescription varchar(1024) NULL,
    IsDeleted bit NOT NULL DEFAULT 0,
    CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CreatedBy varchar(255) NOT NULL,
    UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	EvaluationTypeId int NOT NULL FOREIGN KEY REFERENCES EvaluationTypes(EvaluationTypeId)
)
GO

CREATE TABLE TaskTrueFalseTypes(
    TaskId int NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
    Question varchar(1024) NOT NULL,
    Answer bit NOT NULL,
    Score int NOT NULL,
)
GO

CREATE TABLE TaskMatchingChoices(
    TaskMatchingChoiceId int NOT NULL PRIMARY KEY IDENTITY,
    TaskId int not null FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
    BlobId uniqueidentifier NULL FOREIGN KEY REFERENCES Blobs(BlobId) ON UPDATE NO ACTION ON DELETE NO ACTION,
	TaskMatchingChoiceCode char(3) NOT NULL,
    Text varchar(64) NULL
)
GO 

CREATE TABLE TaskMatchingTypes(
    TaskId int NOT NULL FOREIGN KEY REFERENCES Tasks(TaskID) ON DELETE CASCADE ON UPDATE CASCADE PRIMARY KEY,
    Question varchar(1024) NOT NULL,
    Answer varchar(256) NOT NULL,
    Score int NOT NULL,
)
GO

CREATE TABLE TaskSequenceChoices(
    TaskSequenceChoiceId int NOT NULL PRIMARY KEY IDENTITY,
    TaskID int NOT NULL FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
    Number int NOT NULL,
    Text varchar(64) NOT NULL,
    Score int NOT NULL
)
GO

CREATE TABLE TaskSequenceTypes(
    TaskId int NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
    Question varchar(1024) NOT NULL,
    Answer varchar(256) NOT NULL
)
GO

CREATE TABLE TaskTebakGambarPictures (
	TaskTebakGambarId int PRIMARY KEY IDENTITY,
	TaskId int NOT NULL FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
	BlobId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES Blobs(BlobId) ON UPDATE NO ACTION ON DELETE NO ACTION,
	Number int NOT NULL
)
GO

CREATE TABLE TaskTebakGambarTypes (
	TaskId int NOT NULL FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
	Question varchar(1024) NOT NULL,
	Answer varchar(256) NOT NULL,
	Score int NOT NULL,
	PRIMARY KEY(TaskId)
)
GO

CREATE TABLE TaskHotSpotAnswers (
	TaskHotSpotAnswerId int PRIMARY KEY IDENTITY,
	TaskId int NOT NULL FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
	[Number] [int] NOT NULL,
	Answer int NOT NULL,
	Score int NOT NULL
)
GO

CREATE TABLE TaskHotSpotTypes (
	TaskId int NOT NULL FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
	BlobId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES Blobs(BlobId) ON UPDATE NO ACTION ON DELETE NO ACTION,
	Question varchar(1024) NOT NULL,
	PRIMARY KEY(TaskId)
)
GO

CREATE TABLE TaskShortAnswerTypes(
	TaskId int NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
	Question varchar(512) NOT NULL
)
GO

CREATE TABLE TaskEssayTypes(
	TaskId int NOT NULL FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
	Question varchar(1024) NOT NULL,
	PRIMARY KEY (TaskId)
)
GO

CREATE TABLE TaskChecklistChoices(
	TaskChecklistChoiceId int PRIMARY KEY IDENTITY,
	TaskId int NOT NULL FOREIGN KEY REFERENCES Tasks(TaskId) ON DELETE CASCADE ON UPDATE CASCADE,
	Number int NOT NULL,
	Text varchar(64) NOT NULL,
	IsAnswer bit NOT NULL,
	Score int NOT NULL,
)
GO


CREATE TABLE TaskChecklistTypes(
	TaskId int PRIMARY KEY FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
	Question varchar(1024) NOT NULL
)
GO

CREATE TABLE TaskRatingChoices(
	TaskRatingChoiceId int PRIMARY KEY IDENTITY,
	TaskId int NOT NULL FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
	Number int NOT NULL,
	Text varchar(64) NOT NULL,
)
GO

CREATE TABLE TaskRatingTypes(
	TaskId int PRIMARY KEY FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
	Question varchar(1024) NOT NULL,
	Score0To20 int NOT NULL,
	Socre21To40 int NOT NULL,
	Score41To60 int NOT NULL,
	Score61To80 int NOT NULL,
	Score81To100 int NOT NULL
)
GO

CREATE TABLE TaskMultipleChoiceChoices(
	TaskMultipleChoiceChoiceId int PRIMARY KEY IDENTITY,
	TaskId int NOT NULL FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
	Number int NOT NULL,
	Text varchar(64) NOT NULL,
)
GO

CREATE TABLE TaskMultipleChoiceTypes(
	TaskId int PRIMARY KEY FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE CASCADE ON DELETE CASCADE,
	AnswerId int NOT NULL FOREIGN KEY REFERENCES TaskMultipleChoiceChoices(TaskMultipleChoiceChoiceId) ON UPDATE NO ACTION ON DELETE NO ACTION,
	Question varchar(1024) NOT NULL,
	Score int NOT NULL,
)
GO

CREATE TABLE TaskFileUploadTypes (
	TaskId int NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Tasks(TaskId) ON DELETE CASCADE ON UPDATE CASCADE,
	Question varchar(1024) NOT NULL
)
GO

CREATE TABLE TaskMatrixTypes (
	TaskId int NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Tasks(TaskId) ON DELETE CASCADE ON UPDATE CASCADE,
	Question varchar(1024) NOT NULL,
	ScoreColumn1 int NOT NULL,
	ScoreColumn2 int NOT NULL,
	ScoreColumn3 int NOT NULL,
	ScoreColumn4 int NOT NULL,
	ScoreColumn5 int NOT NULL
)
GO

CREATE TABLE TaskMatrixChoices (
	TaskMatrixChoiceId int PRIMARY KEY IDENTITY,
	TaskId int FOREIGN KEY REFERENCES Tasks(TaskId),
	Text varchar(64) NOT NULL
)
GO

CREATE TABLE SetupModules (
	SetupModuleId int PRIMARY KEY IDENTITY,
	CourseId int NULL FOREIGN KEY REFERENCES Courses(CourseId) ON UPDATE CASCADE ON DELETE CASCADE,
	ModuleId int NOT NULL FOREIGN KEY REFERENCES Modules(ModuleId) ON UPDATE NO ACTION ON DELETE NO ACTION,
	TrainingTypeId int NOT NULL FOREIGN KEY REFERENCES TrainingTypes(TrainingTypeId) ON UPDATE CASCADE ON DELETE CASCADE,
	TimePointId int NOT NULL FOREIGN KEY REFERENCES TimePoints(TimePointId) ON UPDATE CASCADE ON DELETE CASCADE,
    IsRecommendedForYou bit NOT NULL,
    IsPopular bit NOT NULL,
    MinimumScore int NOT NULL,
    IsForRemedial bit NOT NULL,
)
GO

CREATE TABLE PopQuizes (
	PopQuizId int PRIMARY KEY IDENTITY,
    ApprovalId int NOT NULL FOREIGN KEY REFERENCES ApprovalStatuses(ApprovalId) ON UPDATE CASCADE ON DELETE CASCADE,
    PopQuizName varchar(64) NOT NULL,
    IsPublished bit NOT NULL,
    CreatedAt datetime2(7) NOT NULL,
    UpdatedAt datetime2(7) NOT NULL,
)
GO

CREATE TABLE SetupTasks (
	SetupTaskId int PRIMARY KEY IDENTITY,
    SetupModuleId int NULL FOREIGN KEY REFERENCES SetupModules(SetupModuleId) ON UPDATE NO ACTION ON DELETE NO ACTION,
    PopQuizId int NULL FOREIGN KEY REFERENCES PopQuizes(PopQuizId) ON UPDATE NO ACTION ON DELETE NO ACTION,
    CompetencyId int NULL FOREIGN KEY REFERENCES Competencies(CompetencyId) ON UPDATE CASCADE ON DELETE CASCADE,
    ModuleId int NULL FOREIGN KEY REFERENCES Modules(ModuleId) ON UPDATE CASCADE ON DELETE CASCADE,
    TestTime int NOT NULL,
    IsGrouping bit NOT NULL,
    TotalParticipant int NULL,
    TotalQuestion int NULL,
    QuestionPerParticipant int NULL,
)
GO

CREATE TABLE SetupTaskCodes (
	SetupTaskId int NOT NULL FOREIGN KEY REFERENCES SetupTasks(SetupTaskId) ON UPDATE CASCADE ON DELETE CASCADE,
	TaskId int NOT NULL FOREIGN KEY REFERENCES Tasks(TaskId) ON UPDATE NO ACTION ON DELETE NO ACTION,
    QuestionNumber int NOT NULL,
	PRIMARY KEY(SetupTaskId, TaskId)
)
GO

-- Setup Learning & Release Training
CREATE TABLE TrainingServiceLevels (
	TrainingServiceLevelId int PRIMARY KEY,
    TrainingServiceLevelName varchar(64) NOT NULL,
    TrainingServiceLevelBasicLevel int NOT NULL,
    TrainingServiceLevelFundamentalLevel int NOT NULL,
    TrainingServiceLevelAdvanceLevel int NOT NULL,
)
GO

CREATE TABLE Trainings (
    TrainingId int PRIMARY KEY IDENTITY,
    SetupModuleId int NOT NULL FOREIGN KEY REFERENCES SetupModules(SetupModuleId) ON UPDATE CASCADE ON DELETE CASCADE,
    ApprovalId int NOT NULL FOREIGN KEY REFERENCES ApprovalStatuses(ApprovalId) ON UPDATE NO ACTION ON DELETE NO ACTION,
    TrainingBatch int NOT NULL,
    TrainingStartDate date NOT NULL,
    TrainingEndDate date NOT NULL,
    TrainingQuota int NOT NULL,
    TrainingLocation varchar NOT NULL,
    IsPublished bit NOT NULL,
    IsParticipantTrainee bit NOT NULL,
    CreatedAt datetime2(7) NOT NULL,
    UpdatedAt datetime2(7) NOT NULL,
)
GO

CREATE TABLE TrainingModuleMappings (
    TrainingId int PRIMARY KEY IDENTITY,
    ModuleId int NOT NULL FOREIGN KEY REFERENCES  SetupModules(SetupModuleId) ON UPDATE CASCADE ON DELETE CASCADE,
    TimePointId int NOT NULL FOREIGN KEY REFERENCES TimePoints(TimePointId) ON UPDATE NO ACTION ON DELETE NO ACTION,
    CoachId int NOT NULL FOREIGN KEY REFERENCES Coaches(CoachId) ON UPDATE NO ACTION ON DELETE NO ACTION,
    TrainingStart datetime2(7) NULL,
    TrainingEnd datetime2(7) NULL,
)
GO

CREATE TABLE TrainingAreaMappings (
    TrainingId int NOT NULL FOREIGN KEY REFERENCES Trainings(TrainingId) ON UPDATE CASCADE ON DELETE CASCADE,
    AreaId int NOT NULL FOREIGN KEY REFERENCES Areas(AreaId) ON UPDATE CASCADE ON DELETE CASCADE,
	PRIMARY KEY(TrainingId, AreaId),
)
GO

CREATE TABLE TrainingRegionMappings (
    TrainingId int NOT NULL FOREIGN KEY REFERENCES Trainings(TrainingId) ON UPDATE CASCADE ON DELETE CASCADE,
    RegionId int NOT NULL FOREIGN KEY REFERENCES Regions(RegionId) ON UPDATE CASCADE ON DELETE CASCADE,
	PRIMARY KEY(TrainingId, RegionId),
)
GO

CREATE TABLE TrainingDealerMappings (
    TrainingId int NOT NULL FOREIGN KEY REFERENCES Trainings(TrainingId) ON UPDATE CASCADE ON DELETE CASCADE,
    DealerId varchar(64) NOT NULL FOREIGN KEY REFERENCES Dealers(DealerId) ON UPDATE CASCADE ON DELETE CASCADE,
	PRIMARY KEY(TrainingId, DealerId),
)
GO

CREATE TABLE TrainingProvinceMappings (
    TrainingId int NOT NULL FOREIGN KEY REFERENCES Trainings(TrainingId) ON UPDATE CASCADE ON DELETE CASCADE,
    ProvinceId varchar(32) NOT NULL FOREIGN KEY REFERENCES Provinces(ProvinceId) ON UPDATE CASCADE ON DELETE CASCADE,
	PRIMARY KEY(TrainingId, ProvinceId),
)
GO

CREATE TABLE TrainingCityMappings (
    TrainingId int NOT NULL FOREIGN KEY REFERENCES Trainings(TrainingId) ON UPDATE CASCADE ON DELETE CASCADE,
    CityId varchar(32) NOT NULL FOREIGN KEY REFERENCES Cities(CityId) ON UPDATE CASCADE ON DELETE CASCADE,
	PRIMARY KEY(TrainingId, CityId),
)
GO

CREATE TABLE TrainingOutletMappings (
    TrainingId int NOT NULL FOREIGN KEY REFERENCES Trainings(TrainingId) ON UPDATE CASCADE ON DELETE CASCADE,
    OutletId varchar(64) NOT NULL FOREIGN KEY REFERENCES Outlets(OutletId) ON UPDATE CASCADE ON DELETE CASCADE,
	PRIMARY KEY(TrainingId, OutletId),
)
GO

CREATE TABLE TrainingPositionMappings (
    TrainingId int NOT NULL FOREIGN KEY REFERENCES Trainings(TrainingId) ON UPDATE CASCADE ON DELETE CASCADE,
    PositionId int NOT NULL FOREIGN KEY REFERENCES Positions(PositionId) ON UPDATE CASCADE ON DELETE CASCADE,
	PRIMARY KEY(TrainingId, PositionId),
)
GO

--My Team
CREATE TABLE Teams (
	TeamId int PRIMARY KEY IDENTITY,
	CoachId int NOT NULL FOREIGN KEY REFERENCES Coaches(CoachId) ON UPDATE CASCADE ON DELETE CASCADE
)
GO

CREATE TABLE CoachEmployeeMappings (
	TeamId int FOREIGN KEY REFERENCES Teams(TeamId) ON UPDATE CASCADE ON DELETE CASCADE,
	EmployeeId int FOREIGN KEY REFERENCES Coaches(CoachId) ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO

CREATE TABLE CourseTeamMappings (
	CourseId int FOREIGN KEY REFERENCES Courses(CourseId) ON UPDATE CASCADE ON DELETE CASCADE,
	TeamId int FOREIGN KEY REFERENCES Teams(TeamId) ON UPDATE CASCADE ON DELETE CASCADE
)
GO

--My Events
CREATE TABLE EventCategories (
	EventCategoryId int PRIMARY KEY IDENTITY,
	EventCategoryName varchar(64) NOT NULL,
	EventCategoryDescription varchar(1024) NULL,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP
)
GO

CREATE TABLE [Events] (
	EventId int PRIMARY KEY IDENTITY,
	EventCategoryId int NOT NULL FOREIGN KEY REFERENCES EventCategories(EventCategoryId) ON UPDATE CASCADE ON DELETE CASCADE,
	TopicId int NOT NULL FOREIGN KEY REFERENCES Topics(TopicId) ON UPDATE CASCADE ON DELETE CASCADE,
	BlobId uniqueidentifier FOREIGN KEY REFERENCES Blobs(BlobId) ON UPDATE NO ACTION ON DELETE NO ACTION,
	EventName varchar(64) NOT NULL,
	StartDateTime datetime2 (7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	EndDateTime datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	EventHostName varchar(64) NOT NULL,
	EventDescription varchar(1024) NULL,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP
)
GO

--My Points
CREATE TABLE RewardTypes (
	RewardTypeId int PRIMARY KEY IDENTITY,
	RewardName varchar(64) NOT NULL
)
GO
INSERT INTO RewardTypes(RewardName) VALUES
	('Modules'),
	('Coach'),
	('Seminar'),
	('Merchandise'),
	('Voucher'),
	('Trip'),
	('Training Discount'),
	('Learning & Training Tools')
GO

CREATE TABLE Rewards (
	RewardId int PRIMARY KEY IDENTITY,
	RewardTypeId int NOT NULL FOREIGN KEY REFERENCES RewardTypes(RewardTypeId) ON UPDATE CASCADE ON DELETE CASCADE,
	ModuleId int NULL FOREIGN KEY REFERENCES Modules(ModuleId) ON UPDATE CASCADE ON DELETE CASCADE,
	CoachId int NULL FOREIGN KEY REFERENCES Coaches(CoachId) ON UPDATE CASCADE ON DELETE CASCADE,
	EventId int NULL FOREIGN KEY REFERENCES [Events](EventId) ON UPDATE NO ACTION ON DELETE NO ACTION,
	RewardName varchar(64) NOT NULL,
	RewardRequiredLearningPoints int NOT NULL DEFAULT 0,
	RewardRequiredTeachingPoints int NOT NULL DEFAULT 0,
	RewardRequiredTotalPoints int NOT NULL DEFAULT 0,
	RewardIsActive bit NOT NULL,
	RewardDescription varchar(1024) NULL,
	RewardTermsAndConditions varchar(1024) NOT NULL,
	RewardHowToUse varchar(1024) NOT NULL,
	CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP
)
GO

--Setup Learning 
CREATE TABLE SetupLearning (
	SetupLearningId int PRIMARY KEY IDENTITY,
    SetupModuleId int NULL FOREIGN KEY REFERENCES SetupModules(SetupModuleId) ON UPDATE NO ACTION ON DELETE NO ACTION,
    PopQuizId int NULL FOREIGN KEY REFERENCES PopQuizes(PopQuizId) ON UPDATE NO ACTION ON DELETE NO ACTION,
    CourseId int NULL FOREIGN KEY REFERENCES Courses(CourseId) ON UPDATE NO ACTION ON DELETE NO ACTION,
	[LearningName] varchar(64) NOT NULL,
	ProgramTypeName varchar(64) NOT NULL,
	LearningCategoryName varchar(64) NOT NULL,
	ApprovalStatus varchar(64) NOT NULL,
    CreatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt datetime2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	IsPublished BIT NULL DEFAULT 0
)
GO 


-- CourseRatings
CREATE TABLE CourseRatings 
(
	CourseRatingId INT 
		CONSTRAINT PK_CourseRatings PRIMARY KEY IDENTITY,

	EmployeeId VARCHAR(64) NOT NULL 
		CONSTRAINT FK_CourseRatings_Employees FOREIGN KEY REFERENCES Employees(EmployeeId),

	CourseId INT NOT NULL 
		CONSTRAINT FK_CourseRating_Courses FOREIGN KEY REFERENCES Courses(CourseId),
	
	RatingScore INT NOT NULL,

	CreatedAt DATETIME2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	CreatedBy VARCHAR(64) NOT NULL,
	UpdatedAt DATETIME2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	IsDeleted BIT NOT NULL DEFAULT 0
)