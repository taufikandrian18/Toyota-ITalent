ALTER TABLE StagingActualOrganizationStructure
ADD MUID UNIQUEIDENTIFIER NULL

ALTER TABLE StagingActualOrganizationStructure
ADD VersionName VARCHAR(50) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD VersionNumber INT NULL

ALTER TABLE StagingActualOrganizationStructure
ADD Version_ID INT NULL

ALTER TABLE StagingActualOrganizationStructure
ADD VersionFlag VARCHAR(50) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD Name VARCHAR(250) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD ChangeTrackingMask INT NULL

ALTER TABLE StagingActualOrganizationStructure
ADD Service VARCHAR(50) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD EmployeeGroup VARCHAR(10) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD EmployeeGroupText VARCHAR(150) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD EmployeeSubgroup VARCHAR(50) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD EmployeeSubgroupText VARCHAR(150) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD WorkContract VARCHAR(10) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD WorkContractText VARCHAR(150) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD PersonalArea VARCHAR(10) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD PersonalSubarea VARCHAR(10) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD DepthLevel DECIMAL(38, 0) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD Chief DECIMAL(38, 0) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD OrgLevel DECIMAL(38, 0) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD Period DATETIME2 NULL

ALTER TABLE StagingActualOrganizationStructure
ADD Vacant DECIMAL(38, 0) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD Structure VARCHAR(500) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD ObjectDescription VARCHAR(50) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD EnterDateTime DATETIME2 NULL

ALTER TABLE StagingActualOrganizationStructure
ADD EnterUserName VARCHAR(100) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD EnterVersionNumber INT NULL

ALTER TABLE StagingActualOrganizationStructure
ADD LastChgDateTime DATETIME2 NULL

ALTER TABLE StagingActualOrganizationStructure
ADD LastChgUserName VARCHAR(100) NULL

ALTER TABLE StagingActualOrganizationStructure
ADD LastChgVersionNumber INT NULL

ALTER TABLE StagingActualOrganizationStructure
ADD ValidationStatus VARCHAR(250) NULL