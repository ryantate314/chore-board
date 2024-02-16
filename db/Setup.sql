USE choreboard
GO

DROP TABLE IF EXISTS app.TaskInstance;
DROP TABLE IF EXISTS app.TaskStatus;
DROP TABLE IF EXISTS app.TaskSchedule;
DROP TABLE IF EXISTS app.TaskDefinition;
DROP TABLE IF EXISTS app.FamilyMember;

DROP SCHEMA IF EXISTS app;

DROP TABLE IF EXISTS auth.[User];

DROP SCHEMA IF EXISTS auth;

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
	Id INT NOT NULL IDENTITY(1, 1)
	, Uuid UNIQUEIDENTIFIER NOT NULL
		CONSTRAINT DF_FamilyMember_FamilyMemberGuid
		DEFAULT (NEWID())
	, FirstName VARCHAR(32) NOT NULL
	, LastName VARCHAR(32) NOT NULL
	, CreatedAt DATETIME NOT NULL
		CONSTRAINT DF_FamilyMember_CreatedAt
		DEFAULT(CURRENT_TIMESTAMP)
	, DeletedAt DATETIME NULL
	, CONSTRAINT PK_FamilyMember
		PRIMARY KEY (Id)
	, CONSTRAINT UQ_FamilyMember_Uuid
		UNIQUE (Uuid)
);

GO

CREATE TABLE app.TaskDefinition (
	Id INT NOT NULL IDENTITY(1, 1)
	, Uuid UNIQUEIDENTIFIER NOT NULL
		CONSTRAINT DF_TaskDefinition_TaskDefinitionGuid
		DEFAULT (NEWID())
	, ShortDescription NVARCHAR(128) NOT NULL
	, Description NVARCHAR(MAX) NULL
	, CreatedAt DATETIME NOT NULL
		CONSTRAINT DF_TaskDefintion_CreatedAt
		DEFAULT (CURRENT_TIMESTAMP)
	, DeletedAt DATETIME NULL
	, CONSTRAINT PK_TaskDefintion
		PRIMARY KEY (Id)
	, CONSTRAINT UQ_TaskDefinition_Uuid
		UNIQUE (Uuid)
);

GO

CREATE TABLE app.TaskSchedule (
	  Id INT NOT NULL IDENTITY(1, 1)
	, StartDate DATETIME NOT NULL
	, EndDate DATETIME NOT NULL
	, RRule VARCHAR(128) NOT NULL
	, TaskDefinitionId INT NOT NULL
	, CreatedAt DATETIME NOT NULL
		CONSTRAINT DF_TaskSchedule_CreatedAt
		DEFAULT (CURRENT_TIMESTAMP)
	, DeletedAt DATETIME NULL
	, CONSTRAINT PK_TaskSchedule
		PRIMARY KEY (Id)
	, CONSTRAINT FK_TaskSchedule_TaskDefinitionId_TaskDefinition
		FOREIGN KEY (TaskDefinitionId)
		REFERENCES app.TaskDefinition (Id)
);

GO

CREATE TABLE app.TaskInstance (
	Id INT NOT NULL IDENTITY(1, 1)
	, Uuid UNIQUEIDENTIFIER NOT NULL
		CONSTRAINT DF_TaskInstance_Uuid
		DEFAULT (NEWID())
	, TaskDefinitionId INT NOT NULL
	, InstanceDate DATETIME NOT NULL
	, [Status] CHAR(1) NULL
	, CompletedAt DATETIME NULL
	, CONSTRAINT PK_TaskInstance
		PRIMARY KEY (Id)
	, CONSTRAINT FK_TaskInstance_TaskDefinitionId_TaskDefinition
		FOREIGN KEY (TaskDefinitionId)
		REFERENCES app.TaskDefinition (Id)
	, CONSTRAINT UQ_TaskInstance_Uuid
		UNIQUE (Uuid)
	, CONSTRAINT FK_TaskInstance_TaskStatus
		FOREIGN KEY ([Status])
		REFERENCES app.TaskStatus (StatusCode)
);

GO

CREATE TABLE app.TaskStatusHistory (
	Id INT NOT NULL IDENTITY(1, 1)
	, TaskInstanceId INT NOT NULL
	, ModifiedAt DATETIME NOT NULL
		CONSTRAINT DF_TaskStatusHistory_ModifiedAt
		DEFAULT (CURRENT_TIMESTAMP)
	, FamilyMemberId INT NULL
	, CONSTRAINT PK_TaskStatusHistory
		PRIMARY KEY (Id)
	, CONSTRAINT FK_TaskStatusHistory_TaskInstanceId
		FOREIGN KEY (TaskInstanceId)
		REFERENCES app.TaskInstance (ID)
	, CONSTRAINT FK_TaskStatusHistory_FamilyMemberId
		FOREIGN KEY (FamilyMemberId)
		REFERENCES app.FamilyMember (Id)
)

GO

CREATE SCHEMA auth;

GO

CREATE TABLE auth.[User] (
	Id INT NOT NULL
	, Uuid UNIQUEIDENTIFIER NOT NULL
		CONSTRAINT DF_User_Uuid
		DEFAULT (NEWID())
	, Email VARCHAR(128) NOT NULL
	, PasswordHash CHAR(128) NULL
	, CreatedAt DATETIME NOT NULL
		CONSTRAINT DF_User_CreatedAt
		DEFAULT (CURRENT_TIMESTAMP)
	, DeletedAt DATETIME NULL
	, CONSTRAINT UQ_User_Uuid
		UNIQUE (Uuid)
)

GO

CREATE OR ALTER PROCEDURE app.usp_GetTasks (
	@startDate DATETIME
	, @endDate DATETIME
)
AS
BEGIN

	SET NOCOUNT ON;

	-- Schedules active during the provided dates
	SELECT
		  TS.StartDate
		, TS.EndDate
		, TS.RRule
		, TD.Uuid
	FROM app.TaskSchedule TS
		JOIN app.TaskDefinition TD
			ON TS.TaskDefinitionId = TD.Id
	WHERE TS.StartDate <= @endDate
		AND TS.EndDate >= @startDate
		AND TD.DeletedAt IS NULL;

	-- Concrete Task Instances during the provided dates
	SELECT
		TI.Uuid
		, TI.InstanceDate
		, TD.Uuid AS TaskDefinitionUuid
	FROM app.TaskInstance TI
		JOIN app.TaskDefinition TD
			ON TI.TaskDefinitionId = TD.Id
	WHERE TI.InstanceDate >= @startDate AND TI.InstanceDate < @endDate;

	-- Last Task for Each Definition
	SELECT
		TI.Uuid
		, TD.ShortDescription
		, TD.Description
		, TD.Uuid AS TaskDefinitionId
	FROM app.TaskInstance TI
		JOIN app.TaskDefinition TD
			ON TI.TaskDefinitionId = TD.Id
	WHERE TI.InstanceDate < @endDate
		AND TI.TaskStatus != 'D' -- Deleted
		AND ROW_NUMBER() OVER (PARTITION BY TI.TaskDefinitionId ORDER BY InstanceDate DESC) = 1;


END

GO