SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Invoice](
    [InvoID] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceNumber] [varchar](30) NULL,
    [OrdrID] [int] NOT NULL,
    [StatusId] [smallint] NOT NULL,
    [InvoiceDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED
(
    [InvoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Order] FOREIGN KEY([OrdrID])
REFERENCES [dbo].[Order] ([OrdrID])
GO

ALTER TABLE [dbo].[Invoice] ADD  CONSTRAINT [DF_Invoice_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO

ALTER TABLE [dbo].[Invoice] ADD  CONSTRAINT [DF_Invoice_InvoiceDate]  DEFAULT (getdate()) FOR [InvoiceDate]
GO
