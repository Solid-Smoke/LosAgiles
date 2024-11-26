USE LosAgilesDB
GO

CREATE FUNCTION [GetBusinessEarningsReportByMonthAndYear] (@Year INT, @BusinessID INT = NULL)
RETURNS TABLE
AS
RETURN
(
    SELECT
        
b.Name
 AS [BusinessName],
        MONTH(o.CreatedDate) AS [Month],
        SUM(op.Amount * p.Price) AS [TotalPurchase],
        SUM(o.DeliveryCost) AS [DeliveryCost],
		SUM((op.Amount * p.Price)+o.DeliveryCost) AS [TotalCost]
    FROM [dbo].[Orders] o
    JOIN [dbo].[OrderProducts] op ON o.OrderID = op.OrderID
    JOIN [dbo].[Products] p ON op.ProductID = p.ProductID
    JOIN [dbo].[Businesses] b ON p.BusinessID = b.BusinessID
    WHERE YEAR(o.CreatedDate) = @Year
    AND (@BusinessID IS NULL OR p.BusinessID = @BusinessID)
    AND Status = 'Completada'
    GROUP BY 
b.Name
, MONTH(o.CreatedDate)
);
GO