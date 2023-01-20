USE [KTOtomasyon]
GO

ALTER TABLE [dbo].[OrderDetail] DROP CONSTRAINT [FK_OrderDetail_Orders]
GO

ALTER TABLE [dbo].[OrderDetail] DROP CONSTRAINT [FK_OrderDetail_Operations]
GO

/****** Object:  Table [dbo].[OrderDetail]    Script Date: 23.9.2018 02:11:44 ******/
DROP TABLE [dbo].[OrderDetail]
GO

/****** Object:  Table [dbo].[OrderDetail]    Script Date: 23.9.2018 02:11:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderDetail](
	[OrderDetail_Id] [int] IDENTITY(1,1) NOT NULL,
	[Order_Id] [int] NULL,
	[Operation_Id] [int] NULL,
	[Quantity] [int] NULL,
	[Price] [decimal](12, 6) NULL,
	[TotalPrice] [decimal](12, 6) NULL,
 CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[OrderDetail_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Operations] FOREIGN KEY([Operation_Id])
REFERENCES [dbo].[Operations] ([Operation_Id])
GO

ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Operations]
GO

ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Orders] FOREIGN KEY([Order_Id])
REFERENCES [dbo].[Orders] ([Order_Id])
GO

ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Orders]
GO


