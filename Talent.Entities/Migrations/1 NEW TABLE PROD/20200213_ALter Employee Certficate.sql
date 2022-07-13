ALTER TABLE EmployeeCertificates
ADD CourseId INT NULL
	CONSTRAINT FK_EmployeeCertificates_CourseId FOREIGN KEY REFERENCES Courses(CourseId)


ALTER TABLE EmployeeCertificates
ADD CertificateNumber VARCHAR(255) NULL