CREATE TABLE Accommodations (
	AccommodationId INT NOT NULL
		CONSTRAINT PK_Accommodations PRIMARY KEY,

	[Name] VARCHAR(255) NOT NULL,
	Price INT NOT NULL
)
GO

CREATE TABLE TrainingInvitations (
	TrainingInvitationId INT NOT NULL
		CONSTRAINT PK_TrainingInvitations PRIMARY KEY IDENTITY,

	TrainingId INT NOT NULL
		CONSTRAINT FK_TrainingInvitations_Trainings FOREIGN KEY REFERENCES Trainings(TrainingId),

	EmployeeId VARCHAR(64) NOT NULL
		CONSTRAINT FK_TrainingInvitations_Employees FOREIGN KEY REFERENCES Employees(EmployeeId),

	ApprovalStatusId INT NOT NULL
		CONSTRAINT FK_TrainingInvitations_ApprovalStatus FOREIGN KEY REFERENCES ApprovalStatus(ApprovalStatusId),

	CreatedAt DATETIME2(7) NOT NULL
		CONSTRAINT DF_TrainingInvitations_CreatedAt DEFAULT CURRENT_TIMESTAMP,

	CreatedBy VARCHAR(64) NOT NULL
		CONSTRAINT DF_TrainingInvitations_CreatedBy DEFAULT 'SYSTEM',

	UpdatedAt DATETIME2(7) NOT NULL
		CONSTRAINT DF_TrainingInvitations_UpdatedAt DEFAULT CURRENT_TIMESTAMP,

	UpdatedBy VARCHAR(64) NOT NULL
		CONSTRAINT DF_TrainingInvitations_UpdatedBy DEFAULT 'SYSTEM',
)
GO

CREATE TABLE TrainingProcesses (
	TrainingProcessId INT NOT NULL
		CONSTRAINT PK_TrainingProcesses PRIMARY KEY IDENTITY,

	TrainingInvitationId INT NOT NULL
		CONSTRAINT FK_TrainingProcesses_TrainingInvitation FOREIGN KEY REFERENCES TrainingInvitations(TrainingInvitationId),

	AccomodationId INT NULL
		CONSTRAINT FK_TrainingProcesses_Accommodation FOREIGN KEY REFERENCES Accommodations(AccommodationId),

	DateofStayStart DATETIME2(7) NULL,
	DateofStayEnd DATETIME2(7) NULL,

	IsConfirmed BIT NOT NULL 
		CONSTRAINT DF_TrainingProcesses_IsConfirmed DEFAULT 0,

	CreatedAt DATETIME2(7) NOT NULL 
		CONSTRAINT DF_TrainingProcesses_CreatedAt DEFAULT CURRENT_TIMESTAMP,

	CreatedBy VARCHAR(64) NOT NULL
		CONSTRAINT DF_TrainingProcesses_CreatedBy DEFAULT 'SYSTEM',

	UpdatedAt DATETIME2(7) NOT NULL
		CONSTRAINT DF_TrainingProcesses_UpdatedAt DEFAULT CURRENT_TIMESTAMP,	

	UpdatedBy VARCHAR(64) NOT NULL
		CONSTRAINT DF_TrainingProcesses_UpdatedBy DEFAULT 'SYSTEM',
)
GO