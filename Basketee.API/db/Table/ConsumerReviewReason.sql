
CREATE TABLE [dbo].[ConsumerReviewReason](
	[ReasonID] [int] IDENTITY(1,1) NOT NULL,
	[ReasonText] [varchar](max) NULL,
 CONSTRAINT [PK_ConsumerReviewReason] PRIMARY KEY CLUSTERED 
(
	[ReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO




