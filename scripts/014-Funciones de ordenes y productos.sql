USE [LosAgilesDB]
GO

CREATE FUNCTION dbo.udfGetPendingOrders ()
RETURNS TABLE AS RETURN (
SELECT Orders.OrderID,
	   CONCAT(Clients.Name, ' ', Clients.LastNames) AS Buyer,
	   Orders.CreatedDate,
	   SUM(Products.Price * OrderProducts.Amount) AS TotalAmount,
	   CONCAT(ClientsAddresses.Province, ', ',
			  ClientsAddresses.Canton, ', ',
			  ClientsAddresses.District, ', ',
			  ClientsAddresses.OtherSigns, ', ',
			  ClientsAddresses.PostalCode
		) AS [Address]
FROM Orders
INNER JOIN Clients ON Clients.UserID = Orders.ClientID
INNER JOIN OrderProducts ON OrderProducts.OrderID = Orders.OrderID
INNER JOIN Products ON Products.ProductID = OrderProducts.ProductID
INNER JOIN ClientsAddresses ON ClientsAddresses.AddressID = Orders.DeliveryAddress
WHERE Orders.Status = 'Pendiente'
GROUP BY Orders.OrderID,
		Clients.Name,
		Clients.LastNames,
		Orders.CreatedDate,
		ClientsAddresses.Province,
		ClientsAddresses.Canton,
		ClientsAddresses.District,
		ClientsAddresses.OtherSigns,
		ClientsAddresses.PostalCode
);
GO

CREATE FUNCTION udfProductsByOrderID ( @Id INT )
RETURNS TABLE AS RETURN
	SELECT Products.Name, OrderProducts.Amount
	FROM OrderProducts
	INNER JOIN Products ON Products.ProductID = OrderProducts.ProductID
	WHERE OrderProducts.OrderID = @Id;
GO
