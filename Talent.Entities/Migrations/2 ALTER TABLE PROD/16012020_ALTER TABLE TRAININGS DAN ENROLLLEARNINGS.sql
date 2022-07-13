ALTER TABLE Trainings
	ADD RescheduledAt DATETIME2(7) NULL

ALTER TABLE EnrollLearnings
	ADD IsRejected BIT NOT NULL
		CONSTRAINT DF_EnrollLearnings_IsRejected DEFAULT 0,
		IsJoined BIT NOT NULL
		CONSTRAINT DF_EnrollLearnings_HasJoined DEFAULT 0