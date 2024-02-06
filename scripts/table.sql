CREATE TABLE Status(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL
)

CREATE TABLE SchedulingType(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(200) NOT NULL
)

CREATE TABLE Job(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL,
    MaximumRetries INT,
    CurrentlyRetries INT,
    StatusId INT NOT NULL REFERENCES Status(Id),
    SchedulingTypeId INT NOT NULL REFERENCES SchedulingType(Id),
    InProgress BIT,
    ExecutedBy VARCHAR(50) NULL,
    CreationDate DATETIME DEFAULT GETDATE(),
    LastUpdate DATETIME DEFAULT GETDATE(),
    LastExecution DATETIME,
    CronExpression VARCHAR(50) NULL,
    IntervalInMinutes INT NULL
)

CREATE TABLE JobHistoryStatus(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL
)

CREATE TABLE JobHistory(
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Message VARCHAR(100) NULL,
    JobStatus INT NOT NULL REFERENCES JobHistoryStatus(Id),
    JobId INT NOT NULL REFERENCES Job(Id),
    HistoryDate DATETIME DEFAULT GETDATE(),
    Success BIT NOT NULL,
    StackTrace VARCHAR(MAX) NULL,
    ExceptionMessage VARCHAR(200) NULL
)