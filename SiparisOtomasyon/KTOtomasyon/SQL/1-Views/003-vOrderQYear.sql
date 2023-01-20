USE [KTOtomasyon]
GO


DROP VIEW [dbo].[vOrderQYear]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- SELECT * FROM vOrderQYear 
CREATE VIEW [dbo].[vOrderQYear]
AS
SELECT
	CL.Year,
	ISNULL((SELECT SUM(VT.SipMiktar) FROM vTotalOrder VT WHERE VT.Ay = 1 AND CL.YEAR = VT.Yil),0) Ocak,
	ISNULL((SELECT SUM(VT.SipMiktar) FROM vTotalOrder VT WHERE VT.Ay = 2 AND CL.YEAR = VT.Yil),0) Subat,
	ISNULL((SELECT SUM(VT.SipMiktar) FROM vTotalOrder VT WHERE VT.Ay = 3 AND CL.YEAR = VT.Yil),0) Mart,
	ISNULL((SELECT SUM(VT.SipMiktar) FROM vTotalOrder VT WHERE VT.Ay = 4 AND CL.YEAR = VT.Yil),0) Nisan,
	ISNULL((SELECT SUM(VT.SipMiktar) FROM vTotalOrder VT WHERE VT.Ay = 5 AND CL.YEAR = VT.Yil),0) Mayis,
	ISNULL((SELECT SUM(VT.SipMiktar) FROM vTotalOrder VT WHERE VT.Ay = 6	AND CL.YEAR = VT.Yil),0) Haziran,
	ISNULL((SELECT SUM(VT.SipMiktar) FROM vTotalOrder VT WHERE VT.Ay = 7 AND CL.YEAR = VT.Yil),0) Temmuz,
	ISNULL((SELECT SUM(VT.SipMiktar) FROM vTotalOrder VT WHERE VT.Ay = 8 AND CL.YEAR = VT.Yil),0) Agustos,
	ISNULL((SELECT SUM(VT.SipMiktar) FROM vTotalOrder VT WHERE VT.Ay = 9 AND CL.YEAR = VT.Yil),0) Eylul,
	ISNULL((SELECT SUM(VT.SipMiktar) FROM vTotalOrder VT WHERE VT.Ay = 10 AND CL.YEAR = VT.Yil),0) Ekim,
	ISNULL((SELECT SUM(VT.SipMiktar) FROM vTotalOrder VT WHERE VT.Ay = 11 AND CL.YEAR = VT.Yil),0) Kasim,
	ISNULL((SELECT SUM(VT.SipMiktar) FROM vTotalOrder VT WHERE VT.Ay = 13 AND CL.YEAR = VT.Yil),0) Aralik
	FROM (SELECT DISTINCT DATEPART(YYYY,CL.CalendarDate) Year FROM Calendar CL) CL
	--ORDER BY CL.Year






	
GO


