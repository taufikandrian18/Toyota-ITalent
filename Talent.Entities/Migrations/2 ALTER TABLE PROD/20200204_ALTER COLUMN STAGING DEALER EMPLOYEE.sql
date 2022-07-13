
--StagingActualOrganizationStructure

--StagingAfterSalesArea
ALTER TABLE [dbo].[StagingAfterSalesArea]
ALTER COLUMN [Name] [varchar](250) NULL

--StaingDealerCompany 
ALTER TABLE [dbo].[StagingDealerCompany]
ALTER COLUMN [Name] [varchar](250) NULL

ALTER TABLE [dbo].[StagingDealerCompany]
ALTER COLUMN [DealerGroupId] [decimal](38, 0) NULL

--StagingDealerEmployee
ALTER TABLE [dbo].[StagingDealerEmployee] 
ALTER COLUMN [Name] [varchar](250) NULL

ALTER TABLE [dbo].[StagingDealerEmployee] 
ALTER COLUMN [OutletId] [decimal](38, 0) NULL

ALTER TABLE [dbo].[StagingDealerEmployee] 
ALTER COLUMN [LastManpowerPositionTypeId] [varchar](10) NULL

ALTER TABLE [dbo].[StagingDealerEmployee] 
ALTER COLUMN [State] [varchar](250) NULL

ALTER TABLE [dbo].[StagingDealerEmployee] 
ALTER COLUMN [EmployeeId] [varchar](50) NULL

--StagingDealerGroup
ALTER TABLE [dbo].[StagingDealerGroup]
ALTER COLUMN [DealerGroupCode] [varchar](3) NULL

ALTER TABLE [dbo].[StagingDealerGroup]
ALTER COLUMN [Name] [varchar](250) NULL

-- Staging Man power position type:

ALTER TABLE [dbo].[StagingManpowerPositionType]
ALTER COLUMN [Name] [varchar](250) NULL

--Staging Outlet: 

ALTER TABLE [dbo].[StagingOutlet]
ALTER COLUMN [Name] [varchar](250) NULL

ALTER TABLE [dbo].[StagingOutlet]
ALTER COLUMN [Name] [varchar](250) NULL

ALTER TABLE [dbo].[StagingOutlet]
ALTER COLUMN[DealerCompanyId] [decimal](38, 0) NULL

ALTER TABLE [dbo].[StagingOutlet]
ALTER COLUMN[OutletFunctionId] [varchar](5) NULL

ALTER TABLE [dbo].[StagingOutlet]
ALTER COLUMN [OutletCode] [varchar](10) NULL

ALTER TABLE [dbo].[StagingOutlet]
ALTER COLUMN [KabupatenId] [decimal](38, 0) NULL

ALTER TABLE [dbo].[StagingOutlet]
ALTER COLUMN[TamAreaId] [decimal](38, 0) NULL

--Staging Region:

ALTER TABLE [dbo].[StagingRegion]
ALTER COLUMN [Name] [varchar](250) NULL


--Staging Sales  Area:

ALTER TABLE [dbo].[StagingSalesArea]
ALTER COLUMN [Name] [varchar](250) NULL


--Staging User:

ALTER TABLE [dbo].[StagingUser]
ALTER COLUMN [Name] [varchar](250) NULL

ALTER TABLE [dbo].[StagingUser]
ALTER COLUMN [NoReg] [varchar](250) NULL


---BSYS
--StagingDealerEmployee
alter table [dbo].[StagingDealerEmployee]
add Sex varchar(1) NULL

alter table [dbo].[StagingDealerEmployee]
add KTP varchar(64) NULL

alter table [dbo].[StagingDealerEmployee]
add DateOfBirth datetime2(3) NULL

--Employees
alter table Employees
add KTP varchar(64) null