USE LosAgilesDB
GO

ALTER TABLE Orders ADD ReceivedDate Date 
GO

ALTER TABLE Orders
ADD StatusChangeDate DATETIME NOT NULL DEFAULT GETDATE();
GO

CREATE NONCLUSTERED INDEX IX_Orders_StatusChangeDate
ON Orders (StatusChangeDate);
GO

CREATE TRIGGER trg_UpdateStatusChangeDate
ON Orders
AFTER UPDATE
AS
BEGIN
    IF UPDATE(Status)
    BEGIN
        UPDATE Orders
        SET StatusChangeDate = GETDATE()
        FROM Orders o
        INNER JOIN Inserted i ON o.OrderID = i.OrderID
        INNER JOIN Deleted d ON o.OrderID = d.OrderID
        WHERE d.Status <> i.Status; 
    END
END;
GO
