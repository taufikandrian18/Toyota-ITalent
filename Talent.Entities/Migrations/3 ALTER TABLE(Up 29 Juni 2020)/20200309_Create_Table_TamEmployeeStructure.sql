USE [ITALENT_UAT]
GO

/** Object:  Table [dbo].[TAMEmployeeStructure]    Script Date: 09/03/2020 16:03:56 **/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TAMEmployeeStructure](
	[ID] [int] NOT NULL,
	[Code] [varchar](250) NOT NULL,
	[NoReg] [varchar](20) NULL,
	[PostCode] [varchar](20) NULL,
	[PostName] [varchar](150) NULL,
	[Staffing] [decimal](38, 2) NULL,
	[State] [varchar](250) NULL,
	[OrgCode] [varchar](20) NULL,
	[ParentOrgCode] [varchar](20) NULL,
	[OrgName] [varchar](150) NULL,
	[JobCode] [varchar](20) NULL,
	[JobName] [varchar](150) NULL,
	[Name] [varchar](250) NULL,
	[EmployeeGroup] [varchar](10) NULL,
	[EmployeeGroupText] [varchar](150) NULL,
	[EmployeeSubgroup] [varchar](50) NULL,
	[EmployeeSubgroupText] [varchar](150) NULL,
	[WorkContract] [varchar](10) NULL,
	[WorkContractText] [varchar](150) NULL,
	[PersonalArea] [varchar](10) NULL,
	[PersonalSubarea] [varchar](10) NULL,
	[DepthLevel] [decimal](38, 0) NULL,
	[Chief] [decimal](38, 0) NULL,
	[OrgLevel] [decimal](38, 0) NULL,
	[Period] [datetime2](7) NULL,
	[Vacant] [decimal](38, 0) NULL,
	[Structure] [varchar](500) NULL,
	[LastChgDateTime] [datetime2](7) NULL,
	[TalentLevel] [int] NOT NULL,
 CONSTRAINT [PK_TAMEmployeeStructure] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO