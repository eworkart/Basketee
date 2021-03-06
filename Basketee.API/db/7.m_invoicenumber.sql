﻿SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[InvoiceNumbers](
    [InvnrID] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceNumber] [int] NOT NULL,
    [AgenID] INT NOT NULL,
 CONSTRAINT [PK_InvoiceNumber] PRIMARY KEY CLUSTERED
(
    [InvnrID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[InvoiceNumbers]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceNumbers_Agency] FOREIGN KEY([AgenID])
REFERENCES [dbo].[Agency] ([AgenID])
GO
