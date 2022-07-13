CREATE TABLE StagingOrganizationObject (
	ID INT NOT NULL,
	Code VARCHAR(250) NOT NULL
		CONSTRAINT PK_StagingOrganizationObject PRIMARY KEY,
	ObjectType VARCHAR(10) NULL,
	ObjectID VARCHAR(20) NULL,
	Abbreviation VARCHAR(50) NULL,
	ObjectText VARCHAR(250) NULL,
	ObjectDescription VARCHAR(250) NULL,
	State VARCHAR(250) NULL
)

ALTER TABLE StagingActualOrganizationStructure
ADD OrgCode VARCHAR(20)

ALTER TABLE StagingActualOrganizationStructure
ADD ParentOrgCode VARCHAR(20)

ALTER TABLE StagingActualOrganizationStructure
ADD OrgName VARCHAR(150)

ALTER TABLE StagingActualOrganizationStructure
ADD JobCode VARCHAR(20)

ALTER TABLE StagingActualOrganizationStructure
ADD JobName VARCHAR(150)

ALTER TABLE StagingActualOrganizationStructure
ALTER COLUMN State VARCHAR(250) NULL