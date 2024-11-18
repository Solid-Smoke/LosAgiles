USE LosAgilesDB;
GO

CREATE TRIGGER trg_RestoreStockOnRechazada ON Orders AFTER UPDATE AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted WHERE [Status] = 'Rechazada')
    BEGIN
        DECLARE @OrderID INT;
        DECLARE OrderCursor CURSOR FOR
            SELECT OrderID FROM inserted WHERE [Status] = 'Rechazada';

        OPEN OrderCursor;
        FETCH NEXT FROM OrderCursor INTO @OrderID;
        WHILE @@FETCH_STATUS = 0
        BEGIN
            UPDATE Products
            SET Products.Stock += OrderProducts.Amount
            FROM Products
            INNER JOIN OrderProducts ON Products.ProductID = OrderProducts.ProductID
            WHERE OrderProducts.OrderID = @OrderID;

            FETCH NEXT FROM OrderCursor INTO @OrderID;
        END

        CLOSE OrderCursor;
        DEALLOCATE OrderCursor;
    END
END;
GO

CREATE TRIGGER trg_PreventStatusChangeOnFinalStatus ON Orders AFTER UPDATE AS
BEGIN
    IF EXISTS (SELECT 1 FROM deleted
        JOIN inserted ON deleted.OrderID = inserted.OrderID
        WHERE deleted.[Status] = 'Rechazada' AND inserted.[Status] != 'Rechazada' OR
		deleted.[Status] = 'Completada' AND inserted.[Status] != 'Completada')
    BEGIN
        RAISERROR('Una vez una orden esta en un estado final este no puede cambiar.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END
GO