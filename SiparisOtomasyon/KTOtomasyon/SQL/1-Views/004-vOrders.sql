USE [KTOtomasyon]
GO


DROP VIEW [dbo].[vOrders]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE View [dbo].[vOrders]
as
SELECT Order_Id, CustomerName, PhoneNumber,Debt,Addition,OrderDate,IsDelivered,Discount,(Select sum(TotalPrice) from OrderDetail d where d.Order_Id = Orders.Order_Id) as NetTotal,((Select sum(TotalPrice) from OrderDetail d where d.Order_Id = Orders.Order_Id)-(Select sum(TotalPrice) from OrderDetail d where d.Order_Id = Orders.Order_Id)*Discount/100) TotalPrice
FROM dbo.Orders 
WHERE Orders.IsDeleted = '0'
GO


