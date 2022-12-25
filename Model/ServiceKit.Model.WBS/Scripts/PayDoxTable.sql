DROP TABLE [dbo].[WBS_SyncRequestItems]
GO
DROP TABLE [dbo].[WBS_SyncRequests]
GO
CREATE TABLE [dbo].[WBS_SyncRequests](
	[RequestId] [uniqueidentifier] NOT NULL,

	[ProjectCode] [nvarchar](100) NULL,
	[ProjectName] [nvarchar](512) NULL,

	[Status] [int] NOT NULL,
	[ErrorMessage] [nvarchar](1000) NULL,
	[Created] [datetime] NULL,
	CONSTRAINT [PK_WBS_SyncProjects] PRIMARY KEY CLUSTERED ([RequestId] ASC)
 )
GO

CREATE TABLE [dbo].[WBS_SyncRequestItems](
	[RequestId] [uniqueidentifier] NOT NULL,

	[Id] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[Level] [int] NOT NULL,
	[ShortCode] [nvarchar](100) NULL,
	[FullCode] [nvarchar](512) NULL,
	[ShortName] [nvarchar](512) NULL,
	[FullName] [nvarchar](1000) NULL,
	[Comment] [nvarchar](1000) NULL,

	[Status] [int] NOT NULL,
	[ErrorMessage] [nvarchar](1000) NULL,
	[Created] [datetime] NULL,
    CONSTRAINT [PK_WBS_SyncProjectItems] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

ALTER TABLE [dbo].[WBS_SyncRequestItems]  WITH CHECK ADD  CONSTRAINT [FK_WBS_SyncRequestItems_WBS_SyncRequests_RequestId] FOREIGN KEY([RequestId])
REFERENCES [dbo].[WBS_SyncRequests] ([RequestId])
GO