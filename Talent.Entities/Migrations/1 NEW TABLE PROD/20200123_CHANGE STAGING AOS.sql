DROP TABLE StagingActualOrganizationStructure

CREATE TABLE StagingActualOrganizationStructure (
	ID INT NOT NULL,
	Code VARCHAR(250) NOT NULL
		CONSTRAINT PK_StagingActualOrganizationStructure PRIMARY KEY,
	NoReg VARCHAR(20) NULL,
	PostCode VARCHAR(20) NULL,
	PostName VARCHAR(150) NULL,
	Staffing DECIMAL(38, 2) NULL,
	State VARCHAR(250) NOT NULL
)