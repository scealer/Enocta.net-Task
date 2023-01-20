USE [KTOtomasyon]
GO


DROP VIEW [dbo].[vTodayTotalOrder]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- SELECT * FROM [vTodayTotalOrder]

CREATE VIEW [dbo].[vTodayTotalOrder]
AS
SELECT  ISNULL(ROW_NUMBER() OVER(ORDER BY SUM(ORDET.TotalPrice)),0) Sira,
		--CONVERT(nvarchar(10), ORS.OrderDate,126) SipTarih,		
		COUNT(ORS.Order_Id) SipMiktar,
		SUM(ORDET.TotalPrice) SipTutar	
  
FROM Orders ORS
INNER JOIN OrderDetail ORDET ON ORS.Order_Id = ORDET.Order_Id 
GROUP BY ORS.OrderDate

GO


