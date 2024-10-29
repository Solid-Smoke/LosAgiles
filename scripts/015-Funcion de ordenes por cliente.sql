USE [LosAgilesDB]
GO

CREATE FUNCTION udfOrdersByClientID ( @Id INT )
RETURNS TABLE AS RETURN (
SELECT Orders.OrderID,
	   Orders.CreatedDate,
	   Orders.DeliveryDate,
	   SUM(Products.Price * OrderProducts.Amount) AS TotalAmount,
	   Orders.[Status],
	   CONCAT(ClientsAddresses.Province, ', ',
			  ClientsAddresses.Canton, ', ',
			  ClientsAddresses.District, ', ',
			  ClientsAddresses.OtherSigns, ', ',
			  ClientsAddresses.PostalCode
		) AS [Address]
FROM Orders
INNER JOIN OrderProducts ON OrderProducts.OrderID = Orders.OrderID
INNER JOIN Products ON Products.ProductID = OrderProducts.ProductID
INNER JOIN ClientsAddresses ON ClientsAddresses.AddressID = Orders.DeliveryAddress
WHERE Orders.ClientID = @Id
GROUP BY Orders.OrderID,
		Orders.CreatedDate,
		Orders.DeliveryDate,
		Orders.[Status],
		ClientsAddresses.Province,
		ClientsAddresses.Canton,
		ClientsAddresses.District,
		ClientsAddresses.OtherSigns,
		ClientsAddresses.PostalCode
);
GO