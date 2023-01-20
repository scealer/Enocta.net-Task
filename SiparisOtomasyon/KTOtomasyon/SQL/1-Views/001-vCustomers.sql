USE [KTOtomasyon]
GO


DROP VIEW [dbo].[vCustomers]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vCustomers]
AS
SELECT  O.CustomerName,
		O.PhoneNumber 
FROM Orders O
GROUP BY O.Order_Id,O.PhoneNumber,O.CustomerName 
GO


