USE [KTOtomasyon]
GO

ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_CreatedUser]
GO

/****** Object:  Table [dbo].[Orders]    Script Date: 23.9.2018 02:12:12 ******/
DROP TABLE [dbo].[Orders]
GO

/****** Object:  Table [dbo].[Orders]    Script Date: 23.9.2018 02:12:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Orders](
	[Order_Id] [int] IDENTITY(1000000,1) NOT NULL,
	[CustomerName] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[Debt] [nvarchar](max) NULL,
	[Addition] [nvarchar](max) NULL,
	[OrderDate] [datetime] NULL,
	[CreatedUser] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[IsPaid] [bit] NULL,
	[IsDelivered] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[Discount] [int] NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Order_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_CreatedUser] FOREIGN KEY([CreatedUser])
REFERENCES [dbo].[Users] ([User_Id])
GO

ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_CreatedUser]
GO


