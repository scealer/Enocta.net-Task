USE [KTOtomasyon]
GO


DROP PROCEDURE [dbo].[ILKPROS]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[ILKPROS]
(
	@firstodate datetime,
	@lastodate datetime
)
AS
BEGIN
	SET NOCOUNT ON;
		
	SELECT SUM(ORDET.TotalPrice) Toplam FROM OrderDetail ORDET 
	INNER JOIN Orders ORS ON ORS.Order_Id = ORDET.Order_Id 

	WHERE ORS.OrderDate <= @lastodate AND ORS.OrderDate > @firstodate

	-- SELECT Price,Quantity,TotalPrice FROM OrderDetail
END
GO


