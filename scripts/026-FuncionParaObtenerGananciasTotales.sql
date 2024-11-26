USE LosAgilesDB
GO

CREATE FUNCTION [dbo].[GetBusinessEarningsReportByMonthAndYear]
(
    @Year INT,
    @BusinessID INT = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        [b].[Name] AS [BusinessName],
        FORMAT([o].[StatusChangeDate], 'MM') AS [Month],
        SUM([op].[Amount] * [op].[PriceAtTimeOfPurchase]) AS [TotalPurchase], 
        SUM([o].[DeliveryCost]) AS [DeliveryCost],
        SUM([o].[TotalCost]) AS [TotalCost] 
    FROM 
        [dbo].[BusinessOrders] [bo]
    INNER JOIN 
        [dbo].[Orders] [o] ON [bo].[OrderID] = [o].[OrderID]
    INNER JOIN 
        [dbo].[Businesses] [b] ON [bo].[BusinessID] = [b].[BusinessID]
    INNER JOIN
        [dbo].[OrderProducts] [op] ON [o].[OrderID] = [op].[OrderID]
    WHERE 
        [o].[Status] = 'Completada' 
        AND YEAR([o].[StatusChangeDate]) = @Year 
        AND (@BusinessID IS NULL OR [b].[BusinessID] = @BusinessID) 
    GROUP BY 
        [b].[Name],
        FORMAT([o].[StatusChangeDate], 'MM') 
);
GO