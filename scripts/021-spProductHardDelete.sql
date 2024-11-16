CREATE TYPE IntList AS TABLE 
(
    [Value] INT
);
GO
CREATE procedure spProductsHardDelete
@productIds IntList READONLY
AS
BEGIN
    DECLARE @rowProducts CURSOR
    SET @rowProducts = CURSOR FOR SELECT Products.ProductId
        FROM Products INNER JOIN @productIds AS p ON Products.ProductId = p.[Value]
    open @rowProducts
    DECLARE @ProductsProductId INT
    DECLARE @rowCountProducts INT = (SELECT COUNT(*) FROM @productIds)
    DECLARE @i INT = 0
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    BEGIN TRAN
    WHILE @i < @rowCountProducts
    BEGIN
        fetch NEXT FROM @rowProducts INTO @ProductsProductId
        BEGIN try
			DELETE FROM Products WHERE current of @rowProducts
		END try
        BEGIN catch
			ROLLBACK;
            close @rowProducts;
            THROW;
		END catch
        SET @i += 1
    END
    COMMIT
    close @rowProducts
END
GO