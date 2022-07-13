CREATE TABLE CalculateLearningQueue(
	
    CalculateLearningQueueId uniqueidentifier DEFAULT NEWID()
        CONSTRAINT PK_CalculateLearningQueue PRIMARY KEY,
	EnrollLearningId INT NOT NULL
        CONSTRAINT FK_CalculateLearningQueue_EnrollLearning FOREIGN KEY REFERENCES EnrollLearnings(EnrollLearningId) ON UPDATE CASCADE ON DELETE CASCADE,
    FinishedAt DATETIME2 (7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
	EnrollType varchar(64) NOT NULL,
	SetupModuleId INT NULL
		CONSTRAINT FK_CalculateLearningQueue_SetupModules FOREIGN KEY REFERENCES SetupModules(SetupModuleId)
)