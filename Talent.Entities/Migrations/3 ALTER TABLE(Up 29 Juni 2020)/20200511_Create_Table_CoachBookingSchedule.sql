/**CREATE TABLE CoachBookingSchedules**/

CREATE TABLE CoachBookingSchedules(
    CoachBookingScheduleId INT 
        CONSTRAINT PK_CoachBookingSchedules PRIMARY KEY IDENTITY,
	CoachScheduleId INT NOT NULL
        CONSTRAINT FK_CoachBookingSchedules_CoachSchedules FOREIGN KEY REFERENCES CoachSchedules(CoachScheduleId) ON UPDATE CASCADE ON DELETE CASCADE,
    EmployeeId varchar(64) NOT NULL
        CONSTRAINT FK_CoachBookingSchedules_Employees FOREIGN KEY REFERENCES Employees(EmployeeId),
	CreatedAt DATETIME2 (7) NOT NULL DEFAULT CURRENT_TIMESTAMP,
)