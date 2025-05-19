CREATE PROCEDURE spReportRevenuePerCategory
AS
BEGIN
    WITH OrderTotal AS (
        SELECT SUM(TotalCost) AS TotalRevenue
        FROM [Order]
        WHERE Status <> 16 -- Exclude canceled orders
    )
    
    SELECT
        c.Name AS CategoryName,
        COUNT(DISTINCT od.OrderID) AS OrderCount,
        SUM(od.Quantity * od.UnitPrice) AS Revenue,
        FORMAT(SUM(od.Quantity * od.UnitPrice), 'N2') + ' Lei' AS RevenueFormatted,
        CASE
            WHEN (SELECT TotalRevenue FROM OrderTotal) > 0
            THEN CAST(SUM(od.Quantity * od.UnitPrice) * 100 / (SELECT TotalRevenue FROM OrderTotal) AS DECIMAL(10,2))
            ELSE 0
        END AS PercentageOfTotal
    FROM
        OrderDish od
        INNER JOIN Preparat p ON od.PreparatID = p.PreparatID
        INNER JOIN Category c ON p.CategoryID = c.CategoryId
        INNER JOIN [Order] o ON od.OrderID = o.OrderID
    WHERE
        o.Status <> 16 -- Exclude canceled orders
    GROUP BY
        c.Name
    ORDER BY
        Revenue DESC
END
GO