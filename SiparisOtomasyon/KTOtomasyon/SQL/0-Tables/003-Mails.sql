USE [KTOtomasyon]
GO

ALTER TABLE [dbo].[Mails] DROP CONSTRAINT [FK_Mails_Users]
GO


DROP TABLE [dbo].[Mails]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mails](
	[Mail_Id] [int] IDENTITY(1,1) NOT NULL,
	[MailSubject] [nvarchar](max) NULL,
	[MailBody] [nvarchar](max) NULL,
	[MailTo] [nvarchar](300) NULL,
	[MailFrom] [nvarchar](300) NULL,
	[MailCC] [nvarchar](300) NULL,
	[MailBCC] [nvarchar](300) NULL,
	[IsBodyHtml] [bit] NULL,
	[IsSend] [bit] NULL,
	[SendDate] [datetime] NULL,
	[ErrorMessage] [nvarchar](300) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [int] NULL,
 CONSTRAINT [PK_Mails] PRIMARY KEY CLUSTERED 
(
	[Mail_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Mails]  WITH CHECK ADD  CONSTRAINT [FK_Mails_Users] FOREIGN KEY([CreatedUser])
REFERENCES [dbo].[Users] ([User_Id])
GO

ALTER TABLE [dbo].[Mails] CHECK CONSTRAINT [FK_Mails_Users]
GO


