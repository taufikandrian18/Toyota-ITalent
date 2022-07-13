/** CREATE ApprovalLevels Table and ALTER ApprovalMappings **/

CREATE TABLE ApprovalLevels(
ApprovalLevelId INT NOT NULL
CONSTRAINT PK_ApprovalLevels PRIMARY KEY,
[Name] VARCHAR(255) NOT NULL,
[Description] VARCHAR(255) NOT NULL,
)

GO

INSERT INTO ApprovalLevels
VALUES
(1,'Section Head','Level 1 - Content yang membutuhkan Approval hingga 1 tingkatan.'),
(2,'Department Head','Level 2 - Content yang membutuhkan Approval hingga 2 tingkatan. Posisi yang termasuk Level ini adalah Department Head, PJS Department Manager'),
(3,'Division Head','Level 3 - Content yang membutuhkan Approval hingga 3 tingkatan. Posisi yang termasuk Level ini adalah Division Head, PJS Division Head, Executive General Manager, PJS General Manager')

GO

ALTER TABLE ApprovalMappings
DROP CONSTRAINT FK_ApprovalMappings_Positions;

ALTER TABLE ApprovalMappings
DROP COLUMN PositionId

GO

ALTER TABLE ApprovalMappings
DROP COLUMN ApprovalLevel;

ALTER TABLE ApprovalMappings
ADD
ApprovalLevelId INT,
CONSTRAINT FK_ApprovalMappings_ApprovalLevels
FOREIGN KEY (ApprovalLevelId) REFERENCES ApprovalLevels(ApprovalLevelId);

ALTER TABLE Approvals
ADD ApprovalTo varchar(64)

ALTER TABLE Approvals ADD CONSTRAINT FK_Approvals_ApprovalTo 
FOREIGN KEY (ApprovalTo) REFERENCES Employees(EmployeeId) 
GO