CREATE TABLE [ShoppingCarts] (
    [ClientID] int NOT NULL,
    [ProductID] int NOT NULL,
    [Amount] int,
    CONSTRAINT [PK_ShoppingCarts] PRIMARY KEY([ClientID], [ProductID]),
    CONSTRAINT [FK_ShoppingCarts_ClientID] FOREIGN KEY([ClientID])
    REFERENCES [Clients]([UserID]),
    CONSTRAINT [FK_ShoppingCarts_ProductID] FOREIGN KEY([ProductID])
    REFERENCES [Products]([ProductID])
);
GO