USE LosAgilesDB;
GO

ALTER TABLE OrderProducts ADD PriceAtTimeOfPurchase decimal
GO

CREATE TRIGGER trg_InsertPriceAtTimeOfPurchase
ON OrderProducts
AFTER INSERT
AS
BEGIN
    UPDATE OrderProducts
    SET OrderProducts.PriceAtTimeOfPurchase = Products.Price
    FROM OrderProducts
    INNER JOIN Products ON OrderProducts.ProductID = Products.ProductID
    INNER JOIN inserted ON OrderProducts.OrderID = inserted.OrderID
	AND OrderProducts.ProductID = inserted.ProductID;
END;
GO
