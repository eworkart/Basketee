ALTER TABLE [dbo].[TeleOrder] ALTER COLUMN  [DrvrID] [int] NULL
GO

ALTER TABLE [dbo].[Reminder] ADD [ProdID] INT NULL
GO

ALTER TABLE [dbo].[Reminder]  WITH CHECK ADD  CONSTRAINT [FK_Reminder_Product] FOREIGN KEY([ProdID])
REFERENCES [dbo].[Product] ([ProdID])
GO

ALTER TABLE [dbo].[TeleOrderPrdocuctExchange] DROP CONSTRAINT [FK_TeleOrderPrdocuctExchange_Order]
GO

ALTER TABLE [dbo].[TeleOrderPrdocuctExchange] DROP COLUMN [OrdrID]
GO

ALTER TABLE [dbo].[TeleOrderPrdocuctExchange] ADD [TeleOrdID] INT NOT NULL
GO

ALTER TABLE [dbo].[TeleOrderPrdocuctExchange] ADD CONSTRAINT [FK_TeleOrderPrdocuctExchange_TeleOrder] FOREIGN KEY ([TeleOrdID]) REFERENCES [dbo].[TeleOrder] ([TeleOrdID])
GO