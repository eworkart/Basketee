SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TeleOrderPrdocuctExchange](
    [TelOrdPrdExID] [int] IDENTITY(1,1) NOT NULL,
    [OrdrID] [int] NOT NULL,
    [ProdID] [int] NOT NULL,
    [ExchangeWith] [varchar](150) NOT NULL,
    [ExchangeQuantity] [int] NOT NULL,
    [ExchangePrice] [numeric](18, 3) NOT NULL,
    [ExchangePromoPrice] [numeric](18, 3) NOT NULL,
    [StatusId] [bit] NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TeleOrderPrdocuctExchange] PRIMARY KEY CLUSTERED
(
    [TelOrdPrdExID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[TeleOrderPrdocuctExchange]  WITH CHECK ADD  CONSTRAINT [FK_TeleOrderPrdocuctExchange_Order] FOREIGN KEY([OrdrID])
REFERENCES [dbo].[Order] ([OrdrID])
GO

ALTER TABLE [dbo].[TeleOrderPrdocuctExchange] CHECK CONSTRAINT [FK_TeleOrderPrdocuctExchange_Order]
GO

ALTER TABLE [dbo].[TeleOrderPrdocuctExchange]  WITH CHECK ADD  CONSTRAINT [FK_TeleOrderPrdocuctExchange_Product] FOREIGN KEY([ProdID])
REFERENCES [dbo].[Product] ([ProdID])
GO

ALTER TABLE [dbo].[TeleOrderPrdocuctExchange] CHECK CONSTRAINT [FK_TeleOrderPrdocuctExchange_Product]
GO

ALTER TABLE [dbo].[TeleOrderPrdocuctExchange] ADD  CONSTRAINT [DF_TeleOrderPrdocuctExchange_ExchangePrice]  DEFAULT ((0)) FOR [ExchangePrice]
GO

ALTER TABLE [dbo].[TeleOrderPrdocuctExchange] ADD  CONSTRAINT [DF_TeleOrderPrdocuctExchange_ExchangePromoPrice]  DEFAULT ((0)) FOR [ExchangePromoPrice]
GO

ALTER TABLE [dbo].[TeleOrderPrdocuctExchange] ADD  CONSTRAINT [DF_TeleOrderPrdocuctExchange_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO

ALTER TABLE [dbo].[TeleOrderPrdocuctExchange] ADD  CONSTRAINT [DF_TeleOrderPrdocuctExchange_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
