/*ALTER TABLE EmployeeRewardsMapping to add RedeemedAt Time Stamp*/

ALTER TABLE EmployeeRewardMappings
ADD 
	RedeemedAt DATETIME2(7)
	CONSTRAINT DF_EmployeeRewardMappings_RedeemedAt DEFAULT CURRENT_TIMESTAMP NOT NULL