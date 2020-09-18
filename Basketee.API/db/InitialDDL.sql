--USE [LpgDb]
--GO
/****** Object:  Table [dbo].[ActivityLog]    Script Date: 30-08-2017 16:09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ActivityLog](
	[ActvID] [int] IDENTITY(1,1) NOT NULL,
	[Item] [varchar](50) NOT NULL,
	[Activity] [varchar](50) NOT NULL,
	[SAdminID] [smallint] NOT NULL,
	[ActDate] [date] NOT NULL,
	[ActTime] [time](7) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ActivityLog] PRIMARY KEY CLUSTERED 
(
	[ActvID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Agency]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Agency](
	[AgenID] [int] IDENTITY(1,1) NOT NULL,
	[SoldToNumber] [varchar](50) NOT NULL,
	[AgencyName] [varchar](100) NOT NULL,
	[Region] [varchar](100) NOT NULL,
	[StatusId] [bit] NOT NULL,
	[CreatedBy] [smallint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [smallint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Agency] PRIMARY KEY CLUSTERED 
(
	[AgenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AgentAdmin]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AgentAdmin](
	[AgadmID] [int] IDENTITY(1,1) NOT NULL,
	[AgentAdminName] [varchar](50) NOT NULL,
	[AgenID] [int] NOT NULL,
	[DbptID] [int] NOT NULL,
	[MobileNumber] [varchar](20) NOT NULL,
	[ProfileImage] [varchar](50) NULL,
	[AppToken] [varchar](max) NULL,
	[LastLogin] [datetime] NULL,
	[Password] [varchar](25) NOT NULL,
	[StatusId] [bit] NOT NULL,
	[CreatedBy] [smallint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [smallint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[AgadmID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AgentBoss]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AgentBoss](
	[AbosID] [int] IDENTITY(1,1) NOT NULL,
	[OwnerName] [varchar](100) NOT NULL,
	[AgenID] [int] NOT NULL,
	[MobileNumber] [varchar](20) NOT NULL,
	[ProfileImage] [varchar](50) NULL,
	[AppToken] [varchar](max) NULL,
	[Password] [varchar](25) NOT NULL,
	[LastLogin] [datetime] NULL,
	[StatusId] [bit] NOT NULL,
	[CreatedBy] [smallint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [smallint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_AgentBoss] PRIMARY KEY CLUSTERED 
(
	[AbosID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Consumer]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Consumer](
	[ConsID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[PhoneNumber] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[IMEI] [varchar](50) NULL,
	[AccToken] [varchar](64) NULL,
	[AppID] [varchar](255) NULL,
	[AppToken] [varchar](255) NULL,
	[ProfileImage] [varchar](50) NULL,
	[LastLogin] [datetime] NULL,
	[ConsActivated] [bit] NOT NULL,
	[ConsBlocked] [bit] NOT NULL,
	[StatusID] [smallint] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Consumer] PRIMARY KEY CLUSTERED 
(
	[ConsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConsumerAddress]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConsumerAddress](
	[AddrID] [int] IDENTITY(1,1) NOT NULL,
	[ConsID] [int] NOT NULL,
	[Address] [varchar](150) NOT NULL,
	[DistID] [int] NOT NULL,
	[PostalCode] [varchar](10) NOT NULL,
	[AdditionalInfo] [varchar](150) NULL,
	[Latitude] [varchar](20) NULL,
	[Longitude] [varchar](20) NULL,
	[IsDefault] [bit] NOT NULL,
	[StatusID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ConsumerAddress] PRIMARY KEY CLUSTERED 
(
	[AddrID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConsumerReview]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConsumerReview](
	[ReviewID] [int] IDENTITY(1,1) NOT NULL,
	[ConsID] [int] NOT NULL,
	[OrdrID] [int] NOT NULL,
	[DrvrID] [int] NOT NULL,
	[Rating] [int] NOT NULL,
	[ReasonID] [int] NULL,
	[Comments] [varchar](max) NULL,
 CONSTRAINT [PK_ConsumerReview] PRIMARY KEY CLUSTERED 
(
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ContactInfo]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ContactInfo](
	[CinfoID] [int] IDENTITY(1,1) NOT NULL,
	[ContactInfoImage] [varchar](50) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[StatusId] [bit] NOT NULL,
	[CreatedBy] [smallint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [smallint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Consumer_Info] PRIMARY KEY CLUSTERED 
(
	[CinfoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DistributionPoint]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DistributionPoint](
	[DbptID] [int] IDENTITY(1,1) NOT NULL,
	[AgenID] [int] NOT NULL,
	[DistributionPointName] [varchar](100) NOT NULL,
	[Latitude] [varchar](20) NOT NULL,
	[Longitude] [varchar](20) NOT NULL,
	[Address] [varchar](250) NOT NULL,
	[StatusId] [bit] NOT NULL,
	[CreatedBy] [smallint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [smallint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_DistributionPoint] PRIMARY KEY CLUSTERED 
(
	[DbptID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Driver]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Driver](
	[DrvrID] [int] IDENTITY(1,1) NOT NULL,
	[DriverName] [varchar](100) NOT NULL,
	[AgenID] [int] NOT NULL,
	[DbptID] [int] NOT NULL,
	[MobileNumber] [varchar](20) NOT NULL,
	[StatusId] [bit] NOT NULL,
	[CreatedBy] [smallint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [smallint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Driver] PRIMARY KEY CLUSTERED 
(
	[DrvrID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MDeliverySlot]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MDeliverySlot](
	[SlotID] [smallint] IDENTITY(1,1) NOT NULL,
	[SlotName] [varchar](50) NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTine] [time](7) NOT NULL,
	[StatusId] [bit] NOT NULL,
 CONSTRAINT [PK_DeliverySlot] PRIMARY KEY CLUSTERED 
(
	[SlotID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MDeliveryStatus]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MDeliveryStatus](
	[DlstID] [smallint] IDENTITY(1,1) NOT NULL,
	[DeliveryStatus] [varchar](50) NOT NULL,
	[StatusId] [bit] NOT NULL,
 CONSTRAINT [PK_MDeliveryStatus] PRIMARY KEY CLUSTERED 
(
	[DlstID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MDistrict]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MDistrict](
	[DistID] [int] IDENTITY(1,1) NOT NULL,
	[DistName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_MDistrict] PRIMARY KEY CLUSTERED 
(
	[DistID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MFaq]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MFaq](
	[FaqID] [int] IDENTITY(1,1) NOT NULL,
	[Question] [varchar](500) NOT NULL,
	[Answer] [varchar](max) NOT NULL,
	[Position] [int] NOT NULL,
	[StatusID] [bit] NOT NULL,
 CONSTRAINT [PK_MFaq] PRIMARY KEY CLUSTERED 
(
	[FaqID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MOrderStatus]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MOrderStatus](
	[OrstID] [smallint] IDENTITY(1,1) NOT NULL,
	[OrderStatus] [varchar](50) NOT NULL,
	[StatusId] [bit] NOT NULL,
 CONSTRAINT [PK_MOrderStatus] PRIMARY KEY CLUSTERED 
(
	[OrstID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OneTimePwd]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OneTimePwd](
	[OtgID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[UserType] [char](1) NOT NULL,
	[Otg] [varchar](5) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_OneTimePwd] PRIMARY KEY CLUSTERED 
(
	[OtgID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Order]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Order](
	[OrdrID] [int] IDENTITY(1,1) NOT NULL,
	[ConsID] [int] NOT NULL,
	[AddrID] [int] NOT NULL,
	[AgadmID] [int] NULL,
	[InvoiceNumber] [varchar](25) NOT NULL,
	[OrderDate] [date] NOT NULL,
	[OrderTime] [time](7) NOT NULL,
	[DeliveryDate] [date] NOT NULL,
	[DeliverySlotID] [smallint] NOT NULL,
	[NumberOfProducts] [int] NOT NULL,
	[SubTotal] [numeric](18, 3) NOT NULL,
	[PromoProduct] [numeric](18, 3) NOT NULL,
	[ShippingCharge] [numeric](18, 3) NOT NULL,
	[PromoShipping] [numeric](18, 3) NOT NULL,
	[GrandTotal] [numeric](18, 3) NOT NULL,
	[StatusID] [smallint] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrdrID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderAllocationLog]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderAllocationLog](
	[AlogID] [bigint] IDENTITY(1,1) NOT NULL,
	[OrdrID] [int] NOT NULL,
	[AgadmID] [int] NOT NULL,
	[DbptID] [int] NOT NULL,
	[Distance] [int] NOT NULL,
	[TimeSlotAvailable] [bit] NOT NULL,
	[AllocationStatus] [smallint] NOT NULL,
	[AssignmentType] [smallint] NOT NULL,
	[CreatedBy] [smallint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_OrderAllocationLog] PRIMARY KEY CLUSTERED 
(
	[AlogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderDelivery]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDelivery](
	[DelvID] [int] IDENTITY(1,1) NOT NULL,
	[OrdrID] [int] NOT NULL,
	[DrvrID] [int] NOT NULL,
	[AgadmID] [int] NOT NULL,
	[AcceptedDate] [datetime] NOT NULL,
	[StartDate] [datetime] NULL,
	[DeliveryDate] [datetime] NULL,
	[StatusId] [smallint] NOT NULL,
	[deviation] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_OrderDelivery] PRIMARY KEY CLUSTERED 
(
	[DelvID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrdtID] [int] IDENTITY(1,1) NOT NULL,
	[OrdrID] [int] NOT NULL,
	[ProdID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [numeric](18, 3) NOT NULL,
	[SubTotal] [numeric](18, 3) NOT NULL,
	[PromoProduct] [numeric](18, 3) NOT NULL,
	[ShippingCharge] [numeric](18, 3) NOT NULL,
	[PromoShipping] [numeric](18, 3) NOT NULL,
	[TotamAmount] [numeric](18, 3) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[OrdtID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[ProdID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](150) NOT NULL,
	[Position] [int] NOT NULL,
	[ProductImage] [varchar](100) NULL,
	[ProductImageDetails] [varchar](100) NULL,
	[TubePrice] [numeric](18, 3) NOT NULL,
	[TubePromoPrice] [numeric](18, 3) NOT NULL,
	[RefillPrice] [numeric](18, 3) NOT NULL,
	[RefillPromoPrice] [numeric](18, 3) NOT NULL,
	[ShippingPrice] [numeric](18, 3) NOT NULL,
	[ShippingPromoPrice] [numeric](18, 3) NOT NULL,
	[Published] [bit] NOT NULL,
	[StatusId] [bit] NOT NULL,
	[CreatedBy] [smallint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [smallint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProdID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductExchange]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductExchange](
	[PrExID] [int] IDENTITY(1,1) NOT NULL,
	[ProdID] [int] NOT NULL,
	[ExchangeWith] [varchar](250) NOT NULL,
	[ExchangeQuantity] [int] NOT NULL,
	[ExchangePrice] [numeric](18, 18) NOT NULL,
	[ExchangePromoPrice] [numeric](18, 3) NOT NULL,
	[StatusId] [bit] NOT NULL,
	[CreatedBy] [smallint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [smallint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ProductExchange] PRIMARY KEY CLUSTERED 
(
	[PrExID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PromoBanner]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PromoBanner](
	[BannerID] [int] IDENTITY(1,1) NOT NULL,
	[Caption] [varchar](50) NOT NULL,
	[BannerImage] [varchar](100) NOT NULL,
	[Category] [int] NOT NULL,
	[StatusId] [bit] NOT NULL,
	[CreatedBy] [smallint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [smallint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_PromoBanner] PRIMARY KEY CLUSTERED 
(
	[BannerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PromoInfo]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PromoInfo](
	[InfoID] [int] IDENTITY(1,1) NOT NULL,
	[Caption] [varchar](50) NOT NULL,
	[InfoImage] [varchar](100) NOT NULL,
	[Position] [int] NOT NULL,
	[StatusID] [smallint] NOT NULL,
	[CreatedBy] [smallint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [smallint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_PromoInfo] PRIMARY KEY CLUSTERED 
(
	[InfoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Reminder]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Reminder](
	[RmdrID] [int] IDENTITY(1,1) NOT NULL,
	[ReminderImage] [varchar](50) NOT NULL,
	[UserType] [bit] NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[StatusId] [bit] NOT NULL,
	[CreatedBy] [smallint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [smallint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Reminder] PRIMARY KEY CLUSTERED 
(
	[RmdrID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SuperAdmin]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SuperAdmin](
	[SAdminID] [smallint] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](50) NOT NULL,
	[MobileNum] [varchar](20) NOT NULL,
	[Password] [varchar](25) NOT NULL,
	[ProfileImage] [varchar](50) NULL,
	[AppToken] [varchar](max) NULL,
	[LastLogin] [datetime] NULL,
	[UserType] [int] NOT NULL,
	[StatusID] [bit] NOT NULL,
 CONSTRAINT [PK_SuperAdmin] PRIMARY KEY CLUSTERED 
(
	[SAdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TeleCustomer]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TeleCustomer](
	[TeleCustID] [int] IDENTITY(1,1) NOT NULL,
	[TeleOrdID] [int] NOT NULL,
	[CustomerName] [varchar](100) NOT NULL,
	[Latitude] [varchar](20) NULL,
	[Longitude] [varchar](20) NULL,
	[Address] [varchar](max) NOT NULL,
	[MobileNumber] [varchar](20) NOT NULL,
	[StatusId] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TeleCustomers] PRIMARY KEY CLUSTERED 
(
	[TeleCustID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TeleOrder]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TeleOrder](
	[TeleOrdID] [int] IDENTITY(1,1) NOT NULL,
	[AgadmID] [int] NOT NULL,
	[DrvrID] [int] NOT NULL,
	[InvoiceNumber] [varchar](25) NOT NULL,
	[OrderDate] [date] NOT NULL,
	[OrderTime] [time](7) NOT NULL,
	[DeliveryDate] [date] NULL,
	[DeliverySlotID] [smallint] NULL,
	[NumberOfProducts] [int] NOT NULL,
	[SubTotal] [numeric](18, 3) NOT NULL,
	[PromoProduct] [numeric](18, 3) NULL,
	[ShippingCharge] [numeric](18, 3) NULL,
	[PromoShipping] [numeric](18, 3) NULL,
	[GrantTotal] [numeric](18, 3) NOT NULL,
	[DeliveryType] [bit] NOT NULL,
	[DeliveryStartDate] [datetime] NULL,
	[DeliveredDate] [datetime] NULL,
	[Deviation] [int] NOT NULL,
	[StatusId] [smallint] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TeleOrder] PRIMARY KEY CLUSTERED 
(
	[TeleOrdID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TeleOrderDetails]    Script Date: 30-08-2017 16:09:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeleOrderDetails](
	[TeleOrdDetID] [int] IDENTITY(1,1) NOT NULL,
	[TeleOrdID] [int] NOT NULL,
	[ProdID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [numeric](18, 3) NOT NULL,
	[SubTotal] [numeric](18, 3) NOT NULL,
	[PromoProduct] [numeric](18, 3) NULL,
	[ShippingCharge] [numeric](18, 3) NULL,
	[PromoShipping] [numeric](18, 3) NULL,
	[TotalAmount] [numeric](18, 3) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TeleOrderDetails] PRIMARY KEY CLUSTERED 
(
	[TeleOrdDetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ActivityLog] ADD  CONSTRAINT [DF_ActivityLog_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Agency] ADD  CONSTRAINT [DF_Agency_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[Agency] ADD  CONSTRAINT [DF_Agency_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Agency] ADD  CONSTRAINT [DF_Agency_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[AgentAdmin] ADD  CONSTRAINT [DF_Table_1_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[AgentAdmin] ADD  CONSTRAINT [DF_AgentAdmin_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[AgentAdmin] ADD  CONSTRAINT [DF_AgentAdmin_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[AgentBoss] ADD  CONSTRAINT [DF_AgentBoss_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[AgentBoss] ADD  CONSTRAINT [DF_AgentBoss_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[AgentBoss] ADD  CONSTRAINT [DF_AgentBoss_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Consumer] ADD  CONSTRAINT [DF_Consumer_UserActivated]  DEFAULT ((0)) FOR [ConsActivated]
GO
ALTER TABLE [dbo].[Consumer] ADD  CONSTRAINT [DF_Consumer_UserBlocked]  DEFAULT ((0)) FOR [ConsBlocked]
GO
ALTER TABLE [dbo].[Consumer] ADD  CONSTRAINT [DF_Consumer_StatusID]  DEFAULT ((1)) FOR [StatusID]
GO
ALTER TABLE [dbo].[Consumer] ADD  CONSTRAINT [DF_Consumer_CreatedBy]  DEFAULT ((0)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Consumer] ADD  CONSTRAINT [DF_Consumer_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Consumer] ADD  CONSTRAINT [DF_Consumer_UpdatedBy]  DEFAULT ((0)) FOR [UpdatedBy]
GO
ALTER TABLE [dbo].[Consumer] ADD  CONSTRAINT [DF_Consumer_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[ConsumerAddress] ADD  CONSTRAINT [DF_ConsumerAddress_IsDefault]  DEFAULT ((1)) FOR [IsDefault]
GO
ALTER TABLE [dbo].[ConsumerAddress] ADD  CONSTRAINT [DF_ConsumerAddress_StatusID]  DEFAULT ((1)) FOR [StatusID]
GO
ALTER TABLE [dbo].[ConsumerAddress] ADD  CONSTRAINT [DF_ConsumerAddress_CreatedBy]  DEFAULT ((0)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[ConsumerAddress] ADD  CONSTRAINT [DF_ConsumerAddress_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ConsumerAddress] ADD  CONSTRAINT [DF_ConsumerAddress_UpdatedBy]  DEFAULT ((0)) FOR [UpdatedBy]
GO
ALTER TABLE [dbo].[ConsumerAddress] ADD  CONSTRAINT [DF_ConsumerAddress_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[ConsumerReview] ADD  CONSTRAINT [DF_ConsumerReview_ReasonID]  DEFAULT ((0)) FOR [ReasonID]
GO
ALTER TABLE [dbo].[ContactInfo] ADD  CONSTRAINT [DF_ContactInfo_IsDefault]  DEFAULT ((1)) FOR [IsDefault]
GO
ALTER TABLE [dbo].[ContactInfo] ADD  CONSTRAINT [DF_ContactInfo_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[ContactInfo] ADD  CONSTRAINT [DF_ContactInfo_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ContactInfo] ADD  CONSTRAINT [DF_ContactInfo_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[DistributionPoint] ADD  CONSTRAINT [DF_DistributionPoint_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[DistributionPoint] ADD  CONSTRAINT [DF_DistributionPoint_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[DistributionPoint] ADD  CONSTRAINT [DF_DistributionPoint_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Driver] ADD  CONSTRAINT [DF_Driver_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[Driver] ADD  CONSTRAINT [DF_Driver_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Driver] ADD  CONSTRAINT [DF_Driver_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[MDeliverySlot] ADD  CONSTRAINT [DF_DeliverySlot_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[MDeliveryStatus] ADD  CONSTRAINT [DF_MDeliveryStatus_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[MFaq] ADD  CONSTRAINT [DF_MFaq_Position]  DEFAULT ((0)) FOR [Position]
GO
ALTER TABLE [dbo].[MFaq] ADD  CONSTRAINT [DF_MFaq_StatusID]  DEFAULT ((1)) FOR [StatusID]
GO
ALTER TABLE [dbo].[MOrderStatus] ADD  CONSTRAINT [DF_MOrderStatus_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[OneTimePwd] ADD  CONSTRAINT [DF_OneTimePwd_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_StatusID]  DEFAULT ((1)) FOR [StatusID]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[OrderAllocationLog] ADD  CONSTRAINT [DF_OrderAllocationLog_TimeSlotAvailable]  DEFAULT ((1)) FOR [TimeSlotAvailable]
GO
ALTER TABLE [dbo].[OrderAllocationLog] ADD  CONSTRAINT [DF_OrderAllocationLog_CreatedBy]  DEFAULT ((0)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[OrderAllocationLog] ADD  CONSTRAINT [DF_OrderAllocationLog_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[OrderAllocationLog] ADD  CONSTRAINT [DF_OrderAllocationLog_UpdatedBy]  DEFAULT ((0)) FOR [UpdatedBy]
GO
ALTER TABLE [dbo].[OrderAllocationLog] ADD  CONSTRAINT [DF_OrderAllocationLog_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[OrderDelivery] ADD  CONSTRAINT [DF_OrderDelivery_AcceptedDate]  DEFAULT (getdate()) FOR [AcceptedDate]
GO
ALTER TABLE [dbo].[OrderDelivery] ADD  CONSTRAINT [DF_OrderDelivery_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[OrderDelivery] ADD  CONSTRAINT [DF_OrderDelivery_deviation]  DEFAULT ((0)) FOR [deviation]
GO
ALTER TABLE [dbo].[OrderDelivery] ADD  CONSTRAINT [DF_OrderDelivery_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[OrderDetails] ADD  CONSTRAINT [DF_OrderDetails_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_TubePrice]  DEFAULT ((0)) FOR [TubePrice]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_TubePromoPrice]  DEFAULT ((0)) FOR [TubePromoPrice]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_RefillPrice]  DEFAULT ((0)) FOR [RefillPrice]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_RefillPromoPrice]  DEFAULT ((0)) FOR [RefillPromoPrice]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_ShippingPrice]  DEFAULT ((0)) FOR [ShippingPrice]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_ShippingPromoPrice]  DEFAULT ((0)) FOR [ShippingPromoPrice]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_StatusID]  DEFAULT ((1)) FOR [Published]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_StatusID_1]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[ProductExchange] ADD  CONSTRAINT [DF_ProductExchange_ExchangeQuantity]  DEFAULT ((0)) FOR [ExchangeQuantity]
GO
ALTER TABLE [dbo].[ProductExchange] ADD  CONSTRAINT [DF_ProductExchange_ExchangePrice]  DEFAULT ((0)) FOR [ExchangePrice]
GO
ALTER TABLE [dbo].[ProductExchange] ADD  CONSTRAINT [DF_ProductExchange_ExchangePromoPrice]  DEFAULT ((0)) FOR [ExchangePromoPrice]
GO
ALTER TABLE [dbo].[ProductExchange] ADD  CONSTRAINT [DF_ProductExchange_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[ProductExchange] ADD  CONSTRAINT [DF_ProductExchange_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ProductExchange] ADD  CONSTRAINT [DF_ProductExchange_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[PromoBanner] ADD  CONSTRAINT [DF_PromoBanner_StatusID]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[PromoBanner] ADD  CONSTRAINT [DF_PromoBanner_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PromoBanner] ADD  CONSTRAINT [DF_PromoBanner_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[PromoInfo] ADD  CONSTRAINT [DF_PromoInfo_StatusID]  DEFAULT ((1)) FOR [StatusID]
GO
ALTER TABLE [dbo].[PromoInfo] ADD  CONSTRAINT [DF_PromoInfo_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PromoInfo] ADD  CONSTRAINT [DF_PromoInfo_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Reminder] ADD  CONSTRAINT [DF_Reminder_UserType]  DEFAULT ((1)) FOR [UserType]
GO
ALTER TABLE [dbo].[Reminder] ADD  CONSTRAINT [DF_Reminder_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Reminder] ADD  CONSTRAINT [DF_Reminder_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[SuperAdmin] ADD  CONSTRAINT [DF_SuperAdmin_UserType]  DEFAULT ((1)) FOR [UserType]
GO
ALTER TABLE [dbo].[SuperAdmin] ADD  CONSTRAINT [DF_SuperAdmin_StatusID]  DEFAULT ((1)) FOR [StatusID]
GO
ALTER TABLE [dbo].[TeleCustomer] ADD  CONSTRAINT [DF_TeleCustomers_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[TeleCustomer] ADD  CONSTRAINT [DF_TeleCustomers_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[TeleOrder] ADD  CONSTRAINT [DF_TeleOrder_DeliveryType]  DEFAULT ((1)) FOR [DeliveryType]
GO
ALTER TABLE [dbo].[TeleOrder] ADD  CONSTRAINT [DF_TeleOrder_Deviation]  DEFAULT ((0)) FOR [Deviation]
GO
ALTER TABLE [dbo].[TeleOrder] ADD  CONSTRAINT [DF_TeleOrder_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[TeleOrder] ADD  CONSTRAINT [DF_TeleOrder_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[TeleOrderDetails] ADD  CONSTRAINT [DF_TeleOrderDetails_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ActivityLog]  WITH CHECK ADD  CONSTRAINT [FK_ActivityLog_SuperAdmin] FOREIGN KEY([SAdminID])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[ActivityLog] CHECK CONSTRAINT [FK_ActivityLog_SuperAdmin]
GO
ALTER TABLE [dbo].[Agency]  WITH CHECK ADD  CONSTRAINT [FK_Agency_SuperAdmin_Create] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[Agency] CHECK CONSTRAINT [FK_Agency_SuperAdmin_Create]
GO
ALTER TABLE [dbo].[Agency]  WITH CHECK ADD  CONSTRAINT [FK_Agency_SuperAdmin_Update] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[Agency] CHECK CONSTRAINT [FK_Agency_SuperAdmin_Update]
GO
ALTER TABLE [dbo].[AgentAdmin]  WITH CHECK ADD  CONSTRAINT [FK_AgentAdmin_Agency] FOREIGN KEY([AgenID])
REFERENCES [dbo].[Agency] ([AgenID])
GO
ALTER TABLE [dbo].[AgentAdmin] CHECK CONSTRAINT [FK_AgentAdmin_Agency]
GO
ALTER TABLE [dbo].[AgentAdmin]  WITH CHECK ADD  CONSTRAINT [FK_AgentAdmin_DistributionPoint] FOREIGN KEY([DbptID])
REFERENCES [dbo].[DistributionPoint] ([DbptID])
GO
ALTER TABLE [dbo].[AgentAdmin] CHECK CONSTRAINT [FK_AgentAdmin_DistributionPoint]
GO
ALTER TABLE [dbo].[AgentAdmin]  WITH CHECK ADD  CONSTRAINT [FK_AgentAdmin_SuperAdmin_Create] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[AgentAdmin] CHECK CONSTRAINT [FK_AgentAdmin_SuperAdmin_Create]
GO
ALTER TABLE [dbo].[AgentBoss]  WITH CHECK ADD  CONSTRAINT [FK_AgentBoss_Agency] FOREIGN KEY([AgenID])
REFERENCES [dbo].[Agency] ([AgenID])
GO
ALTER TABLE [dbo].[AgentBoss] CHECK CONSTRAINT [FK_AgentBoss_Agency]
GO
ALTER TABLE [dbo].[AgentBoss]  WITH CHECK ADD  CONSTRAINT [FK_AgentBoss_SuperAdmin_Create] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[AgentBoss] CHECK CONSTRAINT [FK_AgentBoss_SuperAdmin_Create]
GO
ALTER TABLE [dbo].[ConsumerAddress]  WITH CHECK ADD  CONSTRAINT [FK_ConsumerAddress_Consumer] FOREIGN KEY([ConsID])
REFERENCES [dbo].[Consumer] ([ConsID])
GO
ALTER TABLE [dbo].[ConsumerAddress] CHECK CONSTRAINT [FK_ConsumerAddress_Consumer]
GO
ALTER TABLE [dbo].[ConsumerAddress]  WITH CHECK ADD  CONSTRAINT [FK_ConsumerAddress_MDistrict] FOREIGN KEY([DistID])
REFERENCES [dbo].[MDistrict] ([DistID])
GO
ALTER TABLE [dbo].[ConsumerAddress] CHECK CONSTRAINT [FK_ConsumerAddress_MDistrict]
GO
ALTER TABLE [dbo].[ConsumerReview]  WITH CHECK ADD  CONSTRAINT [FK_ConsumerReview_Consumer] FOREIGN KEY([ConsID])
REFERENCES [dbo].[Consumer] ([ConsID])
GO
ALTER TABLE [dbo].[ConsumerReview] CHECK CONSTRAINT [FK_ConsumerReview_Consumer]
GO
ALTER TABLE [dbo].[ConsumerReview]  WITH CHECK ADD  CONSTRAINT [FK_ConsumerReview_Driver] FOREIGN KEY([DrvrID])
REFERENCES [dbo].[Driver] ([DrvrID])
GO
ALTER TABLE [dbo].[ConsumerReview] CHECK CONSTRAINT [FK_ConsumerReview_Driver]
GO
ALTER TABLE [dbo].[ConsumerReview]  WITH CHECK ADD  CONSTRAINT [FK_ConsumerReview_Order] FOREIGN KEY([OrdrID])
REFERENCES [dbo].[Order] ([OrdrID])
GO
ALTER TABLE [dbo].[ConsumerReview] CHECK CONSTRAINT [FK_ConsumerReview_Order]
GO
ALTER TABLE [dbo].[ContactInfo]  WITH CHECK ADD  CONSTRAINT [FK_ContactInfo_SuperAdmin_Create] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[ContactInfo] CHECK CONSTRAINT [FK_ContactInfo_SuperAdmin_Create]
GO
ALTER TABLE [dbo].[ContactInfo]  WITH CHECK ADD  CONSTRAINT [FK_ContactInfo_SuperAdmin_Update] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[ContactInfo] CHECK CONSTRAINT [FK_ContactInfo_SuperAdmin_Update]
GO
ALTER TABLE [dbo].[DistributionPoint]  WITH CHECK ADD  CONSTRAINT [FK_DistributionPoint_Agency] FOREIGN KEY([AgenID])
REFERENCES [dbo].[Agency] ([AgenID])
GO
ALTER TABLE [dbo].[DistributionPoint] CHECK CONSTRAINT [FK_DistributionPoint_Agency]
GO
ALTER TABLE [dbo].[DistributionPoint]  WITH CHECK ADD  CONSTRAINT [FK_DistributionPoint_SuperAdmin_Create] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[DistributionPoint] CHECK CONSTRAINT [FK_DistributionPoint_SuperAdmin_Create]
GO
ALTER TABLE [dbo].[DistributionPoint]  WITH CHECK ADD  CONSTRAINT [FK_DistributionPoint_SuperAdmin_Update] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[DistributionPoint] CHECK CONSTRAINT [FK_DistributionPoint_SuperAdmin_Update]
GO
ALTER TABLE [dbo].[Driver]  WITH CHECK ADD  CONSTRAINT [FK_Driver_Agency] FOREIGN KEY([AgenID])
REFERENCES [dbo].[Agency] ([AgenID])
GO
ALTER TABLE [dbo].[Driver] CHECK CONSTRAINT [FK_Driver_Agency]
GO
ALTER TABLE [dbo].[Driver]  WITH CHECK ADD  CONSTRAINT [FK_Driver_DistributionPoint] FOREIGN KEY([DbptID])
REFERENCES [dbo].[DistributionPoint] ([DbptID])
GO
ALTER TABLE [dbo].[Driver] CHECK CONSTRAINT [FK_Driver_DistributionPoint]
GO
ALTER TABLE [dbo].[Driver]  WITH CHECK ADD  CONSTRAINT [FK_Driver_SuperAdmin_Create] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[Driver] CHECK CONSTRAINT [FK_Driver_SuperAdmin_Create]
GO
ALTER TABLE [dbo].[Driver]  WITH CHECK ADD  CONSTRAINT [FK_Driver_SuperAdmin_Update] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[Driver] CHECK CONSTRAINT [FK_Driver_SuperAdmin_Update]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_AgentAdmin] FOREIGN KEY([AgadmID])
REFERENCES [dbo].[AgentAdmin] ([AgadmID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_AgentAdmin]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Consumer] FOREIGN KEY([ConsID])
REFERENCES [dbo].[Consumer] ([ConsID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Consumer]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_ConsumerAddress] FOREIGN KEY([AddrID])
REFERENCES [dbo].[ConsumerAddress] ([AddrID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_ConsumerAddress]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_MDeliverySlot] FOREIGN KEY([DeliverySlotID])
REFERENCES [dbo].[MDeliverySlot] ([SlotID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_MDeliverySlot]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_MOrderStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[MOrderStatus] ([OrstID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_MOrderStatus]
GO
ALTER TABLE [dbo].[OrderDelivery]  WITH CHECK ADD  CONSTRAINT [FK_OrderDelivery_AgentAdmin] FOREIGN KEY([AgadmID])
REFERENCES [dbo].[AgentAdmin] ([AgadmID])
GO
ALTER TABLE [dbo].[OrderDelivery] CHECK CONSTRAINT [FK_OrderDelivery_AgentAdmin]
GO
ALTER TABLE [dbo].[OrderDelivery]  WITH CHECK ADD  CONSTRAINT [FK_OrderDelivery_Driver] FOREIGN KEY([DrvrID])
REFERENCES [dbo].[Driver] ([DrvrID])
GO
ALTER TABLE [dbo].[OrderDelivery] CHECK CONSTRAINT [FK_OrderDelivery_Driver]
GO
ALTER TABLE [dbo].[OrderDelivery]  WITH CHECK ADD  CONSTRAINT [FK_OrderDelivery_MDeliveryStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[MDeliveryStatus] ([DlstID])
GO
ALTER TABLE [dbo].[OrderDelivery] CHECK CONSTRAINT [FK_OrderDelivery_MDeliveryStatus]
GO
ALTER TABLE [dbo].[OrderDelivery]  WITH CHECK ADD  CONSTRAINT [FK_OrderDelivery_Order] FOREIGN KEY([OrdrID])
REFERENCES [dbo].[Order] ([OrdrID])
GO
ALTER TABLE [dbo].[OrderDelivery] CHECK CONSTRAINT [FK_OrderDelivery_Order]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Order] FOREIGN KEY([OrdrID])
REFERENCES [dbo].[Order] ([OrdrID])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Order]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Product] FOREIGN KEY([ProdID])
REFERENCES [dbo].[Product] ([ProdID])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Product]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_SuperAdmin_Create] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_SuperAdmin_Create]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_SuperAdmin_Update] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_SuperAdmin_Update]
GO
ALTER TABLE [dbo].[ProductExchange]  WITH CHECK ADD  CONSTRAINT [FK_ProductExchange_Product] FOREIGN KEY([ProdID])
REFERENCES [dbo].[Product] ([ProdID])
GO
ALTER TABLE [dbo].[ProductExchange] CHECK CONSTRAINT [FK_ProductExchange_Product]
GO
ALTER TABLE [dbo].[ProductExchange]  WITH CHECK ADD  CONSTRAINT [FK_ProductExchange_SuperAdmin_Create] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[ProductExchange] CHECK CONSTRAINT [FK_ProductExchange_SuperAdmin_Create]
GO
ALTER TABLE [dbo].[ProductExchange]  WITH CHECK ADD  CONSTRAINT [FK_ProductExchange_SuperAdmin_Update] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[ProductExchange] CHECK CONSTRAINT [FK_ProductExchange_SuperAdmin_Update]
GO
ALTER TABLE [dbo].[PromoBanner]  WITH CHECK ADD  CONSTRAINT [FK_PromoBanner_PromoBanner_Update] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[PromoBanner] CHECK CONSTRAINT [FK_PromoBanner_PromoBanner_Update]
GO
ALTER TABLE [dbo].[PromoBanner]  WITH CHECK ADD  CONSTRAINT [FK_PromoBanner_SuperAdmin_Create] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[PromoBanner] CHECK CONSTRAINT [FK_PromoBanner_SuperAdmin_Create]
GO
ALTER TABLE [dbo].[PromoInfo]  WITH CHECK ADD  CONSTRAINT [FK_PromoInfo_SuperAdmin_create] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[PromoInfo] CHECK CONSTRAINT [FK_PromoInfo_SuperAdmin_create]
GO
ALTER TABLE [dbo].[PromoInfo]  WITH CHECK ADD  CONSTRAINT [FK_PromoInfo_SuperAdmin_update] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[PromoInfo] CHECK CONSTRAINT [FK_PromoInfo_SuperAdmin_update]
GO
ALTER TABLE [dbo].[Reminder]  WITH CHECK ADD  CONSTRAINT [FK_Reminder_SuperAdmin_Create] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[Reminder] CHECK CONSTRAINT [FK_Reminder_SuperAdmin_Create]
GO
ALTER TABLE [dbo].[Reminder]  WITH CHECK ADD  CONSTRAINT [FK_Reminder_SuperAdmin_Update] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[SuperAdmin] ([SAdminID])
GO
ALTER TABLE [dbo].[Reminder] CHECK CONSTRAINT [FK_Reminder_SuperAdmin_Update]
GO
ALTER TABLE [dbo].[TeleCustomer]  WITH CHECK ADD  CONSTRAINT [FK_TeleCustomer_AgentAdmin_Create] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AgentAdmin] ([AgadmID])
GO
ALTER TABLE [dbo].[TeleCustomer] CHECK CONSTRAINT [FK_TeleCustomer_AgentAdmin_Create]
GO
ALTER TABLE [dbo].[TeleCustomer]  WITH CHECK ADD  CONSTRAINT [FK_TeleCustomer_TeleOrder] FOREIGN KEY([TeleOrdID])
REFERENCES [dbo].[TeleOrder] ([TeleOrdID])
GO
ALTER TABLE [dbo].[TeleCustomer] CHECK CONSTRAINT [FK_TeleCustomer_TeleOrder]
GO
ALTER TABLE [dbo].[TeleOrder]  WITH CHECK ADD  CONSTRAINT [FK_TeleOrder_AgentAdmin] FOREIGN KEY([AgadmID])
REFERENCES [dbo].[AgentAdmin] ([AgadmID])
GO
ALTER TABLE [dbo].[TeleOrder] CHECK CONSTRAINT [FK_TeleOrder_AgentAdmin]
GO
ALTER TABLE [dbo].[TeleOrder]  WITH CHECK ADD  CONSTRAINT [FK_TeleOrder_AgentAdmin_Created] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AgentAdmin] ([AgadmID])
GO
ALTER TABLE [dbo].[TeleOrder] CHECK CONSTRAINT [FK_TeleOrder_AgentAdmin_Created]
GO
ALTER TABLE [dbo].[TeleOrder]  WITH CHECK ADD  CONSTRAINT [FK_TeleOrder_AgentAdmin_Update] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[AgentAdmin] ([AgadmID])
GO
ALTER TABLE [dbo].[TeleOrder] CHECK CONSTRAINT [FK_TeleOrder_AgentAdmin_Update]
GO
ALTER TABLE [dbo].[TeleOrder]  WITH CHECK ADD  CONSTRAINT [FK_TeleOrder_Driver] FOREIGN KEY([DrvrID])
REFERENCES [dbo].[Driver] ([DrvrID])
GO
ALTER TABLE [dbo].[TeleOrder] CHECK CONSTRAINT [FK_TeleOrder_Driver]
GO
ALTER TABLE [dbo].[TeleOrder]  WITH CHECK ADD  CONSTRAINT [FK_TeleOrder_MDeliverySlot] FOREIGN KEY([DeliverySlotID])
REFERENCES [dbo].[MDeliverySlot] ([SlotID])
GO
ALTER TABLE [dbo].[TeleOrder] CHECK CONSTRAINT [FK_TeleOrder_MDeliverySlot]
GO
ALTER TABLE [dbo].[TeleOrder]  WITH CHECK ADD  CONSTRAINT [FK_TeleOrder_MOrderStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[MOrderStatus] ([OrstID])
GO
ALTER TABLE [dbo].[TeleOrder] CHECK CONSTRAINT [FK_TeleOrder_MOrderStatus]
GO
ALTER TABLE [dbo].[TeleOrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_TeleOrderDetails_Product] FOREIGN KEY([ProdID])
REFERENCES [dbo].[Product] ([ProdID])
GO
ALTER TABLE [dbo].[TeleOrderDetails] CHECK CONSTRAINT [FK_TeleOrderDetails_Product]
GO
ALTER TABLE [dbo].[TeleOrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_TeleOrderDetails_TeleOrder] FOREIGN KEY([TeleOrdID])
REFERENCES [dbo].[TeleOrder] ([TeleOrdID])
GO
ALTER TABLE [dbo].[TeleOrderDetails] CHECK CONSTRAINT [FK_TeleOrderDetails_TeleOrder]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 => TeleOrder, 0 => Pickup' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TeleOrder', @level2type=N'COLUMN',@level2name=N'DeliveryType'
GO
