ALTER TABLE Positions
DROP CONSTRAINT FK_Positions_DealerPeopleCategory

ALTER TABLE Positions
DROP COLUMN DealerPeopleCategoryId

ALTER TABLE Roles
DROP CONSTRAINT FK_Roles_DealerPeopleCategory

ALTER TABLE Roles
DROP COLUMN DealerPeopleCategoryId

DROP TABLE DealerPeopleCategories

CREATE TABLE DealerPeopleCategories (
	DealerPeopleCategoryCode VARCHAR(250)
		CONSTRAINT PK_DealerPeopleCategories PRIMARY KEY,
	Name VARCHAR(250) NOT NULL
)

ALTER TABLE Roles
ADD DealerPeopleCategoryCode VARCHAR(250) NULL
	CONSTRAINT FK_Roles_DealerPeopleCategories FOREIGN KEY
		REFERENCES DealerPeopleCategories(DealerPeopleCategoryCode)

ALTER TABLE Employees
ADD DealerPeopleCategoryCode VARCHAR(250) NULL
	CONSTRAINT FK_Employees_DealerPeopleCategories FOREIGN KEY
		REFERENCES DealerPeopleCategories(DealerPeopleCategoryCode)