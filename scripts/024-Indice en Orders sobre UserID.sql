USE LosAgilesDB
GO

CREATE NONCLUSTERED INDEX IX_Orders_ClientID
ON Orders (ClientID);
GO