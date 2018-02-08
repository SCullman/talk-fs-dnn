CREATE TABLE [SummitTasks](
	[Id] [uniqueidentifier] NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Task] [nvarchar](50) NOT NULL,
	[Complete] [bit] NOT NULL,
  [CreatedByUserId] [int] NOT NULL,
	[CreatedOnDate] [datetime] NOT NULL,
	[LastModifiedByUserId] [int] NOT NULL,
	[LastModifiedOnDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Summit_Tasks] PRIMARY KEY CLUSTERED 
  (
    [Id] ASC
  )
  GO

ALTER TABLE [dbo].[SummitTasks] ADD  CONSTRAINT [DF_SummitTasks_Complete]  DEFAULT ((0)) FOR [Complete]
GO