USE [KTOtomasyon]
GO

ALTER TABLE [dbo].[Operations] DROP CONSTRAINT [FK_Operations_Products]
GO

/****** Object:  Table [dbo].[Operations]    Script Date: 23.9.2018 02:11:24 ******/
DROP TABLE [dbo].[Operations]
GO

/****** Object:  Table [dbo].[Operations]    Script Date: 23.9.2018 02:11:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Operations](
	[Operation_Id] [int] IDENTITY(1,1) NOT NULL,
	[Product_Id] [int] NULL,
	[Name] [varchar](50) NULL,
	[Price] [int] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Operations] PRIMARY KEY CLUSTERED 
(
	[Operation_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Operations]  WITH CHECK ADD  CONSTRAINT [FK_Operations_Products] FOREIGN KEY([Product_Id])
REFERENCES [dbo].[Products] ([Product_Id])
GO

ALTER TABLE [dbo].[Operations] CHECK CONSTRAINT [FK_Operations_Products]
GO


