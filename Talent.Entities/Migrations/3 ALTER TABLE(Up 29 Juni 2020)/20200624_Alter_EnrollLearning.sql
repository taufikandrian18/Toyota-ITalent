ALTER TABLE EnrollLearnings
	ADD IsDrafted BIT NOT NULL
		CONSTRAINT DF_EnrollLearnings_IsDrafted DEFAULT 0