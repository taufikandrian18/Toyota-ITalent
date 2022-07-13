--DELETE FROM StagingDealerEmployee
--DELETE FROM StagingTAMEmployee
--DELETE FROM StagingOutlet
--DELETE FROM StagingRegion
--DELETE FROM StagingSalesArea
--DELETE FROM StagingAfterSalesArea
--DELETE FROM StagingDealerCompany
--DELETE FROM StagingDealerGroup
--DELETE FROM StagingManpowerPositionType

ALTER TABLE StagingDealerEmployee
ADD ID INT NOT NULL

ALTER TABLE StagingTAMEmployee
ADD ID INT NOT NULL

ALTER TABLE StagingOutlet
ADD ID INT NOT NULL

ALTER TABLE StagingRegion
ADD ID INT NOT NULL

ALTER TABLE StagingSalesArea
ADD ID INT NOT NULL

ALTER TABLE StagingAfterSalesArea
ADD ID INT NOT NULL

ALTER TABLE StagingDealerCompany
ADD ID INT NOT NULL

ALTER TABLE StagingDealerGroup
ADD ID INT NOT NULL

ALTER TABLE StagingManpowerPositionType
ADD ID INT NOT NULL

ALTER TABLE StagingRegion
ADD RegionTypeId VARCHAR(1) NOT NULL