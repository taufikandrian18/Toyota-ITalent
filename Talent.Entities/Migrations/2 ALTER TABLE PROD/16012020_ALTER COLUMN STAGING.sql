--DELETE FROM StagingDealerEmployee
--DELETE FROM StagingTAMEmployee
--DELETE FROM StagingOutlet
--DELETE FROM StagingRegion
--DELETE FROM StagingSalesArea
--DELETE FROM StagingAfterSalesArea
--DELETE FROM StagingDealerCompany
--DELETE FROM StagingDealerGroup
--DELETE FROM StagingManpowerPositionType

ALTER TABLE StagingManpowerPositionType
ADD State VARCHAR(250) NOT NULL

ALTER TABLE StagingOutlet
ADD State VARCHAR(250) NOT NULL

ALTER TABLE StagingRegion
ADD State VARCHAR(250) NOT NULL

ALTER TABLE StagingSalesArea
ADD State VARCHAR(250) NOT NULL

ALTER TABLE StagingAfterSalesArea
ADD State VARCHAR(250) NOT NULL

ALTER TABLE StagingDealerGroup
ADD State VARCHAR(250) NOT NULL

ALTER TABLE StagingDealerCompany
ADD State VARCHAR(250) NOT NULL