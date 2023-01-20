USE [KTOtomasyon]
GO

/****** Object:  Table [dbo].[Logs]    Script Date: 23.9.2018 02:10:25 ******/
DROP TABLE [dbo].[Logs]
GO

/****** Object:  Table [dbo].[Logs]    Script Date: 23.9.2018 02:10:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Logs](
	[Log_Id] [int] IDENTITY(1,1) NOT NULL,
	[MethodName] [nvarchar](150) NULL,
	[ExMessage] [nvarchar](max) NULL,
	[Message] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [int] NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[Log_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


