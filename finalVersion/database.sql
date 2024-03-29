USE [CrowFuding]
GO
/****** Object:  Table [dbo].[Compaign]    Script Date: 8/10/2022 22:47:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Compaign](
	[compaign_id] [int] IDENTITY(1,1) NOT NULL,
	[compaign_creater_id] [int] NOT NULL,
	[compaign_description] [varchar](max) NOT NULL,
	[compaign_date_created] [varchar](255) NOT NULL CONSTRAINT [DF__Compaign__compai__145C0A3F]  DEFAULT (getdate()),
	[compaign_title] [varchar](50) NOT NULL,
 CONSTRAINT [PK__Compaign__9EADEF8F06F86630] PRIMARY KEY CLUSTERED 
(
	[compaign_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Donation]    Script Date: 8/10/2022 22:47:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Donation](
	[donation_reference] [int] IDENTITY(1,1) NOT NULL,
	[donation_compaign_id] [int] NOT NULL,
	[donation_user_id] [int] NOT NULL,
	[donation_amount] [int] NOT NULL,
	[donation_date_created] [varchar](255) NOT NULL CONSTRAINT [DF__Donation__donati__182C9B23]  DEFAULT (getdate()),
 CONSTRAINT [PK__Donation__B541DCF36C297681] PRIMARY KEY CLUSTERED 
(
	[donation_reference] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 8/10/2022 22:47:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Profile](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[user_fname] [varchar](64) NOT NULL,
	[user_email] [varchar](30) NOT NULL,
	[user_address] [varchar](255) NOT NULL,
	[user_password] [varchar](255) NOT NULL,
	[user_Lastname] [varchar](50) NOT NULL,
 CONSTRAINT [PK__Profile__B9BE370F9EDFE78B] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[refund]    Script Date: 8/10/2022 22:47:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[refund](
	[refund_id] [int] IDENTITY(1,1) NOT NULL,
	[users_id] [int] NOT NULL,
	[Compaign_id] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[donation_reference] [int] NULL,
 CONSTRAINT [PK__refund__897E9EA329996528] PRIMARY KEY CLUSTERED 
(
	[refund_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[withdrawl_desciption]    Script Date: 8/10/2022 22:47:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[withdrawl_desciption](
	[withdrawl_reference] [int] IDENTITY(1,1) NOT NULL,
	[withdrawl_discription] [varchar](max) NOT NULL,
	[withdrawl_user_id] [int] NOT NULL,
	[withdrawl_amount] [int] NOT NULL,
	[withdrawl_compaign_id] [int] NOT NULL,
	[donation_date_created] [varchar](255) NOT NULL CONSTRAINT [DF__withdrawl__donat__1CF15040]  DEFAULT (getdate()),
	[withdrawl_status] [int] NULL,
 CONSTRAINT [PK__withdraw__0EBF3A017AD5F1A2] PRIMARY KEY CLUSTERED 
(
	[withdrawl_reference] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[withdrawls_request]    Script Date: 8/10/2022 22:47:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[withdrawls_request](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requested_by] [int] NOT NULL,
	[requested_to] [int] NOT NULL,
	[withdraw_description_id] [int] NOT NULL,
	[status] [int] NOT NULL,
 CONSTRAINT [PK_withdrawls_request] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Compaign]  WITH CHECK ADD  CONSTRAINT [FK__Compaign__compai__15502E78] FOREIGN KEY([compaign_creater_id])
REFERENCES [dbo].[Profile] ([user_id])
GO
ALTER TABLE [dbo].[Compaign] CHECK CONSTRAINT [FK__Compaign__compai__15502E78]
GO
ALTER TABLE [dbo].[Donation]  WITH CHECK ADD  CONSTRAINT [FK__Donation__donati__1920BF5C] FOREIGN KEY([donation_compaign_id])
REFERENCES [dbo].[Compaign] ([compaign_id])
GO
ALTER TABLE [dbo].[Donation] CHECK CONSTRAINT [FK__Donation__donati__1920BF5C]
GO
ALTER TABLE [dbo].[Donation]  WITH CHECK ADD  CONSTRAINT [FK__Donation__donati__1A14E395] FOREIGN KEY([donation_user_id])
REFERENCES [dbo].[Profile] ([user_id])
GO
ALTER TABLE [dbo].[Donation] CHECK CONSTRAINT [FK__Donation__donati__1A14E395]
GO
ALTER TABLE [dbo].[refund]  WITH CHECK ADD  CONSTRAINT [FK__refund__Compaign__70DDC3D8] FOREIGN KEY([Compaign_id])
REFERENCES [dbo].[Compaign] ([compaign_id])
GO
ALTER TABLE [dbo].[refund] CHECK CONSTRAINT [FK__refund__Compaign__70DDC3D8]
GO
ALTER TABLE [dbo].[refund]  WITH CHECK ADD  CONSTRAINT [FK__refund__users_id__6FE99F9F] FOREIGN KEY([users_id])
REFERENCES [dbo].[Profile] ([user_id])
GO
ALTER TABLE [dbo].[refund] CHECK CONSTRAINT [FK__refund__users_id__6FE99F9F]
GO
ALTER TABLE [dbo].[withdrawl_desciption]  WITH CHECK ADD  CONSTRAINT [FK__withdrawl__withd__1DE57479] FOREIGN KEY([withdrawl_compaign_id])
REFERENCES [dbo].[Compaign] ([compaign_id])
GO
ALTER TABLE [dbo].[withdrawl_desciption] CHECK CONSTRAINT [FK__withdrawl__withd__1DE57479]
GO
ALTER TABLE [dbo].[withdrawl_desciption]  WITH CHECK ADD  CONSTRAINT [FK__withdrawl__withd__1ED998B2] FOREIGN KEY([withdrawl_user_id])
REFERENCES [dbo].[Profile] ([user_id])
GO
ALTER TABLE [dbo].[withdrawl_desciption] CHECK CONSTRAINT [FK__withdrawl__withd__1ED998B2]
GO
ALTER TABLE [dbo].[withdrawl_desciption]  WITH CHECK ADD  CONSTRAINT [FK_withdrawl_request_withdrawl_request] FOREIGN KEY([withdrawl_reference])
REFERENCES [dbo].[withdrawl_desciption] ([withdrawl_reference])
GO
ALTER TABLE [dbo].[withdrawl_desciption] CHECK CONSTRAINT [FK_withdrawl_request_withdrawl_request]
GO
