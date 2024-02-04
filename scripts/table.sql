CREATE TABLE Status(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR NOT NULL
)

CREATE TABLE SchedulingType(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR NOT NULL,
    Description VARCHAR NOT NULL
)

CREATE TABLE Job(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR NOT NULL,
    MaximumRetries INT,
    CurrentlyRetries INT,
    StatusId INT NOT NULL REFERENCES Status(Id),
    SchedulingTypeId INT NOT NULL REFERENCES SchedulingType(Id),
    InProgress BIT,
    ExecutedBy VARCHAR NULL,
    CreationDate DATETIME DEFAULT GETDATE(),
    LastExecution DATETIME,
    CronExpression VARCHAR NULL,
    IntervalInMinutes INT NULL
)