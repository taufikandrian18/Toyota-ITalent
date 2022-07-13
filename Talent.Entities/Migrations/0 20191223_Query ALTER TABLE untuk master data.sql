--ALTER TABLE UNTUK TAMBAHIN UpdatedAt
ALTER TABLE SetupModules 
    ADD UpdatedAt DATETIME2 NOT NULL 
            CONSTRAINT DF_SetupModules_UpdatedAt DEFAULT CURRENT_TIMESTAMP
ALTER TABLE Courses 
    ADD SetupCourseUpdatedAt DATETIME2 NOT NULL 
        CONSTRAINT DF_SetupCourse_UpdatedAt DEFAULT CURRENT_TIMESTAMP
ALTER TABLE TrainingServiceLevels 
    ADD UpdatedAt DATETIME2 NOT NULL 
        CONSTRAINT DF_TrainingServiceLevels_UpdatedAt DEFAULT CURRENT_TIMESTAMP
ALTER TABLE Inboxes 
    ADD UpdatedAt DATETIME2 NOT NULL 
        CONSTRAINT DF_Inboxes_UpdatedAt DEFAULT CURRENT_TIMESTAMP

--ALTER TABLE UNTUK TAMBAHIN CreatedAt
ALTER TABLE SetupModules 
    ADD CreatedAt DATETIME2 NOT NULL 
        CONSTRAINT DF_SetupModules_CreatedAt DEFAULT CURRENT_TIMESTAMP
ALTER TABLE Courses
	ADD SetupCourseCreatedAt DATETIME2 NOT NULL
		CONSTRAINT DF_SetupCourse_CreatedAt DEFAULT CURRENT_TIMESTAMP
ALTER TABLE TrainingServiceLevels 
    ADD CreatedAt DATETIME2 NOT NULL 
        CONSTRAINT DF_TrainingServiceLevels_CreatedAt DEFAULT CURRENT_TIMESTAMP

--ALTER TABLE UNTUK TAMBAHIN UpdatedBy
ALTER TABLE KeyActions 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_KeyActions_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Competencies 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Competencies_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Topics 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Topics_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE CompetencyEvaluationMappings 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_CompetencyEvaluationMappings_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Coaches 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Coaches_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE DigitalSignatures 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_DigitalSignatures_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Courses 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Courses_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Modules 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Modules_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE TimePoints 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_TimePoints_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Tasks 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Tasks_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE SetupModules 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_SetupModules_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE PopQuizzes 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_PopQuizzes_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Courses 
    ADD SetupCourseUpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_SetupCourse_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Trainings 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Trainings_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Banners 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Banners_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Hobbies 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Hobbies_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Rewards 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Rewards_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Assessments 
    ADD UpdatedBy VARCHAR(64) NOT NULL
         CONSTRAINT DF_Assessments_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Guides 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Guides_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE EventCategories 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_EventCategories_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Events 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Events_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE News 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_News_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Roles 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Roles_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE PrivilegePageMappings 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_PrivilegePageMappings_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE PositionCompentencyMappings 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_PositionCompentencyMappings_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE ApprovalMappings 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_ApprovalMappings_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE TrainingServiceLevels 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_TrainingServiceLevels_UpdatedBy DEFAULT 'SYSTEM'
ALTER TABLE Inboxes 
    ADD UpdatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Inboxes_UpdatedBy DEFAULT 'SYSTEM'

--ALTER TABLE UNTUK TAMBAHIN CreatedBy
ALTER TABLE KeyActions 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_KeyActions_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE Competencies 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Competencies_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE Topics 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Topics_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE CompetencyEvaluationMappings 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_CompetencyEvaluationMappings_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE Coaches 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Coaches_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE Modules 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Modules_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE TimePoints 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_TimePoints_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE SetupModules 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_SetupModules_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE PopQuizzes 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_PopQuizzes_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE Courses 
    ADD SetupCourseCreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_SetupCourse_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE EventCategories 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_EventCategories_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE Events 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_Events_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE PrivilegePageMappings 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_PrivilegePageMappings_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE ApprovalMappings 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_ApprovalMappings_CreatedBy DEFAULT 'SYSTEM'
ALTER TABLE TrainingServiceLevels 
    ADD CreatedBy VARCHAR(64) NOT NULL 
        CONSTRAINT DF_TrainingServiceLevels_CreatedBy DEFAULT 'SYSTEM'