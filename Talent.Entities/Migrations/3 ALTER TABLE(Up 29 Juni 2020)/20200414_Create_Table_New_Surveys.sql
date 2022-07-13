
CREATE TABLE SurveyMatrixChoices 
(
   SurveyMatrixChoiceId INT 
       CONSTRAINT PK_SurveyMatrixChoices PRIMARY KEY IDENTITY,
   SurveyQuestionId INT 
       CONSTRAINT FK_SurveyMatrixChoices_SurveyQuestions FOREIGN KEY REFERENCES SurveyQuestions(SurveyQuestionId),
   [Text] VARCHAR(64) NOT NULL
)

GO

CREATE TABLE SurveyMatrixQuestions 
(
   SurveyMatrixQuestionId INT 
       CONSTRAINT PK_SurveyMatrixQuestions PRIMARY KEY IDENTITY,
   SurveyQuestionId INT 
       CONSTRAINT FK_SurveyMatrixQuestions_SurveyQuestions FOREIGN KEY REFERENCES SurveyQuestions(SurveyQuestionId),
   Number INT,
   Question VARCHAR(64) NOT NULL
)

GO

CREATE TABLE SurveyMatchingChoices
(
   SurveyMatchingChoiceId INT
       CONSTRAINT PK_SurveyMatchingChoices PRIMARY KEY IDENTITY,

   SurveyQuestionId INT NOT NULL 
       CONSTRAINT FK_SurveyMatchingChoices_SurveyQuestions FOREIGN KEY REFERENCES SurveyQuestions(SurveyQuestionId) 
           ON UPDATE CASCADE ON DELETE CASCADE,
   BlobId UNIQUEIDENTIFIER NULL 
       CONSTRAINT FK_SurveyMatchingChoices_Blobs FOREIGN KEY REFERENCES Blobs(BlobId) 
           ON UPDATE NO ACTION ON DELETE NO ACTION,

   SurveyMatchingChoiceCode CHAR(3) NOT NULL,
   [Text] VARCHAR(64) NULL
)
GO