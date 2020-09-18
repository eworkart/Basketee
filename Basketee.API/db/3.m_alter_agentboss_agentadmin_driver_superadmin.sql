ALTER TABLE [dbo].[AgentAdmin] ADD [AccToken] [varchar](64) NULL
GO
ALTER TABLE [dbo].[AgentBoss] ADD [AccToken] [varchar](64) NULL, [AppID] [varchar](255) NULL
GO
ALTER TABLE [dbo].[Driver] ADD 
	[AccToken] [varchar](64) NULL, 
	[AppID] [varchar](255) NULL, 
	[AppToken] [varchar](255) NULL, 
	[LastLogin] [datetime] NULL
GO
ALTER TABLE [dbo].[SuperAdmin] ADD [AccToken] [varchar](64) NULL, [AppID] [varchar](255) NULL
GO
