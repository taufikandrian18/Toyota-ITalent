/** CREATE ITalentReport Table **/

CREATE TABLE ITalentReports(
    ITalentReportId INT
        CONSTRAINT PK_ITalentReportId PRIMARY KEY,
    [Name] VARCHAR(255) NOT NULL,
    [Url] VARCHAR(MAX) NOT NULL
)

/** Seeding Report Table **/

INSERT INTO [Pages](PageId,MenuId,Name,NeedApproval) VALUES 
('RPT','RPRT','Report',0);
