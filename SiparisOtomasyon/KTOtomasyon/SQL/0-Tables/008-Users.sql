USE [KTOtomasyon]
GO


DROP TABLE [dbo].[Users]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[User_Id] [int] NOT NULL,
	[Name] [nvarchar](10) NULL,
	[Gender] [bit] NULL,
	[Birthday] [datetime] NULL,
	[Mail] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[UserType] [int] NULL,
	[IsDeleted] [bit] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


