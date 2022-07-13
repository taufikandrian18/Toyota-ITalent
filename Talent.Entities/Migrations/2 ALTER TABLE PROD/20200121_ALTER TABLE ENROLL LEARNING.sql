ALTER TABLE EnrollLearnings
	ADD SetupModuleId INT NULL 
		CONSTRAINT FK_EnrollLearnings_SetupModules FOREIGN KEY REFERENCES SetupModules(SetupModuleId)