USE LosAgilesDB
GO

CREATE Procedure spInsertBusinessOrders @orderIDParam INT, @BusinessIDParam INT
AS
BEGIN
INSERT INTO BusinessOrders (OrderID, BusinessID) VALUES (@orderIDParam, @BusinessIDParam)
END
GO
