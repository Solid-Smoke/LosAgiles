USE LosAgilesDB;
GO

CREATE TABLE Orders (
	OrderID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Status] varchar(12) NOT NULL DEFAULT 'Pendiente',
	CreatedDate Date NOT NULL DEFAULT GetDate(),
	DeliveryDate Date,
	ClientID INT NOT NULL FOREIGN KEY REFERENCES Clients(UserID),
	DeliveryAddress INT NOT NULL FOREIGN KEY REFERENCES ClientsAddresses(AddressID),
	CHECK (Status IN ('Pendiente', 'Aprobada', 'Rechazada', 'En envio', 'Completada'))
);
GO

CREATE TRIGGER trg_UpdateDeliveryDate ON Orders AFTER UPDATE AS
BEGIN
    UPDATE Orders SET
	DeliveryDate = GETDATE()
    WHERE OrderID IN (
        SELECT OrderID FROM inserted
        WHERE [Status] = 'Completada'
    );
END;
GO