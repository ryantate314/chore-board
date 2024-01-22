USE choreboard
GO

DROP TABLE IF EXISTS app.TaskInstance;
DROP TABLE IF EXISTS app.TaskStatus;
DROP TABLE IF EXISTS app.TaskSchedule;
DROP TABLE IF EXISTS app.TaskDefinition;
DROP TABLE IF EXISTS app.FamilyMember;

DROP SCHEMA IF EXISTS app;

GO

CREATE SCHEMA app;

GO

CREATE TABLE app.TaskStatus (
	StatusCode CHAR(1) NOT NULL
	, Description VARCHAR(32) NOT NULL
	, CONSTRAINT PK_TaskStatus
		PRIMARY KEY (StatusCode)
);

GO

INSERT INTO app.TaskStatus
VALUES ('U', 'Upcoming')
, ('T', 'ToDo')
, ('I', 'InProgress')
, ('C', 'Complete')
, ('D', 'Deleted')

GO

CREATE TABLE app.FamilyMember (
	FamilyMemberId INT NOT NULL IDENTITY(1, 1)
	, FamilyMemberGuid UNIQUEIDENTIFIER NOT NULL
		CONSTRAINT DF_FamilyMember_FamilyMemberGuid
		DEFAULT (NEWID())
	, FirstName VARCHAR(32) NOT NULL
	, LastName VARCHAR(32) NOT NULL
	, CreatedAt DATETIME NOT NULL
		CONSTRAINT DF_FamilyMember_CreatedAt
		DEFAULT(CURRENT_TIMESTAMP)
	, DeletedAt DATETIME NULL
	, CONSTRAINT PK_FamilyMember
		PRIMARY KEY (FamilyMemberId)
);

GO

CREATE TABLE app.TaskDefinition (
	TaskDefinitionId INT NOT NULL IDENTITY(1, 1)
	, TaskDefinitionGuid UNIQUEIDENTIFIER NOT NULL
		CONSTRAINT DF_TaskDefinition_TaskDefinitionGuid
		DEFAULT (NEWID())
	, ShortDescription NVARCHAR(128) NOT NULL
	, Description NVARCHAR(MAX) NULL
	, CreatedAt DATETIME NOT NULL
		CONSTRAINT DF_TaskDefintion_CreatedAt
		DEFAULT (CURRENT_TIMESTAMP)
	, CreatedBy INT NOT NULL
	, DeletedAt DATETIME NULL
	, CONSTRAINT PK_TaskDefintion
		PRIMARY KEY (TaskDefinitionId)
	, CONSTRAINT FK_TaskDefinition_CreatedBy_FamilyMember
		FOREIGN KEY (CreatedBy)
		REFERENCES app.FamilyMember (FamilyMemberId)
);

GO

CREATE TABLE app.TaskSchedule (
	  TaskScheduleId INT NOT NULL IDENTITY(1, 1)
	, StartDate DATETIME NOT NULL
	, EndDate DATETIME NOT NULL
	, RRule VARCHAR(128) NOT NULL
	, TaskDefinitionId INT NOT NULL
	, CreatedAt DATETIME NOT NULL
		CONSTRAINT DF_TaskSchedule_CreatedAt
		DEFAULT (CURRENT_TIMESTAMP)
	, DeletedAt DATETIME NULL
	, CONSTRAINT PK_TaskSchedule
		PRIMARY KEY (TaskScheduleId)
	, CONSTRAINT FK_TaskSchedule_TaskDefinitionId_TaskDefinition
		FOREIGN KEY (TaskDefinitionId)
		REFERENCES app.TaskDefinition (TaskDefinitionId)
);

GO

CREATE TABLE app.TaskInstance (
	TaskInstanceId INT NOT NULL IDENTITY(1, 1)
	, TaskInstanceGuid UNIQUEIDENTIFIER NOT NULL
		CONSTRAINT DF_TaskInstance_TaskInstanceGuid
		DEFAULT (NEWID())
	, TaskDefinitionId INT NOT NULL
	, InstanceDate DATETIME NOT NULL
	, CONSTRAINT PK_TaskInstance
		PRIMARY KEY (TaskInstanceId)
	, CONSTRAINT FK_TaskInstance_TaskDefinitionId_TaskDefinition
		FOREIGN KEY (TaskDefinitionId)
		REFERENCES app.TaskDefinition (TaskDefinitionId)
);

GO

