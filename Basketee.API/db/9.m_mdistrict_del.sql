ALTER TABLE [dbo].[ConsumerAddress]  DROP  CONSTRAINT [FK_ConsumerAddress_MDistrict]
GO

ALTER TABLE [dbo].[ConsumerAddress]  DROP  COLUMN [DistID]
GO

DROP TABLE [dbo].[MDistrict] 
GO

ALTER TABLE [dbo].[ConsumerAddress] ADD [RegionName] [varchar](255) NULL
GO

ALTER TABLE [dbo].[Invoice]  DROP  CONSTRAINT [FK_Invoice_Order]
GO

DROP TABLE [dbo].[Invoice]
GO