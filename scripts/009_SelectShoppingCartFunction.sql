USE [LosAgilesDB]
GO

DROP FUNCTION IF EXISTS udfSelectShoppingCart
GO

CREATE FUNCTION udfSelectShoppingCart (
    @Id INT
)
RETURNS TABLE
AS
RETURN
    SELECT 
		[p].[ProductID], [p].[Name] AS [ProductName], [b].[Name] AS [BusinessName], [sp].[Amount], [p].[Price], SUM([sp].[Amount] * [p].[Price]) AS [TotalSales]
		, [p].[Weight], [p].[BusinessID]
	FROM 
		[ShoppingCarts] [sp]
	INNER JOIN 
		[Products] [p] ON [p].[ProductID] = [sp].[ProductId]
	INNER JOIN 
		[Businesses] [b] ON [b].[BusinessID] = [p].[BusinessID]
	WHERE 
		[sp].[ClientID] = @Id AND [p].[IsDeleted] = 0
	GROUP BY 
		[b].[Name], [p].[Name],  [sp].[Amount], [p].[Price], [p].[ProductID]
		, [p].[Weight], [p].[BusinessID]
GO
