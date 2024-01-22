BEGIN TRY

BEGIN TRAN;

-- CreateTable
CREATE TABLE [dbo].[Home] (
    [id] INT NOT NULL IDENTITY(1,1),
    [uuid] NVARCHAR(1000) NOT NULL,
    [name] NVARCHAR(1000) NOT NULL,
    CONSTRAINT [Home_pkey] PRIMARY KEY CLUSTERED ([id]),
    CONSTRAINT [Home_uuid_key] UNIQUE NONCLUSTERED ([uuid])
);

-- CreateTable
CREATE TABLE [dbo].[Member] (
    [id] INT NOT NULL IDENTITY(1,1),
    [uuid] NVARCHAR(1000) NOT NULL,
    [homeId] INT NOT NULL,
    CONSTRAINT [Member_pkey] PRIMARY KEY CLUSTERED ([id]),
    CONSTRAINT [Member_uuid_key] UNIQUE NONCLUSTERED ([uuid])
);

-- CreateTable
CREATE TABLE [dbo].[TaskProfile] (
    [id] INT NOT NULL,
    [description] NVARCHAR(1000) NOT NULL,
    CONSTRAINT [TaskProfile_pkey] PRIMARY KEY CLUSTERED ([id])
);

-- CreateTable
CREATE TABLE [dbo].[TaskDefinition] (
    [id] INT NOT NULL IDENTITY(1,1),
    [uuid] NVARCHAR(1000) NOT NULL,
    [shortDescription] NVARCHAR(1000) NOT NULL,
    [description] NVARCHAR(1000),
    [taskProfileId] INT NOT NULL,
    CONSTRAINT [TaskDefinition_pkey] PRIMARY KEY CLUSTERED ([id]),
    CONSTRAINT [TaskDefinition_uuid_key] UNIQUE NONCLUSTERED ([uuid])
);

-- CreateTable
CREATE TABLE [dbo].[FrequencyType] (
    [id] INT NOT NULL,
    [name] NVARCHAR(1000) NOT NULL,
    CONSTRAINT [FrequencyType_pkey] PRIMARY KEY CLUSTERED ([id])
);

-- CreateTable
CREATE TABLE [dbo].[TaskSchedule] (
    [id] INT NOT NULL IDENTITY(1,1),
    [startDate] DATETIME2 NOT NULL,
    [endDate] DATETIME2 NOT NULL,
    [rrule] NVARCHAR(1000) NOT NULL,
    [taskDefinitionId] INT NOT NULL,
    [frequencyTypeId] INT NOT NULL,
    CONSTRAINT [TaskSchedule_pkey] PRIMARY KEY CLUSTERED ([id])
);

-- CreateTable
CREATE TABLE [dbo].[TaskInstance] (
    [id] INT NOT NULL IDENTITY(1,1),
    [uuid] NVARCHAR(1000) NOT NULL,
    [taskDefinitionId] INT NOT NULL,
    [createdAt] DATETIME2 NOT NULL,
    [createdById] INT,
    CONSTRAINT [TaskInstance_pkey] PRIMARY KEY CLUSTERED ([id]),
    CONSTRAINT [TaskInstance_uuid_key] UNIQUE NONCLUSTERED ([uuid])
);

-- CreateTable
CREATE TABLE [dbo].[TaskStatus] (
    [id] INT NOT NULL,
    [name] NVARCHAR(1000) NOT NULL,
    CONSTRAINT [TaskStatus_pkey] PRIMARY KEY CLUSTERED ([id])
);

-- CreateTable
CREATE TABLE [dbo].[TaskHistory] (
    [id] INT NOT NULL IDENTITY(1,1),
    [taskInstanceId] INT NOT NULL,
    [description] NVARCHAR(1000) NOT NULL,
    CONSTRAINT [TaskHistory_pkey] PRIMARY KEY CLUSTERED ([id])
);

-- AddForeignKey
ALTER TABLE [dbo].[Member] ADD CONSTRAINT [Member_homeId_fkey] FOREIGN KEY ([homeId]) REFERENCES [dbo].[Home]([id]) ON DELETE NO ACTION ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE [dbo].[TaskDefinition] ADD CONSTRAINT [TaskDefinition_taskProfileId_fkey] FOREIGN KEY ([taskProfileId]) REFERENCES [dbo].[TaskProfile]([id]) ON DELETE NO ACTION ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE [dbo].[TaskSchedule] ADD CONSTRAINT [TaskSchedule_taskDefinitionId_fkey] FOREIGN KEY ([taskDefinitionId]) REFERENCES [dbo].[TaskDefinition]([id]) ON DELETE NO ACTION ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE [dbo].[TaskSchedule] ADD CONSTRAINT [TaskSchedule_frequencyTypeId_fkey] FOREIGN KEY ([frequencyTypeId]) REFERENCES [dbo].[FrequencyType]([id]) ON DELETE NO ACTION ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE [dbo].[TaskInstance] ADD CONSTRAINT [TaskInstance_taskDefinitionId_fkey] FOREIGN KEY ([taskDefinitionId]) REFERENCES [dbo].[TaskDefinition]([id]) ON DELETE NO ACTION ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE [dbo].[TaskInstance] ADD CONSTRAINT [TaskInstance_createdById_fkey] FOREIGN KEY ([createdById]) REFERENCES [dbo].[Member]([id]) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE [dbo].[TaskHistory] ADD CONSTRAINT [TaskHistory_taskInstanceId_fkey] FOREIGN KEY ([taskInstanceId]) REFERENCES [dbo].[TaskInstance]([id]) ON DELETE NO ACTION ON UPDATE CASCADE;

COMMIT TRAN;

END TRY
BEGIN CATCH

IF @@TRANCOUNT > 0
BEGIN
    ROLLBACK TRAN;
END;
THROW

END CATCH
