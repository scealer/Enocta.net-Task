USE [KTOtomasyon]
GO


DROP VIEW [dbo].[vLastTotalOrder]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[vLastTotalOrder]
AS
  select ISNULL(ROW_NUMBER() OVER(ORDER BY SipMiktar),0) Sira
		 ,SipMiktar
		 ,SipTutar 
  from vTotalOrder 
  where Yil = YEAR(GETDATE()) 
    and Ay = MONTH(GETDATE()) 
	and Hafta = DATEPART(WW,GETDATE())
GO


