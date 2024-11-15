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
    SET TRANSACTION ISOLATION LEVEL READ COMMITTED
    BEGIN TRAN
    WHILE @i < @rowCountProducts
    BEGIN
        fetch NEXT FROM @rowProducts INTO @ProductsProductId
        BEGIN try
			DELETE FROM Products WHERE current of @rowProducts
		END try
        BEGIN catch
			DECLARE @foreign_key_error INT = 547
            IF ERROR_NUMBER() = @foreign_key_error
            BEGIN
				UPDATE Products SET Products.IsDeleted = 1 WHERE current of @rowProducts
			END
			ELSE
            BEGIN
                ROLLBACK;
                close @rowProducts;
				THROW;
			END
		END catch
        SET @i += 1
    END
    COMMIT
    close @rowProducts
END
GO